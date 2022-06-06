using Microsoft.AspNetCore.Mvc;
using ContractManagment_Al_Doori_.Models.ApplicationServices.Abstraction;
using ContractManagment_Al_Doori_.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using ContractManagment_Al_Doori_.Models.Entities.Identity;
using ContractManagment_Al_Doori_.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNet.Identity;

namespace ContractManagment_Al_Doori_.Areas.Security.Controllers
{
    //Addign Area Routing
    [Area("Security")]
    public class AccountController : Controller
    {

        #region Constructor and Dependecny Inejction
        ISecurityApplicationService _securityApplicationService;

        public AccountController(ISecurityApplicationService securityApplicationService)
        {
            _securityApplicationService = securityApplicationService;
        }

        #endregion

        #region Login Action Methods


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if(ModelState.IsValid)
            {
                bool issSuccessfullyLogIn = await _securityApplicationService.Login(loginViewModel);
                if(issSuccessfullyLogIn)
                {
                    return RedirectToAction("Contract","Home",new {area = String.Empty});
                }
                //else [Increase LoginFailed Attemp]
                loginViewModel.LoginFailed = true;
            }
            //[else wrong Inputted Data] => server side validation :)  
            return View(loginViewModel);
        }


        //Return Normal View On Navigation
        public IActionResult Login()
        {
            //We don't want the authenticated user be able to access the login end point because he already signedin
            if (User.Identity.IsAuthenticated)
            {
                return NotFound();
            }
            else
            {
                return View();
            }
        }
        #endregion

        #region Logout Action Method
        public async Task<IActionResult> Logout()
        {
            await _securityApplicationService.Logout();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region AcountSettings Action Methods

        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AccountSettings()
        {
            User currentUser = await _securityApplicationService.GetCurrentUser(User);
            SettingsViewModel settings = new SettingsViewModel(currentUser.FirstName,currentUser.LastName,currentUser.PhoneNumber,currentUser.Email,currentUser.UserName);
            return View(settings);
        }
        #endregion

        #region Action Method to SaveNewSettings
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> SaveNewSettings(SettingsViewModel settingsViewModel)
        {
            //fake validation
            ModelState["PhoneNumber"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            if (ModelState.IsValid)
            {
                var user = await _securityApplicationService.GetCurrentUser(User);
                user.FirstName = settingsViewModel.FirstName;
                user.LastName = settingsViewModel.LastName;
                user.PhoneNumber = settingsViewModel.PhoneNumber;
                user.Email = settingsViewModel.Email;
                user.UserName = settingsViewModel.UserName;

                await _securityApplicationService.UpdateUserData(user);
                //Logout to update the claims 
                await _securityApplicationService.Logout();
                return RedirectToAction("AccountSettings",settingsViewModel);
            }
            return View("AccountSettings", settingsViewModel);

        }
        #endregion

    }
}
