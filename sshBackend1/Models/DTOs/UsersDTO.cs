using System.ComponentModel.DataAnnotations;

namespace sshBackend1.Models.DTOs
{
    public class UsersDTO
    {

        [Key]
        public string Id { get; set; }


        public string password { get; set; }


        public string UserName { get; set; }


        public string Email { get; set; }
        public int role { get; set; }



    }
}
