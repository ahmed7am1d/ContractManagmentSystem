using ContractManagment_Al_Doori_.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ContractManagment_Al_Doori_.Models.ViewModels
{
    public class AdvisorViewModel
    {
        public IList<Advisor> advisors { get; set; }
        [Required]
        [Display(Name = "Client Name")]
        public string AdvisorSearchName { get; set; }

        [Required]
        public string filterByChoice { get; set; }
    }
}
