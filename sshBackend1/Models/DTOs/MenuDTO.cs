using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace sshBackend1.Models.DTOs
{
    public class MenuDTO
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MenuId { get; set; }

        public string Chef_Name { get; set; }

        public decimal Price { get; set; }

        public int? CateringId { get; set; }

        public int? MenuTypeId { get; set; }
        
    }
}
