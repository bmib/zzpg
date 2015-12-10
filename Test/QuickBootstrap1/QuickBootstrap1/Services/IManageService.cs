namespace QuickBootstrap1.Services
{
    public interface IManageService
    {
        bool Login(string username, string password);

        void Logout(string username);
    }
}