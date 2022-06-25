using Microsoft.AspNetCore.Mvc;
using ContractManagment_Al_Doori_.Models.Entities;
using ContractManagment_Al_Doori_.Models.Entities.Identity;
using ContractManagment_Al_Doori_.Models.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using ContractManagment_Al_Doori_.Areas.Admin.Models.ViewModels;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace ContractManagment_Al_Doori_.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ContractController : Controller
    {
        #region Dependency Injection + Constructor + Proprties
        ContractDbContext _contractDbContext;
        static IList<Client> clients;
        static IList<Advisor> advisors;
        static IList<AdvisorContract> ContractAdvisors;
        public ContractController(ContractDbContext contractDbContext)
        {
            _contractDbContext = contractDbContext;
             
        }
        #endregion
        
        #region Method to get mapped data/update

        public async Task getMappedData()
        {
            clients = await _contractDbContext.Clients.ToListAsync();
            ContractAdvisors = await _contractDbContext.AdvisorContracts.ToListAsync();
            advisors = await _contractDbContext.Advisors.ToListAsync();
        }

        #endregion

        #region Edit Action and Delete Methods 
        public async Task<IActionResult> Edit(int id)
        {
            //[1] - Return the Edit View with the Selected Contract
            await getMappedData();
            var SelectedContract = _contractDbContext.Contracts.FirstOrDefault(x => x.Id == id);
            ContractViewModelEdit contractViewModelEdit = new ContractViewModelEdit();
            contractViewModelEdit.clients = clients;
            contractViewModelEdit.advisors = advisors;
            contractViewModelEdit.contract = SelectedContract;
            return View(contractViewModelEdit);

        }

        //Delete Advisor from contract means delete from AdvisorContract to the corresponding contract
        public async Task<IActionResult> deleteAdvisorFromContract(string advisorNameNContractID)
        {
            var resultArray = advisorNameNContractID.Split(';');
            var contractID = int.Parse(resultArray[0]);
            var advisorName = resultArray[1];

            //Remove from contractAdvisor table the corresponding values 
            var advisor  = _contractDbContext.Advisors.FirstOrDefault(x => x.Name == advisorName);
            var advisorContract = _contractDbContext.AdvisorContracts.FirstOrDefault(x => x.ContractID == contractID && x.AdvisorID == advisor.AdvisorId);
            _contractDbContext.AdvisorContracts.Remove(advisorContract);
            _contractDbContext.SaveChanges();

            //update and get linkeddata
            await getMappedData();
            var returnContract = _contractDbContext.Contracts.FirstOrDefault(x => x.Id == contractID);
            ContractViewModelEdit viewModelEdit= new ContractViewModelEdit();
            viewModelEdit.advisors = advisors;
            viewModelEdit.clients = clients;
            viewModelEdit.AddAdvisorViewModel = new AddAdvisorViewModel();
            viewModelEdit.contract = returnContract;
            return RedirectToAction("Edit", new {Id = returnContract.Id});
        }

        [HttpPost]
        public async Task<IActionResult> EditContract(ContractViewModelEdit contractViewModelEdit)
        {
            
            return View("Edit", contractViewModelEdit);
        }

        #endregion
        
        #region Method to add Advisor to specfic Contract
        [HttpPost]
        public async Task<IActionResult> AddAdvisorToContract(ContractViewModelEdit contractViewModelEdit) {
           
            //-- Check if the advisor already exists in the database
            var AdvsiorContract =  _contractDbContext.AdvisorContracts
                .FirstOrDefault(advisorContract => advisorContract.Advisor.AdvisorId ==
                                          int.Parse(contractViewModelEdit.AddAdvisorViewModel.AdvisorID)
                                          &&
                                          (contractViewModelEdit.contract.Id ==
                                          advisorContract.ContractID)
                                          );
            
            if (AdvsiorContract == null)
            {
                //1-- add advisor to the database with the speicifed role and contract id 
                _contractDbContext.AdvisorContracts
                    .Add(new AdvisorContract
                    {
                        ContractID = contractViewModelEdit.contract.Id,
                        AdvisorID = int.Parse(contractViewModelEdit.AddAdvisorViewModel.AdvisorID)
                    });
                //2-- Save Changes to the DbContext or the rep 
                _contractDbContext.SaveChanges();
                await getMappedData();
                var returnContract =
                    _contractDbContext.Contracts.FirstOrDefault(x => x.Id == contractViewModelEdit.contract.Id);


                //3- fill the viewModel and return it 

                contractViewModelEdit.clients = clients;
                contractViewModelEdit.contract = returnContract;
                contractViewModelEdit.advisors = advisors;
                //problem list or generate unrepaeated addadvisors
                contractViewModelEdit.AddAdvisorViewModel = new AddAdvisorViewModel();

                return RedirectToAction("Edit",new {Id = returnContract.Id});
            }
            //else advisorContract is Exists in the database    
            await getMappedData();
            var contracttoReturn =
                _contractDbContext.Contracts.FirstOrDefault(x => x.Id == contractViewModelEdit.contract.Id);
            contractViewModelEdit.clients = clients;
            contractViewModelEdit.contract = contracttoReturn;
            contractViewModelEdit.advisors = advisors;
            //problem list or generate unrepaeated addadvisors
            contractViewModelEdit.AddAdvisorViewModel = new AddAdvisorViewModel();

            return View("Edit", contractViewModelEdit);
            
        }
        #endregion
    }
}
