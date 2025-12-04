using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace BankAlkalmazas
{
    public partial class MainWindow : Window
    {
        private BankRendszer bank;

        public MainWindow()
        {
            InitializeComponent();
            bank = new BankRendszer();
            AlapListakFrissitese();
        }

       
        private void AlapListakFrissitese()
        {
            lstUgyfelek.ItemsSource = null;
            lstUgyfelek.ItemsSource = bank.Ugyfelek;

            lstSzamlak.ItemsSource = null;
            lstSzamlak.ItemsSource = bank.Szamlak;

            lstTranzakciok.ItemsSource = null;
            lstTranzakciok.ItemsSource = bank.Tranzakciok;
        }

        private void btnAlapListak_Click(object sender, RoutedEventArgs e)
        {
            AlapListakFrissitese();
        }

        

        private void lstUgyfelek_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Ugyfel kivalasztott = lstUgyfelek.SelectedItem as Ugyfel;
            if (kivalasztott == null) return;

            List<Szamla> lista = bank.UgyfelSzamlai(kivalasztott.UgyfelId);

            lstSzamlak.ItemsSource = null;
            lstSzamlak.ItemsSource = lista;

            lstTranzakciok.ItemsSource = null;
        }

        private void lstSzamlak_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Szamla kivalasztott = lstSzamlak.SelectedItem as Szamla;
            if (kivalasztott == null) return;

            List<Tranzakcio> lista = bank.SzamlaTranzakcioi(kivalasztott.Szamlaszam);

            lstTranzakciok.ItemsSource = null;
            lstTranzakciok.ItemsSource = lista;
        }

        

        private void btnUjUgyfel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string nev = txtNev.Text;
                string lakcim = txtLakcim.Text;
                string szul = txtSzul.Text;
                string tel = txtTelefon.Text;

                var u = bank.UjUgyfel(nev, lakcim, szul, tel);
                MessageBox.Show("Új ügyfél felvéve. ID: " + u.UgyfelId, "Siker");

                txtNev.Clear();
                txtLakcim.Clear();
                txtSzul.Clear();
                txtTelefon.Clear();

                AlapListakFrissitese();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba: " + ex.Message);
            }
        }

        private void btnUjSzamla_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int ugyfelId = int.Parse(txtUgyfelId.Text);

                if (cmbSzamlaTipus.SelectedItem == null)
                {
                    MessageBox.Show("Válassz számlatípust!");
                    return;
                }
                string tipus = (cmbSzamlaTipus.SelectedItem as ComboBoxItem).Content.ToString();

                double kezdo = double.Parse(txtKezdoOsszeg.Text);

                string szamlaszam = txtSzamlaszam.Text;

                Szamla s = bank.UjSzamla(szamlaszam, ugyfelId, tipus, kezdo);

                MessageBox.Show("Számla létrehozva. Számlaszám: " + s.Szamlaszam, "Siker");

                txtUgyfelId.Clear();
                txtSzamlaszam.Clear();
                cmbSzamlaTipus.SelectedIndex = -1;
                txtKezdoOsszeg.Clear();

                AlapListakFrissitese();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba: " + ex.Message);
            }
        }


        private void btnUjTranzakcio_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbTranzTipus.SelectedItem == null)
                {
                    MessageBox.Show("Válassz tranzakció típust!");
                    return;
                }

                string tipus = (cmbTranzTipus.SelectedItem as ComboBoxItem).Content.ToString();
                double osszeg = double.Parse(txtTranzOsszeg.Text);

                if (tipus == "Befizetés")
                {
                    string szamla = txtTranzSzamla.Text;
                    bank.Befizetes(szamla, osszeg);
                }
                else if (tipus == "Készpénzfelvétel")
                {
                    string szamla = txtTranzSzamla.Text;
                    bank.Kivetel(szamla, osszeg);
                }
                else if (tipus == "Átutalás")
                {
                    string forras = txtForrasSzamla.Text;
                    string cel = txtCelSzamla.Text;
                    bank.Atutalas(forras, cel, osszeg);
                }

                MessageBox.Show("Tranzakció rögzítve.", "Siker");

                txtTranzSzamla.Clear();
                txtForrasSzamla.Clear();
                txtCelSzamla.Clear();
                txtTranzOsszeg.Clear();
                cmbTranzTipus.SelectedIndex = -1;

                AlapListakFrissitese();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba: " + ex.Message);
            }
        }



        
        private void btnUgyfelNevSzures_Click(object sender, RoutedEventArgs e)
        {
            string nev = txtUgyfelNevSzures.Text;

            var eredmeny = new List<Ugyfel>();

            foreach (var u in bank.Ugyfelek)
            {
                if (u.Nev == nev)   
                    eredmeny.Add(u);
            }

            lstUgyfelek.ItemsSource = null;
            lstUgyfelek.ItemsSource = eredmeny;
        }


        private void btnSzamlakSzures_Click(object sender, RoutedEventArgs e)
        {
            string keresettTipus = txtSzamlaTipusSzures.Text;

            int min;
            int max;

            // ha üres, akkor ne korlátozzon lefelé
            if (string.IsNullOrWhiteSpace(txtMinEgyenleg.Text))
            {
                min = int.MinValue;
            }
            else
            {
                min = int.Parse(txtMinEgyenleg.Text);
            }

            // ha üres, akkor ne korlátozzon felfelé
            if (string.IsNullOrWhiteSpace(txtMaxEgyenleg.Text))
            {
                max = int.MaxValue;
            }
            else
            {
                max = int.Parse(txtMaxEgyenleg.Text);
            }

            List<Szamla> eredmeny = new List<Szamla>();

            foreach (var s in bank.Szamlak)
            {
                bool tipusEgyezik = (s.SzamlaTipus == keresettTipus);      // csak pontos egyezés
                bool egyenlegJo = (s.Egyenleg >= min && s.Egyenleg <= max); // tartományba esik

                if (tipusEgyezik && egyenlegJo)
                {
                    eredmeny.Add(s);
                }
            }

            lstSzamlak.ItemsSource = null;
            lstSzamlak.ItemsSource = eredmeny;
        }



        private void btnNapiTranzakcio_Click(object sender, RoutedEventArgs e)
        {
            if (dpTranzNap.SelectedDate == null)
            {
                MessageBox.Show("Válassz dátumot!");
                return;
            }

            string datum = dpTranzNap.SelectedDate.Value.ToShortDateString();
            List<Tranzakcio> lista = bank.TranzakciokIdoszakban(datum, datum);

            lstTranzakciok.ItemsSource = null;
            lstTranzakciok.ItemsSource = lista;
        }

        

        private void btnNegativSzamlak_Click(object sender, RoutedEventArgs e)
        {
            List<Szamla> lista = bank.NegativSzamlak();

            lstSzamlak.ItemsSource = null;
            lstSzamlak.ItemsSource = lista;
        }

        private void btnLegnagyobbForgalom_Click(object sender, RoutedEventArgs e)
        {
            Ugyfel u = bank.LegnagyobbForgalmuUgyfel();

            if (u == null)
            {
                lblLegnagyobbForgalom.Text = "Nincs adat.";
            }
            else
            {
                lblLegnagyobbForgalom.Text = "Ügyfél: " + u.Nev + " (ID: " + u.UgyfelId + ")";
            }
        }

        private void btnIdoszakTranzakciok_Click(object sender, RoutedEventArgs e)
        {
            if (dpIdoszakKezdo.SelectedDate == null || dpIdoszakVeg.SelectedDate == null)
            {
                MessageBox.Show("Válaszd ki a kezdő és vég dátumot!");
                return;
            }

            string kezdo = dpIdoszakKezdo.SelectedDate.Value.ToShortDateString();
            string veg = dpIdoszakVeg.SelectedDate.Value.ToShortDateString();

            List<Tranzakcio> lista = bank.TranzakciokIdoszakban(kezdo, veg);

            lstTranzakciok.ItemsSource = null;
            lstTranzakciok.ItemsSource = lista;
        }
    }
}
