using ContractManagment_Al_Doori_.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace ContractManagment_Al_Doori_.Models.ViewModels
{
    public class ContractViewModel
    {
        #region Member Varaiables/Proprties

        public IList<Contract> contracts { get; set; }
        [Required]
        public string InstitueNameSearch { get; set; }
        #endregion

    }
}
