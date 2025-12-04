using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAlkalmazas
{
    public class Szamla
    {
        public string Szamlaszam { get; set; }
        public int UgyfelId { get; set; }
        public string SzamlaTipus { get; set; }          // pl. Folyószámla, Megtakarítási számla
        public double Egyenleg { get; set; }
        public string SzamlanyitasDatuma { get; set; }   // egyszerű szövegként tároljuk

        public Szamla(string szamlaszam, int ugyfelId, string tipus, double egyenleg, string datum)
        {
            // csak a két megengedett számlatípus engedélyezett
            if (tipus != "Folyószámla" && tipus != "Megtakarítási számla")
            {
                throw new Exception("Érvénytelen számlatípus! (Folyószámla / Megtakarítási számla megengedett)");
            }

            Szamlaszam = szamlaszam;
            UgyfelId = ugyfelId;
            SzamlaTipus = tipus;
            Egyenleg = egyenleg;
            SzamlanyitasDatuma = datum;
        }

        public override string ToString()
        {
            // így fog megjelenni listában / konzolon
            return $"{Szamlaszam} | ÜgyfélID: {UgyfelId} | {SzamlaTipus} | Egyenleg: {Egyenleg} Ft | Nyitás: {SzamlanyitasDatuma}";
        }
    }
}
