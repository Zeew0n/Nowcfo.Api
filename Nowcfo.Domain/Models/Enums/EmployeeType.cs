using System.ComponentModel;

namespace Nowcfo.Domain.Models.Enums
{

    public enum  EmployeeType
    {
        [Description("Full-Time")]
        FullTime = 1,
        [Description("Part-Time")]

        PartTime,
        Hybrid,
        Vendor
    }
}
