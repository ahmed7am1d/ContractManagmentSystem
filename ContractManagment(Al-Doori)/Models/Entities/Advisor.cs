using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ContractManagment_Al_Doori_.Models.Entities
{
    [Table(nameof(Advisor))]
    public class Advisor
    {
        #region Member Varaibles/Proprties

        [Key]
        [Required]
        public int AdvisorId { get; set; }

        public string Name { get; set; }
        public string SurName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string NationalIdentificationNumber { get; set; }

        public int Age { get; set; }


        public IList<AdvisorContract> ContractAdvisors { get; set; }
        #endregion

        #region Constructors
        public Advisor(int advisorId, string name, string surName, string email, string phoneNumber, string nationalIdentificationNumber, int age)
        {
            AdvisorId = advisorId;
            Name = name;
            SurName = surName;
            Email = email;
            PhoneNumber = phoneNumber;
            NationalIdentificationNumber = nationalIdentificationNumber;
            Age = age;
        }
        public Advisor()
        {

        }
        #endregion

    }
}
