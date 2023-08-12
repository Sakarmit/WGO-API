namespace WGO_API.Models.UserModel
{
    public class User
    {
        public int Id { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public Boolean EmailConfirmed { get; set; }
        public Boolean UserBanned { get; set; }
    }
}
