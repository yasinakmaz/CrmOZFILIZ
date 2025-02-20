namespace Crm.Services
{
    public static class PublicServices
    {
        public static event Action? LoginChanged;
        public static string UserFirstName = string.Empty;
        public static string UserLastName = string.Empty;
        public static string UserFullName = string.Empty;
        public static ImageSource? profileImage;
        public static void PushLoginChanged(bool changed)
        {
            LoginChanged?.Invoke();
        }
    }
}
