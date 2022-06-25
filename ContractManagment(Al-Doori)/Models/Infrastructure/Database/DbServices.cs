using ClosedXML.Excel;
using ContractManagment_Al_Doori_.Models.Entities;
using ContractManagment_Al_Doori_.Models.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using ClosedXML;

namespace ContractManagment_Al_Doori_.Models.Infrastructure.Database
{
    public class DbServices
    {


        #region Constructor and Dependecy Injection 
        private readonly ContractDbContext _contractDbContext;
        public DbServices(ContractDbContext contractDbContext)
        {
            _contractDbContext = contractDbContext;
        }


        #endregion

        #region Method to Return Contracts that matches Search Term 
        public async Task<IList<Contract>> GetSpecificContracts(string searchTerm)
        {
            return await _contractDbContext.Contracts.Where(cs => cs.Institution.Contains(searchTerm)).ToListAsync();
        }
        #endregion

        #region Method to Delete Contract and it is related Data From the Database 
        public async Task DeleteContract(int contractID)
        {
            if (!String.IsNullOrEmpty(contractID.ToString()))
            {
                //Getting the Data for (Contract, AdvisorContract) 
                var ContractToDelete = _contractDbContext.Contracts.FirstOrDefault(x => x.Id == contractID);
                var ContractToDeleteAdvisors = _contractDbContext.AdvisorContracts.Where(x => x.ContractID == contractID);

                if (ContractToDelete != null)
                {
                    //[1] Remove AdvisorContract 
                    foreach (var ContractToDeleteAdvisor in ContractToDeleteAdvisors)
                    {
                        _contractDbContext.AdvisorContracts.Remove(ContractToDeleteAdvisor);
                    }
                    //[2] Remove Contract
                    _contractDbContext.Contracts.Remove(ContractToDelete);

                    //[3] Save Changes to the DbSets 
                    _contractDbContext.SaveChanges();
                }
            }
        }
        #endregion

        #region Method To Return Client that's contains search name 
        public async Task<IList<Client>> getSpecficClient(string clientSearchName)
        {
            return await _contractDbContext.Clients.Where(cl => cl.Name.Contains(clientSearchName)).ToListAsync();
        }
        #endregion

        #region Method to Filter Clients by spefic filtering type 
        public async Task<IList<Client>> getFilteredClients(string filterType)
        {
            IList<Client> clients = await _contractDbContext.Clients.ToListAsync();
            switch (filterType)
            {
                case "Name":
                    clients = await _contractDbContext.Clients.OrderBy(cl => cl.Name).ToListAsync();
                    break;
                case "SurName":
                    clients = await _contractDbContext.Clients.OrderBy(cl => cl.SurName).ToListAsync();
                    break;
                case "Age":
                    clients = await _contractDbContext.Clients.OrderBy(cl => cl.Age).ToListAsync();
                    break;
            }


            return clients;
        }
        #endregion

        #region Method To Return Advisor that's contains search name 
        public async Task<IList<Advisor>> getSpecficAdvisor(string advisorSearchTerm)
        {
            return await _contractDbContext.Advisors.Where(cl => cl.Name.Contains(advisorSearchTerm)).ToListAsync();
        }
        #endregion

        #region Method to Filter Advisors by spefic filtering type 
        public async Task<IList<Advisor>> getFilteredAdvisors(string filterType)
        {
            IList<Advisor> advisors = await _contractDbContext.Advisors.ToListAsync();
            switch (filterType)
            {
                case "Name":
                    advisors = await _contractDbContext.Advisors.OrderBy(cl => cl.Name).ToListAsync();
                    break;
                case "SurName":
                    advisors = await _contractDbContext.Advisors.OrderBy(cl => cl.SurName).ToListAsync();
                    break;
                case "Age":
                    advisors = await _contractDbContext.Advisors.OrderBy(cl => cl.Age).ToListAsync();
                    break;
            }


            return advisors;
        }
        #endregion

        #region Method to Export Clients Datas As Excel Sheet or CSV

