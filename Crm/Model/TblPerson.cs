namespace Crm.Model
{
    public class TblPerson
    {
        [Key]
        public Guid IND { get; set; }
        public byte[] UserImage { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Guid UserAuthorityInd { get; set; }
        public string Email {  get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
        public string Durum {  get; set; }
        public DateTime UserCreateDate { get; set; }
        public DateTime UserUpdateDate { get; set; }
        public DateTime UserDeleteDate { get; set; }
        public DateTime UserLastLoginDate { get; set; }
    }
}
