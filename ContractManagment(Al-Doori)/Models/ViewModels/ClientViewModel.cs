using ContractManagment_Al_Doori_.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ContractManagment_Al_Doori_.Models.ViewModels
{
    public class ClientViewModel
    {
        public IList<Client> _clients { get; set; }
        [Required]
        [Display(Name ="Client Name")]
        public string ClientSearchName { get; set; }

        [Required]
        public string filterByChoice { get; set; }
    }
}
