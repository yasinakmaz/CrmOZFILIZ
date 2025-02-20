namespace Crm.Model
{
    public record TblPerson
    {
        [Key]
        public Guid IND { get; set; }
        public byte[]? UserImage { get; set; }
        public ImageSource UserImageSource
        {
            get
            {
                if (UserImage != null && UserImage.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(UserImage));
                }
                return null;
            }
        }
        public string UserName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Durum { get; set; } = string.Empty;
        public DateTime UserCreateDate { get; set; }
        public DateTime? UserUpdateDate { get; set; }
        public DateTime? UserDeleteDate { get; set; }
        public DateTime? UserLastLoginDate { get; set; }
    }
}
