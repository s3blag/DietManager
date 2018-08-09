namespace DM.Logic.Models
{
    public class UserCreation
    {
        public UserCreation(UserCreationVM userCreationVM)
        {
            Email = userCreationVM.Email;
            Password = userCreationVM.Password;
            UserName = userCreationVM.UserName;
        }

        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
    }
}
