using ContractManagment_Al_Doori_.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using ContractManagment_Al_Doori_.Models.Entities;
using ContractManagment_Al_Doori_.Models.Infrastructure.Database;
using ContractManagment_Al_Doori_.Models.ViewModels;
using ClosedXML;
using Microsoft.EntityFrameworkCore;
using System.Data;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;
using ContractManagment_Al_Doori_.Models.Entities.Identity;

namespace ContractManagment_Al_Doori_.Controllers
{
    public class HomeController : Controller
    {

        #region Proprties and LiveData 
        static IList<Client> clients;
        static IList<Advisor> advisors;
        static IList<AdvisorContract> ContractAdvisors;
        DbServices DBservices;


        #endregion

        #region Constructor and Dependency Injection
        private readonly ILogger<HomeController> _logger;
        static ContractDbContext _contractDbContext;

        public HomeController(ILogger<HomeController> logger, ContractDbContext contractDbContext)
        {
            _logger = logger;
            _contractDbContext = contractDbContext;
            DBservices = new DbServices(contractDbContext);
        }
        #endregion

        #region Contract Action Method 

        [HttpPost]
        public async Task<IActionResult> Contract(ContractViewModel contractViewModel)
        {
            if (ModelState.ErrorCount <= 1)
            {
                await getMappedData();
                contractViewModel.contracts = await DBservices.GetSpecificContracts(contractViewModel.InstitueNameSearch);
                return View(contractViewModel);
            }
            else
            {
                await getMappedData();
                contractViewModel.contracts = await _contractDbContext.Contracts.ToListAsync(); ;
                return View(contractViewModel);

            }
        }


        public async Task<IActionResult> Contract()
        {

            //it is nesccary to bring all the content from the database sso Everything can be mapped automatically to the contracts list and proprties
            await getMappedData();
            ContractViewModel contractViewModel = new ContractViewModel();
            contractViewModel.contracts = await _contractDbContext.Contracts.ToListAsync(); ;
            contractViewModel.InstitueNameSearch = "";

            return View(contractViewModel);

        }

        #endregion

        #region Contract Delete Action Method 
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> DeleteContract(int contractId)
        {
            if (!String.IsNullOrEmpty(contractId.ToString()))
            {
                //[1] Delete Contract From the Database 
                await DBservices.DeleteContract(contractId);

                //[2] Return to the contracts Page  
                return RedirectToAction(nameof(Contract));

            }
            return RedirectToAction(nameof(Contract));

        }
        #endregion

        #region Client Action Methods 

        /// <summary>
        /// This Action Method Used for the search bar to retunr admired client name 
        /// </summary>
        /// <param name="clientViewModel"></param>
        /// <returns>View with the list of client that matches the search</returns>
        [HttpPost, ActionName("SearchAction")]
        public async Task<IActionResult> Client(ClientViewModel clientViewModel)
        {
            //Cancel Validation for Filter box (because we want only to submit search bar)
            ModelState["filterByChoice"].Errors.Clear();

            clients = await _contractDbContext.Clients.ToListAsync();
            if (!String.IsNullOrEmpty(clientViewModel.ClientSearchName))
            {
                clients = await DBservices.getSpecficClient(clientViewModel.ClientSearchName);
                clientViewModel._clients = clients;
                return View("Client", clientViewModel);
            }
            clientViewModel._clients = clients;
            return View("Client", clientViewModel);

        }
        /// <summary>
        /// Action Method to get filtered clients and filled them to the view
        /// </summary>
        /// <param name="clientViewModel"></param>
        /// <returns>Client View Model with the filtered result</returns>

        [HttpPost]
        public async Task<IActionResult> FilterAction(ClientViewModel clientViewModel)
        {
            //Cancel Validation for Search Field, because we want to filter only
            ModelState["ClientSearchName"].Errors.Clear();
            clientViewModel._clients = await DBservices.getFilteredClients(clientViewModel.filterByChoice);
            return View("Client", clientViewModel);
        }

