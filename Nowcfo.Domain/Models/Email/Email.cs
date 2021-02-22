using System;

namespace Nowcfo.Domain.Models.Email
{
    public class Email : BaseEntity, ISoftDeletableEntity
    {
        public Guid CompanyId { get; set; }

        public string SendGridTemplateID { get; set; }

        public string SubjectLine { get; set; }
    }
}