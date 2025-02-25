namespace Crm.Model
{
    public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public DbSet<TblProgram> TBLPROGRAM { get; set; }
        public DbSet<TblAgreement> TBLAGREEMENT { get; set; }
        public DbSet<TblPerson> TBLPERSON { get; set; }
        public DbSet<TblPersonAuthority> TBLPERSONAUTHORITY { get; set; }
        public DbSet<TblWaiterList> TBLRECORDLIST { get; set; }

        private readonly string _connectionString;

        public AppDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }
    }
}
