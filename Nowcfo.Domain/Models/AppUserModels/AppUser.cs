﻿using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Nowcfo.Domain.Models.AppUserModels
{
    public class AppUser : IdentityUser<Guid>, ISoftDeletableEntity
    {
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        //[NotMapped]
        public string FullName { get => FirstName + " " + LastName; }

        [NotMapped]
        public string Password { get; set; }

        //Standard Columns
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; } = DateTime.Now;
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public bool IsAdmin { get; set; }
        public bool IsSuperAdmin { get; set; }
    }
}
