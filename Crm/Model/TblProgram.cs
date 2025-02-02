namespace Crm.Model
{
    public class TblProgram
    {
        [Key]
        public Guid IND { get; set; }
        public string ProgramName { get; set; }
        public string ProgramCategory { get; set; }
        public byte[] ProgramImage { get; set; }

        public ImageSource ProgramImageSource
        {
            get
            {
                if (ProgramImage != null && ProgramImage.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(ProgramImage));
                }
                return null;
            }
        }
    }
}
