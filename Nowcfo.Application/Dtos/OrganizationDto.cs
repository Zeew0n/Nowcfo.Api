namespace Nowcfo.Application.Dtos
{
    public class OrganizationDto
    {
        public int OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public bool? HasParent { get; set; }
        public int? ParentOrganizationId { get; set; }
        public string ParentOrganization { get; set; }
    }
}