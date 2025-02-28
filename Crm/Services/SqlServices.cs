namespace Crm.Services
{
    public static class SqlServices
    {
        private static string SqlPC;
        private static string SqlDB;
        private static string SqlUser;
        private static string SqlPassword;

        public static string SqlConnectionString { get; private set; }
        public static Guid CreatUserGuid { get; set; }
        public static Guid LoginUserGuid { get; set; }
        public static Guid ProgramUpdateSelectedItem { get; set; }

        public static event Action ProgramSelectedItemChanged;

        public static void ChangeProgramSelectedItemChanged()
        {
            ProgramSelectedItemChanged?.Invoke();
        }

        public static async Task InitializeAsync()
        {
            SqlPC = await SecureStorage.GetAsync("SqlPc") ?? "192.168.1.1";
            SqlDB = await SecureStorage.GetAsync("SqlDB") ?? "CRMDB";
            SqlUser = await SecureStorage.GetAsync("SqlUser") ?? "sa";
            SqlPassword = await SecureStorage.GetAsync("SqlPassword") ?? "123456a.A";

            // SqlConnectionString = $"Data Source={SqlPC};Initial Catalog={SqlDB};User ID={SqlUser};Password={SqlPassword};Trust Server Certificate=True";
            SqlConnectionString = $"Data Source=192.168.1.5;Initial Catalog=CRMDB;User ID=sa;Password=123456a.A;Trust Server Certificate=True";
        } 
    }
}
