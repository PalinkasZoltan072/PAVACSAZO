using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankAlkalmazas
{
    public class BankRendszer
    {
        public List<Ugyfel> Ugyfelek = new List<Ugyfel>();
        public List<Szamla> Szamlak = new List<Szamla>();
        public List<Tranzakcio> Tranzakciok = new List<Tranzakcio>();

        public BankRendszer()
        {
            Betoltes();


        }



        private void Betoltes()
        {
            Ugyfelek.Clear();
            Szamlak.Clear();
            Tranzakciok.Clear();

            FajlBeolvasasa("ugyfelek.txt");
            FajlBeolvasasa("szamlak.txt");
            FajlBeolvasasa("tranzakciok.txt");
        }

        //private void FajlBeolvasasa(string fajlNev)
        //{
        //    if (!File.Exists(fajlNev)) return;

        //    StreamReader f = new StreamReader(fajlNev);

        //    while (!f.EndOfStream)
        //    {
        //        string sor = f.ReadLine();
        //        if (string.IsNullOrWhiteSpace(sor)) continue;

        //        string[] d = sor.Split(';');

        //        try
        //        {
        //            if (fajlNev == "ugyfelek.txt")
        //            {
        //                Ugyfel u = new Ugyfel(
        //                    int.Parse(d[0]), d[1], d[2], d[3], d[4]);
        //                Ugyfelek.Add(u);
        //            }
        //            else if (fajlNev == "szamlak.txt")
        //            {
        //                Szamla s = new Szamla(
        //                    d[0], int.Parse(d[1]), d[2], double.Parse(d[3]), d[4]);
        //                Szamlak.Add(s);
        //            }
        //            else if (fajlNev == "tranzakciok.txt")
        //            {
        //                Tranzakcio t = new Tranzakcio(
        //                    int.Parse(d[0]), d[1], d[2], double.Parse(d[3]), d[4], d[5]);
        //                Tranzakciok.Add(t);
        //            }
        //        }
        //        catch
        //        {
        //            // hibás sor → átugrás
        //        }
        //    }

        //    f.Close();
        //}

        private void FajlBeolvasasa(string fajlNev)
        {
            if (!File.Exists(fajlNev)) return;

            using (StreamReader f = new StreamReader(fajlNev))
            {
                while (!f.EndOfStream)
                {
                    string sor = f.ReadLine();
                    if (string.IsNullOrWhiteSpace(sor)) continue;

                    string[] d = sor.Split(';');

                    try
                    {
                        Feldolgozas(fajlNev, d);
                    }
                    catch
                    {
                        // Hibás sor → átugrás
                    }
                }
            }
        }

        private void Feldolgozas(string fajlNev, string[] d)
        {
            switch (fajlNev)
            {
                case "ugyfelek.txt":
                    BeolvasUgyfel(d);
                    break;

                case "szamlak.txt":
                    BeolvasSzamla(d);
                    break;

                case "tranzakciok.txt":
                    BeolvasTranzakcio(d);
                    break;
            }
        }

        private void BeolvasUgyfel(string[] d)
        {
            Ugyfel u = new Ugyfel(
                int.Parse(d[0]),
                d[1], d[2], d[3], d[4]);

            Ugyfelek.Add(u);
        }

        private void BeolvasSzamla(string[] d)
        {
            Szamla s = new Szamla(
                d[0],
                int.Parse(d[1]),
                d[2],
                double.Parse(d[3]),
                d[4]);

            Szamlak.Add(s);
        }

        private void BeolvasTranzakcio(string[] d)
        {
            Tranzakcio t = new Tranzakcio(
                int.Parse(d[0]),
                d[1],
                d[2],
                double.Parse(d[3]),
                d[4],
                d[5]);

            Tranzakciok.Add(t);
        }



        private void Mentes()
        {
            UgyfelekMentese();
            SzamlakMentese();
            TranzakciokMentese();
        }

        private void UgyfelekMentese()
        {
            StreamWriter f = new StreamWriter("ugyfelek.txt");

            foreach (Ugyfel u in Ugyfelek)
                f.WriteLine($"{u.UgyfelId};{u.Nev};{u.Lakcim};{u.SzuletesiDatum};{u.Telefonszam}");

            f.Close();
        }

        private void SzamlakMentese()
        {
            StreamWriter f = new StreamWriter("szamlak.txt");

            foreach (Szamla s in Szamlak)
                f.WriteLine($"{s.Szamlaszam};{s.UgyfelId};{s.SzamlaTipus};{s.Egyenleg};{s.SzamlanyitasDatuma}");

            f.Close();
        }

        private void TranzakciokMentese()
        {
            StreamWriter f = new StreamWriter("tranzakciok.txt");

            foreach (Tranzakcio t in Tranzakciok)
                f.WriteLine($"{t.TranzakcioId};{t.Szamlaszam};{t.Tipus};{t.Osszeg};{t.Datum};{t.PartnerSzamlaszam}");

            f.Close();
        }


        private int KovUgyfelId()
        {
            int max = 0;
            foreach (var u in Ugyfelek)
                if (u.UgyfelId > max) max = u.UgyfelId;
            return max + 1;
        }

        private int KovTranzId()
        {
            int max = 0;
            foreach (var t in Tranzakciok)
                if (t.TranzakcioId > max) max = t.TranzakcioId;
            return max + 1;
        }



        private void SZAMLA_ELL(string szamla)
        {
            foreach (var s in Szamlak)
                if (s.Szamlaszam == szamla)
                    return;

            throw new Exception("Nincs ilyen számla.");
        }



        public Ugyfel UjUgyfel(string nev, string lakcim, string szul, string tel)
        {
            if (string.IsNullOrWhiteSpace(nev)) throw new Exception("A név kötelező.");
            if (string.IsNullOrWhiteSpace(lakcim)) throw new Exception("A lakcím kötelező.");

            Ugyfel u = new Ugyfel(KovUgyfelId(), nev, lakcim, szul, tel);
            Ugyfelek.Add(u);
            Mentes();
            return u;
        }

        //public Szamla UjSzamla(string szamlaszam, int ugyfelId, string tipus, double osszeg)
        //{
        //    // számlaszám kötelező
        //    if (string.IsNullOrWhiteSpace(szamlaszam))
        //        throw new Exception("A számlaszám kötelező.");

        //    // számlaszám csak számjegyeket tartalmazhat
        //    foreach (char c in szamlaszam)
        //    {
        //        if (c < '0' || c > '9')
        //            throw new Exception("A számlaszám csak számjegyeket tartalmazhat.");
        //    }

        //    // ügyfél létezik-e
        //    bool vanUgyfel = false;
        //    foreach (Ugyfel u in Ugyfelek)
        //    {
        //        if (u.UgyfelId == ugyfelId)                                                                         // javitas
        //        {
        //            vanUgyfel = true;
        //            break;
        //        }
        //    }
        //    if (!vanUgyfel)
        //        throw new Exception("Nincs ilyen ügyfél.");

        //    // egyedi számlaszám
        //    foreach (Szamla s in Szamlak)
        //    {
        //        if (s.Szamlaszam == szamlaszam)
        //            throw new Exception("Már létezik ilyen számlaszám.");
        //    }

        //    // ha minden oké, létrehozzuk
        //    Szamla uj = new Szamla(szamlaszam, ugyfelId, tipus, osszeg,
        //                           DateTime.Now.ToShortDateString());

        //    Szamlak.Add(uj);
        //    Mentes();
        //    return uj;
        //}


        public Szamla UjSzamla(string szamlaszam, int ugyfelId, string tipus, double osszeg)
        {
            EllenorizSzamlaszamFormatum(szamlaszam);
            EllenorizUgyfelLetezik(ugyfelId);
            EllenorizSzamlaszamEgyediseg(szamlaszam);

            var uj = new Szamla(
                szamlaszam,
                ugyfelId,
                tipus,
                osszeg,
                DateTime.Now.ToShortDateString()
            );

            Szamlak.Add(uj);
            Mentes();

            return uj;
        }

        private void EllenorizSzamlaszamFormatum(string szamlaszam)
        {
            if (string.IsNullOrWhiteSpace(szamlaszam))
                throw new Exception("A számlaszám kötelező.");

            if (!szamlaszam.All(char.IsDigit))
                throw new Exception("A számlaszám csak számjegyeket tartalmazhat.");
        }

        private void EllenorizUgyfelLetezik(int ugyfelId)
        {
            if (!Ugyfelek.Any(u => u.UgyfelId == ugyfelId))
                throw new Exception("Nincs ilyen ügyfél.");
        }

        private void EllenorizSzamlaszamEgyediseg(string szamlaszam)
        {
            if (Szamlak.Any(s => s.Szamlaszam == szamlaszam))
                throw new Exception("Már létezik ilyen számlaszám.");
        }

        public Tranzakcio Befizetes(string szamla, double osszeg)
        {
            if (osszeg < 0)
                throw new ArgumentException("A befizetés összege nem lehet negatív!");

            SZAMLA_ELL(szamla);

            Szamla s = Szamlak.Find(x => x.Szamlaszam == szamla);
            s.Egyenleg += osszeg;

            Tranzakcio t = new Tranzakcio(KovTranzId(), szamla, "Befizetés",
                                          osszeg, DateTime.Now.ToShortDateString(), "");

            Tranzakciok.Add(t);
            Mentes();
            return t;
        }

        public Tranzakcio Kivetel(string szamla, double osszeg)
        {
            SZAMLA_ELL(szamla);

            Szamla s = Szamlak.Find(x => x.Szamlaszam == szamla);

            if (osszeg > s.Egyenleg)
                throw new InvalidOperationException("Nincs elegendő pénz a számlán!");

            s.Egyenleg -= osszeg;

            Tranzakcio t = new Tranzakcio(KovTranzId(), szamla, "Készpénzfelvétel",
                                          osszeg, DateTime.Now.ToShortDateString(), "");

            Tranzakciok.Add(t);
            Mentes();
            return t;
        }

        public Tranzakcio Atutalas(string forras, string cel, double osszeg)
        {
            SZAMLA_ELL(forras);
            SZAMLA_ELL(cel);

            if (forras == cel) throw new Exception("Nem utalhatsz saját magadnak.");

            Szamla fs = Szamlak.Find(x => x.Szamlaszam == forras);
            Szamla cs = Szamlak.Find(x => x.Szamlaszam == cel);

            if (fs.Egyenleg < osszeg)
                throw new Exception("Nincs elegendő pénz a számlán!");

            fs.Egyenleg -= osszeg;
            cs.Egyenleg += osszeg;

            Tranzakcio t = new Tranzakcio(KovTranzId(), forras, "Átutalás",
                                          osszeg, DateTime.Now.ToShortDateString(), cel);

            Tranzakciok.Add(t);
            Mentes();
            return t;
        }




        public List<Szamla> UgyfelSzamlai(int ugyfelId)
        {
            List<Szamla> eredmeny = new List<Szamla>();

            foreach (Szamla s in Szamlak)
            {
                if (s.UgyfelId == ugyfelId)
                {
                    eredmeny.Add(s);
                }
            }

            return eredmeny;
        }


        public List<Tranzakcio> SzamlaTranzakcioi(string szamlaszam)
        {
            List<Tranzakcio> eredmeny = new List<Tranzakcio>();

            foreach (Tranzakcio t in Tranzakciok)
            {
                if (t.Szamlaszam == szamlaszam || t.PartnerSzamlaszam == szamlaszam)
                {
                    eredmeny.Add(t);
                }
            }

            return eredmeny;
        }


        public List<Szamla> NegativSzamlak()
        {
            List<Szamla> eredmeny = new List<Szamla>();

            foreach (Szamla s in Szamlak)
            {
                if (s.Egyenleg < 0)
                {
                    eredmeny.Add(s);
                }
            }

            return eredmeny;
        }


        public List<Tranzakcio> TranzakciokIdoszakban(string kezdoDatum, string vegDatum)
        {
            List<Tranzakcio> talalatok = new List<Tranzakcio>();

            foreach (Tranzakcio tranz in Tranzakciok)
            {
                try
                {
                    DateTime tranzDatum = DateTime.Parse(tranz.Datum);
                    DateTime kezdo = DateTime.Parse(kezdoDatum);
                    DateTime veg = DateTime.Parse(vegDatum);

                    if (tranzDatum >= kezdo && tranzDatum <= veg)
                    {
                        talalatok.Add(tranz);
                    }
                }
                catch
                {
                    // rossz dátumformátum esetén átugorjuk
                }
            }

            return talalatok;
        }

        // Legnagyobb forgalmú ügyfél (összes tranzakció összege alapján)
        //public Ugyfel LegnagyobbForgalmuUgyfel()
        //{
        //    Ugyfel legnagyobb = null;
        //    int legnagyobbForgalom = int.MinValue;

        //    foreach (Ugyfel u in Ugyfelek)
        //    {
        //        int forgalom = 0;


        //        foreach (Szamla s in Szamlak)
        //        {
        //            if (s.UgyfelId == u.UgyfelId)
        //            {

        //                foreach (Tranzakcio t in Tranzakciok)
        //                {                                                                                       // kiszervezes
        //                    if (t.Szamlaszam == s.Szamlaszam ||
        //                        t.PartnerSzamlaszam == s.Szamlaszam)
        //                    {
        //                        forgalom += (int)t.Osszeg;
        //                    }
        //                }
        //            }
        //        }

        //        if (forgalom > legnagyobbForgalom)
        //        {
        //            legnagyobbForgalom = forgalom;
        //            legnagyobb = u;
        //        }
        //    }

        //    return legnagyobb;
        //}

        public Ugyfel LegnagyobbForgalmuUgyfel()
        {
            return Ugyfelek
                .Select(u => new { Ugyfel = u, Forgalom = GetUgyfelForgalom(u) })
                .OrderByDescending(x => x.Forgalom)
                .FirstOrDefault()?.Ugyfel;
        }

        private int GetUgyfelForgalom(Ugyfel ugyfel)
        {
            var szamlak = GetSzamlakByUgyfel(ugyfel.UgyfelId);

            int forgalom = 0;

            foreach (var sz in szamlak)
            {
                foreach (var t in GetTranzakciokBySzamla(sz))
                {
                    forgalom += (int)t.Osszeg;
                }
            }

            return forgalom;
        }

        private IEnumerable<Szamla> GetSzamlakByUgyfel(int ugyfelId)
        {
            return Szamlak.Where(s => s.UgyfelId == ugyfelId);
        }

        private IEnumerable<Tranzakcio> GetTranzakciokBySzamla(Szamla szamla)
        {
            return Tranzakciok.Where(t =>
                t.Szamlaszam == szamla.Szamlaszam ||
                t.PartnerSzamlaszam == szamla.Szamlaszam);
        }
    }
}
