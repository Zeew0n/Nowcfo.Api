namespace Nowcfo.Application.Helper
{
    public  class CrudPermission
    {
        //User
        public const string ViewUser = "user.read";
        public const string AddUser = "user.create";
        public const string UpdateUser = "user.update";
        public const string DeleteUser = "user.delete";

        //Role
        public const string ViewRole = "role.read";
        public const string AddRole = "role.create";
        public const string UpdateRole = "role.update";
        public const string DeleteRole = "role.delete";

        //Organization 
        public const string ViewOrganization = "org.read";
        public const string AddOrganization = "org.create";
        public const string UpdateOrganization = "org.update";
        public const string DeleteOrganization = "org.delete";

        //Employee
        public const string ViewEmployee = "emp.read";
        public const string AddEmployee = "emp.create";
        public const string UpdateEmployee = "emp.update";
        public const string DeleteEmployee = "emp.delete";

        //Designation
        public const string ViewDesignation = "deg.read";
        public const string AddDesignation = "deg.create";
        public const string UpdateDesignation = "deg.update";
        public const string DeleteDesignation = "deg.delete";


    }
}