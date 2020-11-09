namespace MyFirstMvcApp.Services
{
    public interface IUserService
    {
        void CreateUser(string username, string email, string password);

        bool IsUserValid(string username, string password);

        bool IsUsernameAvailable(string username);

        bool IsEmailAvailable(string email);
    }
}
