namespace Crm.Services
{
    public static class SqlServices
    {
        private static string SqlPC;
        private static string SqlDB;
        private static string SqlUser;
        private static string SqlPassword;

        public static string SqlConnectionString { get; private set; }

        public static async Task InitializeAsync()
        {
            SqlPC = await SecureStorage.GetAsync("SqlPc");
            SqlDB = await SecureStorage.GetAsync("SqlDB");
            SqlUser = await SecureStorage.GetAsync("SqlUser");
            SqlPassword = await SecureStorage.GetAsync("SqlPassword");

            // SqlConnectionString = $"Data Source={SqlPC};Initial Catalog={SqlDB};User ID={SqlUser};Password={SqlPassword};Trust Server Certificate=True";
            SqlConnectionString = $"Data Source=.;Initial Catalog=Crm;User ID=sa;Password=123456a.A;Trust Server Certificate=True";
        } 
    }
}
