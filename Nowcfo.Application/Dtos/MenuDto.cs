namespace Nowcfo.Application.DTO
{
    public class MenuDto
    {
        public string MenuName { get; set; }

        public int MenuLevel { get; set; }

        public string UnderMenuId { get; set; }

        public string Icon { get; set; }

        public int? DisplayOrder { get; set; }

        public bool? IsActive { get; set; }
    }
}
