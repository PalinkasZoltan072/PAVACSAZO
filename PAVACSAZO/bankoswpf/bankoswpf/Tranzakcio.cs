using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAlkalmazas
{
    public class Tranzakcio
    {
        public int TranzakcioId { get; set; }
        public string Szamlaszam { get; set; }           // érintett számla
        public string Tipus { get; set; }
        public double Osszeg { get; set; }
        public string Datum { get; set; }
        public string PartnerSzamlaszam { get; set; }    // átutalásnál a cél számla, egyébként üres

        public Tranzakcio(int id, string szamla, string tipus, double osszeg, string datum, string partner)
        {
            // csak a három megengedett tranzakció típus engedélyezett
            if (tipus != "Befizetés" && tipus != "Készpénzfelvétel" && tipus != "Átutalás")
            {
                throw new Exception("Érvénytelen tranzakció típus! (Befizetés / Készpénzfelvétel / Átutalás megengedett)");
            }

            TranzakcioId = id;
            Szamlaszam = szamla;
            Tipus = tipus;
            Osszeg = osszeg;
            Datum = datum;
            PartnerSzamlaszam = partner;
        }

        public override string ToString()
        {
            // ha van partner számla, azt is kiírjuk
            if (!string.IsNullOrWhiteSpace(PartnerSzamlaszam))
            {
                return $"{TranzakcioId} | {Tipus} | {Osszeg} Ft | Számla: {Szamlaszam} -> {PartnerSzamlaszam} | Dátum: {Datum}";
            }
            else
            {
                return $"{TranzakcioId} | {Tipus} | {Osszeg} Ft | Számla: {Szamlaszam} | Dátum: {Datum}";
            }
        }
    }
}
