using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApplication1.Controllers.Securities
{
    public class SecurityModel
    {
        public int HId { get; set; }
        public Guid HGuid { get; set; }
        public string FullName { get; set; }
        //Dátummal is megy:
        //public DateTime Datum { get; set; }

        private static int tid;
        public static SecurityModel CreteNewModel()
        {
            return new SecurityModel()
                {
                    HId = ++tid,
                    HGuid = Guid.NewGuid(),
                    FullName = "Paprikás Mókus " + tid,
                    //Datum = DateTime.Now.AddYears(-10)
                };
        }

        public override string ToString()
        {
            return string.Format("Hid: {0} Guid:{1} Név: {2}", HId, HGuid, FullName);
        }
    }

    public class SecurityStorageModel
    {
        public SecurityModel SecurityModelInternal { get; set; }
        public int Sid { get; set; }
    }
}