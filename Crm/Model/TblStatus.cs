namespace Crm.Model
{
    public record TblStatus
    {
        [Key]
        public Guid IND { get; set; }
        public string? TitleText { get; set; }
        public byte[]? StatusImage { get; set; }
        public ImageSource? StatusImageSource
        {
            get
            {
                if(StatusImage != null && StatusImage.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(StatusImage));
                }
                return null;
            }
        }
        public int? Order { get; set; }
        public string? StatusName { get; set; }
        public string? ColorCode { get; set; }
    }
}
