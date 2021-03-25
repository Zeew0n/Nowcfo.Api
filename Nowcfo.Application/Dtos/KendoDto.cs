using System.Collections.Generic;

namespace Nowcfo.Application.Dtos
{
    public class KendoDto
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public int? ParentOrganizationId { get; set; }
        public List<KendoDto> items { get; set; }
        public int ChildrenCount { get; set; }
        public int CheckType { get; set; }
       

    }


}
