using System;
using System.ComponentModel.DataAnnotations;

namespace Crm.Model
{
    public class TblProgram
    {
        [Key]
        public Guid ID { get; set; }

        public string ProgramAdi { get; set; }

        public byte[] ProgramResim { get; set; }

        public ImageSource ProgramResimSource
        {
            get
            {
                if (ProgramResim != null && ProgramResim.Length > 0)
                {
                    return ImageSource.FromStream(() => new MemoryStream(ProgramResim));
                }
                return null;
            }
        }
    }
}
