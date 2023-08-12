namespace WGO_API.Models.UserModel
{
    public class UserDTO
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