        /// <summary>
        /// Action Method Return the called View
        /// </summary>
        /// <returns>Clients View Component</returns>
        public async Task<IActionResult> Client()
        {
            IList<Client> clients = await _contractDbContext.Clients.ToListAsync();
            ClientViewModel clientViewModel = new ClientViewModel();
            clientViewModel._clients = clients;
            clientViewModel.ClientSearchName = "";
            return View(clientViewModel);
        }
        #endregion

        #region Advisor Action Method 


        /// <summary>
        /// This Action Method Used for the search bar to retunr admired advisor name 
        /// </summary>
        /// <param name="advisorViewModel"></param>
        /// <returns>View with the list of advisors that matches the search</returns>
        [HttpPost, ActionName("SearchActionAdvisor")]
        public async Task<IActionResult> Advisor(AdvisorViewModel advisorViewModel)
        {
            //Cancel Validation for Filter box (because we want only to submit search bar)
            ModelState["filterByChoice"].Errors.Clear();

            IList<Advisor> advisors = await _contractDbContext.Advisors.ToListAsync();
            if (!String.IsNullOrEmpty(advisorViewModel.AdvisorSearchName))
            {
                advisors = await DBservices.getSpecficAdvisor(advisorViewModel.AdvisorSearchName);
                advisorViewModel.advisors = advisors;
                return View("Advisor", advisorViewModel);
            }
            advisorViewModel.advisors = advisors;

            return View("Advisor", advisorViewModel);

        }


        /// <summary>
        /// Action Method to get filtered advisors and filled them to the view
        /// </summary>
        /// <param name="advisorViewModel"></param>
        /// <returns>Client View Model with the filtered result</returns>

        [HttpPost]
        public async Task<IActionResult> FilterActionAdvisor(AdvisorViewModel advisorViewModel)
        {
            //Cancel Validation for Search Field, because we want to filter only
            ModelState["AdvisorSearchName"].Errors.Clear();

            advisorViewModel.advisors = await DBservices.getFilteredAdvisors(advisorViewModel.filterByChoice);
            return View("Advisor", advisorViewModel);
        }




        /// <summary>
        /// Action Method Return the called View
        /// </summary>
        /// <returns>Advisor View Component</returns>
        public async Task<IActionResult> Advisor()
        {
            IList<Advisor> advisors = await _contractDbContext.Advisors.ToListAsync();
            AdvisorViewModel advisorViewModel = new AdvisorViewModel();
            advisorViewModel.advisors = advisors;
            advisorViewModel.AdvisorSearchName = "";
            return View(advisorViewModel);
        }

        #endregion

        #region Export Clients to Excel Sheet Action Method 
        /// <summary>
        /// Action Method Used to conver the clients to an Excel sheet or .csv using ClosedXML library
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ExportClientsToExcel()
        {
            return await DBservices.ExportClientsToExcel();
        }
        #endregion

        #region Export Advisors to Excel Sheet Action Method 
        /// <summary>
        /// Action Method Used to conver the Advisors to an Excel sheet or .csv using ClosedXML library
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ExportAdvisorsToExcel()
        {
            return await DBservices.ExportAdvisorsToExcel();
        }
        #endregion

        #region Export Contracts To Excel Sheet Action Method

        /// <summary>
        /// Action Method Used to conver the clients to an Excel sheet or .csv using ClosedXML library
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ExportContractsToExcel()
        {
            await getMappedData();
            return await DBservices.ExportContractsToExcel();
        }

        #endregion

        #region SplashScreen Action Method
        public async Task<IActionResult> SplashScreen()
        {
            return View();
        }

        #endregion

        #region Method to get mapped data/update

        public static async Task getMappedData()
        {
            clients = await _contractDbContext.Clients.ToListAsync();
            advisors = await _contractDbContext.Advisors.ToListAsync();
            ContractAdvisors = await _contractDbContext.AdvisorContracts.ToListAsync();
        }

        #endregion

        #region Error Action Method
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion
    }
}