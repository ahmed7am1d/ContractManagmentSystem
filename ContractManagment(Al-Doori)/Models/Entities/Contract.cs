using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContractManagment_Al_Doori_.Models.Entities
{
    [Table(nameof(Contract))]
    public class Contract
    {

        #region Member Varaibles/Proprties
        [Required]
        [Key]
        public int Id { get; set; }
        public int RegisterationNumber { get; set; }
        public string? Institution { get; set; }



        [ForeignKey(nameof(Client))]
        public int ClientID { get; set; }
        public Client? Client { get; set; }



        public string? ConclusionDate { get; set; }

        public string? ValidityDate { get; set; }

        public string? TerminationDate { get; set; }

        public IList<AdvisorContract> ContractAdvisors { get; set; }




        #endregion

        #region Constructors 
        public Contract(int id, int registerationNumber, string? institution, int clientID, string? conclusionDate, string? validityDate, string? terminationDate)
        {
            Id = id;
            RegisterationNumber = registerationNumber;
            Institution = institution;
            ClientID = clientID;
            ConclusionDate = conclusionDate;
            ValidityDate = validityDate;
            TerminationDate = terminationDate;
        }
        public Contract()
        {

        }

        #endregion

    }
}
