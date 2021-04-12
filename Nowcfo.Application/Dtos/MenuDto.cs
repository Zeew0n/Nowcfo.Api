using System;

namespace Nowcfo.Application.Dtos
{
    public class MenuDto
    {
        public Guid Id { get; set; }

        public string MenuName { get; set; }

        public string NavigateUrl { get; set; }

        public Guid? UnderMenuId { get; set; }

        public int? MenuLevel { get; set; }

        public string Icon { get; set; }

        public int? DisplayOrder { get; set; }

    }
}