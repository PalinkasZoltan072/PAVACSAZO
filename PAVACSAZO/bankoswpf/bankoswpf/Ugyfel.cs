using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAlkalmazas
{
    public class Ugyfel
    {
        public int UgyfelId { get; set; }
        public string Nev { get; set; }
        public string Lakcim { get; set; }
        public string SzuletesiDatum { get; set; }
        public string Telefonszam { get; set; }

        public Ugyfel(int id, string nev, string lakcim, string szul, string tel)
        {
            UgyfelId = id;
            Nev = nev;
            Lakcim = lakcim;
            SzuletesiDatum = szul;
            Telefonszam = tel;
        }

        public override string ToString()
        {
            return $"{UgyfelId}; {Nev}; {Lakcim}; {SzuletesiDatum}; {Telefonszam}";
        }
    }
}
