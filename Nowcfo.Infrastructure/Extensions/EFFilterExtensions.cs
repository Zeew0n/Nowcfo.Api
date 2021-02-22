using System;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Nowcfo.Infrastructure.Extensions
{
    public static class EfFilterExtensions
    {
        public static void SetSoftDeleteFilter(this ModelBuilder modelBuilder, Type entityType, string softDeleteColumnName)
        {
            SetSoftDeleteFilterMethod.MakeGenericMethod(entityType)
                .Invoke(null, new object[] { modelBuilder, softDeleteColumnName });
        }

        private static readonly MethodInfo SetSoftDeleteFilterMethod = typeof(EfFilterExtensions)
                   .GetMethods(BindingFlags.Public | BindingFlags.Static)
                   .Single(t => t.IsGenericMethod && t.Name == "SetSoftDeleteFilter");

        public static void SetSoftDeleteFilter<TEntity>(this ModelBuilder modelBuilder, string softDeleteColumnName)
            where TEntity : class
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(m => EF.Property<bool>(m, softDeleteColumnName) == false);
        }
    }
}