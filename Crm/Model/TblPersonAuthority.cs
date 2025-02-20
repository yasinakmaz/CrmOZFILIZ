namespace Crm.Model
{
    public record TblPersonAuthority
    {
        [Key]
        public Guid IND { get; set; }
        public string AuthorityName { get; set; }
        public Guid PersonIND { get; set; }
        public int PersonAuthorityID { get; set; }
    }
}
