namespace Crm.Model
{
    public class TblAgreement
    {
        [Key]
        public Guid IND { get; set; }
        public string SpecialCase { get; set; }
        public string AgreementName { get; set; }
        public byte[] AgreementImage { get; set; }

        public ImageSource AgreementImageSource
        {
            get
            {
                if (AgreementImage != null && AgreementImage.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(AgreementImage));
                }
                return null;
            }
        }
    }
}
