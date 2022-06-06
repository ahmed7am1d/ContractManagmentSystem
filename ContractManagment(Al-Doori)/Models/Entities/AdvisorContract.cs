using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ContractManagment_Al_Doori_.Models.Entities
{
    [Table(nameof(AdvisorContract))]
    public class AdvisorContract
    {
        #region Member Varaibles/Proprties

        [Required]
        [Key]
        public int Id { get; set; }
 
        [ForeignKey(nameof(Contract))]
        public int ContractID { get; set; }
        public Contract Contract { get; set; }

        [ForeignKey(nameof(Advisor))]
        public int AdvisorID { get; set; }
        public Advisor Advisor { get; set; }
        public int isAdminstrator { get; set; }

        #endregion
    }
}
