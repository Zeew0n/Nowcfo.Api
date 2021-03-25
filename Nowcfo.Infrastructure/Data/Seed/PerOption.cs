using System.Linq;

namespace Nowcfo.Infrastructure.Data.Seed
{
    public static class PerOption
    {
        #region role
        public static  string ViewRole(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "role.read").Select(c => c.Slug).First();
        public static  string AddRole(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "role.create").Select(c => c.Slug).First();
        public static  string UpdateRole(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "role.update").Select(c => c.Slug).First();
        public static string DeleteRole(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "role.delete").Select(c => c.Slug).First();
        #endregion

        #region user
        public static string ViewUser(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "user.read").Select(c => c.Slug).First();
        public static string AddUser(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "user.create").Select(c => c.Slug).First();
        public static string UpdateUser(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "user.update").Select(c => c.Slug).First();
        public static string DeleteUser(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "user.delete").Select(c => c.Slug).First();
        #endregion

        #region employee
        public static string ViewEmployee(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.read").Select(c => c.Slug).First();
        public static string AddEmployee(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.create").Select(c => c.Slug).First();
        public static string UpdateEmployee(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.update").Select(c => c.Slug).First();
        public static string DeleteEmployee(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.delete").Select(c => c.Slug).First();
        #endregion

        #region organization
        public static string ViewOrganization(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.read").Select(c => c.Slug).First();
        public static string AddOrganization(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.create").Select(c => c.Slug).First();
        public static string UpdateOrganization(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.update").Select(c => c.Slug).First();
        public static string DeleteOrganization(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.delete").Select(c => c.Slug).First();
        #endregion

        #region designation
        public static string ViewDesignation(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.read").Select(c => c.Slug).First();
        public static string AddDesignation(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.create").Select(c => c.Slug).First();
        public static string UpdateDesignation(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.update").Select(c => c.Slug).First();
        public static string DeleteDesignation(ApplicationDbContext context) => context.Permissions.Where(c => c.Slug == "employee.delete").Select(c => c.Slug).First();
        #endregion
    }
}
