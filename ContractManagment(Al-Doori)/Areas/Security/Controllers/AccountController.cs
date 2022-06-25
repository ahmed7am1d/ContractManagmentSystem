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

        /// <summary>
        /// Login Action Method that allows the user to login to website using loginViewModel
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
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
                //else [LoginFailed]
                loginViewModel.LoginFailed = true;
            }
            //[else wrong Inputted Data] => server side validation :)  
            return View(loginViewModel);
        }


        /// <summary>
        /// Return Normal View On Navigation When Calling this method only if user not authenticated
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// ActionResult Method to Logout from the current Session
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await _securityApplicationService.Logout();
            return RedirectToAction(nameof(Login));
        }
        #endregion

        #region AcountSettings Action Methods
        /// <summary>
        /// Method Returns View to set account settings only authorized people can access this view 
        /// </summary>
        /// <returns>View with the filled user data</returns>
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> AccountSettings()
        {
            User currentUser = await _securityApplicationService.GetCurrentUser(User);
            SettingsViewModel settings = new SettingsViewModel(currentUser.FirstName,currentUser.LastName,currentUser.PhoneNumber,currentUser.Email,currentUser.UserName,currentUser.Photo);
            return View(settings);
        }
        #endregion

        #region Action Method to SaveNewSettings
        [Authorize(Roles = nameof(Roles.Admin))]
        public async Task<IActionResult> SaveNewSettings(SettingsViewModel settingsViewModel, IFormFile img)
        {
            //fake validation
            //Problems Here        
            
            ModelState["PhoneNumber"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            ModelState["Photo"].ValidationState = Microsoft.AspNetCore.Mvc.ModelBinding.ModelValidationState.Valid;
            
            if (ModelState.IsValid)
            {
                var user = await _securityApplicationService.GetCurrentUser(User);
                user.FirstName = settingsViewModel.FirstName;
                user.LastName = settingsViewModel.LastName;
                user.PhoneNumber = settingsViewModel.PhoneNumber;
                user.Email = settingsViewModel.Email;
                user.UserName = settingsViewModel.UserName;
                
                //For Photo 
                user.Photo = GetByteArrayFromImage(img);
                settingsViewModel.Photo = user.Photo;
                
    
                await _securityApplicationService.UpdateUserData(user);
                ModelState.Clear();
                //Logout to update the claims 
                await _securityApplicationService.Logout();
            }
            ModelState.Clear();
            return View("AccountSettings", settingsViewModel);

        }
        
        private byte[] GetByteArrayFromImage(IFormFile file)
        {
            using (var target = new MemoryStream())
            {
                file.CopyTo(target);
                return target.ToArray();
            }
        }
        #endregion

    }
}
