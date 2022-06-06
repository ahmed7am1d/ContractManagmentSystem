using System.ComponentModel.DataAnnotations;

namespace ContractManagment_Al_Doori_.Models.ViewModels
{
    public class SettingsViewModel
    {
        #region Memebers Variables/Proprties
        [Required]
      
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string PhoneNumber { get;set; }
      
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }


        #endregion

        #region Constructor 
        public SettingsViewModel(string firstName, string lastName, string phoneNumber, string email, string userName)
        {
            FirstName = firstName;
            LastName = lastName;
            PhoneNumber = phoneNumber;
            Email = email;
            UserName = userName;
        }

        public SettingsViewModel()
        {

        }
        #endregion

    }
}
