namespace Nowcfo.Domain.Models
{
    public class DynamicFilterField
    {
        public int Id { get; set; }
        public string Component { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public int DisplayOrder { get; set; }
    }
}
