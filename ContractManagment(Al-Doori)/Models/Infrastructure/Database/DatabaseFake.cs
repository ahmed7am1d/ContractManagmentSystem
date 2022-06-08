using ContractManagment_Al_Doori_.Models.Entities;
using ContractManagment_Al_Doori_.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace ContractManagment_Al_Doori_.Models.Infrastructure.Database
{
    //Seeding the Database
    public static class DatabaseFake
    {

        #region Method to Return List of Client 
        public static List<Client> GenerateClients()
        {
            List<Client> clients = new List<Client>()
            {
                new Client(1,"John","John1E","john@fe.cz","004206665442","234412FWETRD",22),
                new Client(2,"Alias","Alias3d","alias@fe.cz","004206665432","234412HVSSAD",26),
                new Client(3,"Mack","Mack2d","mack@fe.cz","004206665412","234412FADHER",27),
                new Client(4,"Nelson","NelsonE2","nelson@fe.cz","004206665492","234412FAEWRSAD",21),
                new Client(5,"Malik","Malik2R","malik@fe.cz","004206665422","234412FADSRAD",19),

            };

            return clients;
        }
        #endregion

        #region Methods to Return List of Advisors
        public static List<Advisor> GenerateAdvisors()
        {
            List<Advisor> advisors = new List<Advisor>()
            {
                new Advisor(1,"Jeffrey","Jeffrey1E","Jeffrey@fe.cz","004206663442","234Q12FWETRD",30),
                new Advisor(2,"Fateh","Fateh3d","Fateh@fe.cz","004206265432","234W12HVSSAD",31),
                new Advisor(3,"John","John2d","John@fe.cz","004206655412","2344E2FADHER",29),
                new Advisor(4,"Oisin","OisinE2","Oisin@fe.cz","004207665492","234H12FAEWRSAD",43),
                new Advisor(5,"Carys","Carys2R","Carys@fe.cz","004206675422","23G412FADSRAD",48),

            };

            return advisors;
        }
        #endregion

        #region Methods to Return A Contract
        public static List<Contract> GenerateContracts()
        {
            List<Contract> contracts = new List<Contract>()
            {
                new Contract(1,234,"AEGON",1,"2021/03/04","2023/03/04","2022/03/04"),
                new Contract(2,425,"CBOS",2,"2021/03/06","2023/03/06","2022/03/06"),
                new Contract(3,321,"BLogic",4,"2021/04/06","2023/04/07","2022/04/06"),
                new Contract(4,120,"AL-KAMALI",3,"2021/05/06","2023/05/07","2022/04/05"),
            
            };
            return contracts;
        }

        #endregion

        #region Method to Return List of AdvisorContract 
        public static List<AdvisorContract> GenerateAdvisorContracts()
        {
            return new List<AdvisorContract>()
            {
                    new AdvisorContract(){Id = 1,AdvisorID=1,ContractID=1,isAdminstrator= 1},
                    new AdvisorContract(){Id = 2,AdvisorID=2,ContractID=1, isAdminstrator = 0},
                    new AdvisorContract(){Id = 3,AdvisorID=4,ContractID=2, isAdminstrator = 1}, 
                    new AdvisorContract(){Id = 4,AdvisorID = 3,ContractID = 3 , isAdminstrator=0 },
                    new AdvisorContract(){Id = 5, AdvisorID = 1, ContractID = 4, isAdminstrator = 1},
                    new AdvisorContract(){Id = 6, AdvisorID = 5, ContractID = 4, isAdminstrator = 1},
                   
            };
        }
        #endregion

        #region Method to return list of Role to seed the database
        public static List<Role> GenerateRoles()
        {
            Role admin = new Role(Roles.Admin.ToString(), 1);

            return new List<Role>()
            {
                admin
            };
        }
        #endregion

        #region Method to Insert Admin to database when first the app is built 
        public static async Task EnsureAdminCreated(UserManager<User> userManager)
        {
            //[1] Creating of the user 
            User user = new User
            {
                UserName = "admin",
                Email = "admin@admin.cz",
                EmailConfirmed = true,
                FirstName = "ahmed",
                LastName = "Taha"
            };
            //[2] Password:
            string Password = "admin";

            //[3] Check Admin if already exists in the database 
            User AdminInDb = await userManager.FindByNameAsync(user.UserName);

            if(AdminInDb == null)
            {
                //[4] Create Admin as User if not exist in database 
                IdentityResult result = await userManager.CreateAsync(user,Password);
                if (result == IdentityResult.Success)
                {
                    //[5] give admin user all roles 
                    string[] roles = Enum.GetNames(typeof(Roles));
                    foreach (var role in roles)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
                else if (result != null && result.Errors != null && result.Errors.Count() > 0)
                {
                    foreach (var error in result.Errors)
                    {
                        Debug.WriteLine($"Error during Role creation for Admin: {error.Code}, {error.Description}");
                    }
                }
            }

        }


        #endregion
    }
}
