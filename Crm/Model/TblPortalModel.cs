using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crm.Model
{
    public class TblPortalModel
    {
        public int ID { get; set; }
        public string Secim { get; set; }
        public string AdSoyad { get; set; }
        public string Firma { get; set; }
        public string TelNo { get; set; }
        public string Sorun { get; set; }
        public byte[]? Resim { get; set; }
    }
}
