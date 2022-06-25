using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace ContractManagment_Al_Doori_.Models.Entities.Identity
{
    public class User : IdentityUser<int>
    {
        #region Proprties 
        public virtual string? FirstName { get; set; }
        public virtual string? LastName { get; set; }
        
        public byte[] Photo { get; set; }
        [NotMapped]
        public IFormFile file { get; set; }
            
        #endregion
    }
}
