using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ContractManagment_Al_Doori_.Models.ApplicationServices.Abstraction;
using ContractManagment_Al_Doori_.Models.Entities.Identity;
using ContractManagment_Al_Doori_.Models.ViewModel;
using ContractManagment_Al_Doori_.Models.ViewModels;

namespace UTBEShop.Models.ApplicationServices.Implementation
{
    public class SecurityIdentityApplicationService : ISecurityApplicationService
    {
        UserManager<User> userManager;
        SignInManager<User> sigInManager;

        public SecurityIdentityApplicationService(UserManager<User> userManager, SignInManager<User> sigInManager)
        {
            this.userManager = userManager;
            this.sigInManager = sigInManager;
        }

        #region Method to Find user by Email
        public Task<User> FindUserByEmail(string email)
        {
            return userManager.FindByEmailAsync(email);
        }
        #endregion


        #region Method to Find User by UserName
        public Task<User> FindUserByUsername(string username)
        {
            return userManager.FindByNameAsync(username);
        }
        #endregion


        #region Method to get Current user
        public Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return userManager.GetUserAsync(principal);
        }
        #endregion


        #region Method to Get the roles for a user 
        public Task<IList<string>> GetUserRoles(User user)
        {
            return userManager.GetRolesAsync(user);
        }
        #endregion


        #region Method to Login
        public async Task<bool> Login(LoginViewModel vm)
        {
            var result = await sigInManager.PasswordSignInAsync(vm.Username, vm.Password, true, true);
            return result.Succeeded;
            
        }
        #endregion


        #region Method to logout From Session
        public Task Logout()
        {
            return sigInManager.SignOutAsync();
        }
        #endregion


        #region Method to register User to database 
        public async Task<string[]> Register(RegisterViewModel vm, Roles role)
        {
            //note that we can't add password directly to the user object because the password should be hashed 
            User user = new User()
            {
                UserName = vm.Username,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                PhoneNumber = vm.Phone
            };

            string[] errors = null;
            var result = await userManager.CreateAsync(user, vm.Password);
            if (result.Succeeded)
            {
                var resultRole = await userManager.AddToRoleAsync(user, role.ToString());

                if (resultRole.Succeeded == false)
                {
                    for (int i = 0; i < result.Errors.Count(); ++i)
                        result.Errors.Append(result.Errors.ElementAt(i));
                }
            }

            if (result.Errors != null && result.Errors.Count() > 0)
            {
                errors = new string[result.Errors.Count()];
                for (int i = 0; i < result.Errors.Count(); ++i)
                {
                    errors[i] = result.Errors.ElementAt(i).Description;
                }
            }

            return errors;

            //return null;
        }
        #endregion


        #region Method to Update Personal Data
        public async Task<bool> UpdateUserData(User user)
        {
            var updateResult = await userManager.UpdateAsync(user);
            
            if (updateResult.Succeeded)
            {
                return true;
            }
            //else
            return false;
        }
        #endregion

    }
}
