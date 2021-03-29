using System;

namespace Nowcfo.Application.Dtos
{
    public class MenuDto
    {
        public Guid Id { get; set; }

        public string MenuName { get; set; }

        public string MenuUrl { get; set; }

        public string Icon { get; set; }

        public int? DisplayOrder { get; set; }
    }
}