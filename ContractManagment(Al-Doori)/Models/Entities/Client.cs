using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractManagment_Al_Doori_.Models.Entities
{
    [Table(nameof(Client))]

    public class Client
    {

        #region Member Varaibles/Proprties
        [Key]
        [Required]
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public int Age { get; set; }




        #endregion

        #region Constructors

        public Client(int clientId, string name, string surName, string email, string phoneNumber, string nationalIdentificationNumber, int age)
        {
            ClientId = clientId;
            Name = name;
            SurName = surName;
            Email = email;
            PhoneNumber = phoneNumber;
            NationalIdentificationNumber = nationalIdentificationNumber;
            Age = age;
        }

        public Client(string name, string surName, string email, string phoneNumber, string nationalIdentificationNumber, int age)
        {
            Name = name;
            SurName = surName;
            Email = email;
            PhoneNumber = phoneNumber;
            NationalIdentificationNumber = nationalIdentificationNumber;
            Age = age;
        }

        public Client()
        {

        }
        #endregion

    }
}
