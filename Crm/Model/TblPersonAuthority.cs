namespace Crm.Model
{
    public class TblPersonAuthority
    {
        public Guid IND { get; set; }
        public string AuhtorityName { get; set; }
        public Guid PersonIND { get; set; }
        public int PersonAuthorityID { get; set; }
    }
}
