using Microsoft.EntityFrameworkCore;
using Nowcfo.Domain.Models.AppUserModels;
using System;

namespace Nowcfo.Infrastructure.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<AppRole>().HasData(
                new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = "SuperAdmin"
                    
                },
                new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin"
                },
                new AppRole
                {
                    Id = Guid.NewGuid(),
                    Name = "User"
                }
            );
            
        }
    }
}
