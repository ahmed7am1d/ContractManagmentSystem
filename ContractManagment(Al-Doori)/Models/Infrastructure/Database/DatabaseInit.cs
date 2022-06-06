using ContractManagment_Al_Doori_.Models.Entities;
using ContractManagment_Al_Doori_.Models.Entities.Identity;

namespace ContractManagment_Al_Doori_.Models.Infrastructure.Database
{
    public class DatabaseInit
    {

        #region Member Varaibles/Proprties
        public static List<Advisor> Advisors { get; set; }

        public static List<Client> Clients { get; set; }

        public static List<Contract> Contract{ get; set; }

        public static List<AdvisorContract> AdvisorContracts{ get; set; }

        public static List<Role> roles { get; set; }
        #endregion


        #region Constructor
        static DatabaseInit()
        {
            Advisors = DatabaseFake.GenerateAdvisors();
            Clients = DatabaseFake.GenerateClients();
            Contract = DatabaseFake.GenerateContracts();
            AdvisorContracts = DatabaseFake.GenerateAdvisorContracts();
            roles = DatabaseFake.GenerateRoles();
        }
        #endregion


    }
}
