namespace sshBackend1.Models.DTOs
{
    public class LoginResponseDTO
    {
        public ApplicationUserDTO User { get; set; }
        public string Token { get; set; }
    }
}

