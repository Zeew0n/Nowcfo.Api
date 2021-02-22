using Microsoft.AspNetCore.Identity;
using System;

namespace Nowcfo.Domain.Models.Base
{
    public class User:IdentityUser,IEntity
    {
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }
    }
}
