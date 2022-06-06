using Microsoft.AspNetCore.Identity;

namespace ContractManagment_Al_Doori_.Models.Entities.Identity
{
    public class Role : IdentityRole<int>
    {
        #region Constructors
        public Role()
        {

        }

        public Role(string roleName, int id) : base(roleName)
        {
            Id = id;
            NormalizedName = roleName.ToUpper();
        }


        #endregion

    }
}
