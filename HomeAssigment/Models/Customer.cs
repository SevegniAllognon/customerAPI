using System.ComponentModel.DataAnnotations.Schema;

namespace HomeAssigment.Models
{
    [Table("customers")]
    public class Customer
    {
        public Guid id { get; set; }
        public string firstname { get; set; } = string.Empty;
        public string? middlename { get; set; }
        public string lastname { get; set; }   = string.Empty ;
        public string phonenumber { get; set; }=string.Empty;
        public string emailaddress { get; set; } = string.Empty;
        public DateTime creationdate { get; set; } = DateTime.Now;
        public DateTime? updateddate { get; set; }
    }
}