        public async Task<FileContentResult> ExportClientsToExcel()
        {
            //[1] Creating the datatable 
            DataTable dataTable = new DataTable("Grid");
            dataTable.Columns.AddRange(
                new DataColumn[6]
                {
                    new DataColumn("Name"),
                    new DataColumn("SurName"),
                    new DataColumn("Email"),
                    new DataColumn("Phone Number"),
                    new DataColumn("NIN"),
                    new DataColumn("Age")
                });

            //[2] Gettings all the clients 
            //[3] Append all the clientess to the datatable
            foreach (var client in await _contractDbContext.Clients.ToListAsync())
            {
                dataTable.Rows.Add(client.Name, client.SurName, client.Email, client.PhoneNumber, client.NationalIdentificationNumber, client.Age);
            }
            //[4] Create Excel Sheet and append the datatable to it using the ClosedXML Library
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    //we can change format to csv or .xlsx as wanted
                    FileContentResult fileContentResult = new FileContentResult(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    fileContentResult.FileDownloadName = "Clients.xlsx";
                    return fileContentResult;
                }
            }
        }

        #endregion

        #region Export Advisors To Excel Sheet or CSV 
        public async Task<FileContentResult> ExportAdvisorsToExcel()
        {
            //[1] Creating the datatable 
            DataTable dataTable = new DataTable("Grid");
            dataTable.Columns.AddRange(
                new DataColumn[6]
                {
                    new DataColumn("Name"),
                    new DataColumn("SurName"),
                    new DataColumn("Email"),
                    new DataColumn("Phone Number"),
                    new DataColumn("NIN"),
                    new DataColumn("Age")
                });

            //[2] Gettings all the clients 
 

            //[3] Append all the clientess to the datatable
            foreach (var advisor in await _contractDbContext.Advisors.ToListAsync())
            {
                dataTable.Rows.Add(advisor.Name, advisor.SurName, advisor.Email, advisor.PhoneNumber, advisor.NationalIdentificationNumber, advisor.Age);
            }
            //[4] Create Excel Sheet and append the datatable to it using the ClosedXML Library
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(dataTable);
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    //we can change format to csv or .xlsx as wanted
                    FileContentResult fileContentResult = new FileContentResult(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    fileContentResult.FileDownloadName = "Advisors.xlsx";
                    return fileContentResult;
                }
            }
        }
        #endregion

        #region Export Contracts to Excel Sheet or CSV
        public async Task<FileContentResult> ExportContractsToExcel()
        {
            //[1] Creating the datatable for contracts
            DataTable contractsTable = new DataTable("Grid");
            contractsTable.Columns.AddRange(
                new DataColumn[8]
                {
                    new DataColumn("Registeration Number"),
                    new DataColumn("Institution"),
                    new DataColumn("Client Name"),
                    new DataColumn("Advisor Name"),
                    new DataColumn("Advisor is Admin"),
                    new DataColumn("Conclusion Date"),
                    new DataColumn("Validity Date"),
                    new DataColumn("Terminiation Date")
                });


            //[2] Gettings all the contracts with the mapped data    
            //[3] Append all the contracts to the datatable
            foreach (var contract in await _contractDbContext.Contracts.ToListAsync())
            {
                foreach (var contractAdvisor in contract.ContractAdvisors)
                {
                    contractsTable.Rows.Add(contract.RegisterationNumber, contract.Institution, contract.Client.Name, contractAdvisor.Advisor.Name,
                        contractAdvisor.isAdminstrator == 1 ? "Adminstrator" : "Not AdminStrator", contract.ConclusionDate, contract.ValidityDate, contract.TerminationDate);
                }
            }
            //[4] Create Excel Sheet and append the datatable to it using the ClosedXML Library
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(contractsTable);
                using (MemoryStream ms = new MemoryStream())
                {
                    wb.SaveAs(ms);
                    //we can change format to csv or .xlsx as wanted
                    FileContentResult fileContentResult = new FileContentResult(ms.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");
                    fileContentResult.FileDownloadName = "Contracts.xlsx";
                    return fileContentResult;
                }
            }
        }
        #endregion

    }
}
