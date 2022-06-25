using ContractManagment_Al_Doori_.Models.Entities;
namespace ContractManagment_Al_Doori_.Areas.Admin.Models.ViewModels
{
    public class ContractViewModelEdit
    {
        
        public Contract contract { get; set; }

        public IList<Client> clients { get; set; }

        public IList<Advisor> advisors { get; set; }

        public AddAdvisorViewModel AddAdvisorViewModel { get; set; }

    }
}
