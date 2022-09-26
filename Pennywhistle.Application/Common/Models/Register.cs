namespace Pennywhistle.Application.Common.Models
{
    /// <summary>
    /// Staff user register model
    /// </summary>
    public class Register
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
