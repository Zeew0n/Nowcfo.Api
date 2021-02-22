using System;
using System.ComponentModel.DataAnnotations.Schema;
using Nowcfo.Domain.Models.AppUserModels;

namespace Nowcfo.Domain.Models
{
    [Table("UserLogin")]
    public class AppUserLogin : BaseEntity
    {
        public Guid UserId { get; set; }

        [ForeignKey("UserId")]
        public AppUser User { get; set; }

        public bool IsLogin { get; set; }

        public DateTime LoginDate { get; set; }

        public DateTime LogOutDate { get; set; }

        public string IpAddress { get; set; }
    }
}