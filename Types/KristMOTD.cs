using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Crazed.KristSharp
{
    public class KristMOTD
    {
        public string MOTD;
        public string PSA;
        public string ShrodingersCat;
        public DateTime Set;

        internal KristMOTD(KMOTDResult motd)
        {
            MOTD = motd.motd;
            PSA = motd.psa;
            ShrodingersCat = motd.shrodingers_cat;
            Set = motd.set;
        }
    }
}
