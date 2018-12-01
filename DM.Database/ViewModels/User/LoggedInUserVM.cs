namespace DM.Models.ViewModels
{
    public class LoggedInUserVM: UserVM
    {
        public LoggedInUserVM() {}

        public LoggedInUserVM(UserVM user, AuthToken token)
        {
            Id = user.Id;
            ImageId = user.ImageId;
            Name = user.Name;
            Surname = user.Surname;
            City = user.City;
            IsFriend = user.IsFriend;
            Token = token;
        }

        public AuthToken Token { get; set; }
    }
}
