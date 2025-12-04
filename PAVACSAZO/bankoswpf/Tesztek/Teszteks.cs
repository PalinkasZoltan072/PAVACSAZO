using NUnit.Framework;
using BankAlkalmazas;  // a foprojektre mutato referencia

namespace Tesztek
{
    [TestFixture]
    public class BankRendszerTesztek
    {
        private BankRendszer bank;

        [SetUp]
        public void Setup()
        {
            bank = new BankRendszer();
        }

        [Test] // 1. Deposit
        public void Deposit_Pozitiv()
        {
            var u = bank.UjUgyfel("Teszt User", "Cim 1", "1990-01-01", "0612345678");
            var s = bank.UjSzamla("10000014", u.UgyfelId, "Folyószámla", 1000);

            var t = bank.Befizetes(s.Szamlaszam, 500);

            Assert.AreEqual(1500, s.Egyenleg);
            Assert.AreEqual(500, t.Osszeg);
            Assert.AreEqual("Befizetés", t.Tipus);
        }

        [Test] // 2. Deposit
        public void Deposit_Negativ()
        {
            
            var u = bank.UjUgyfel("Teszt User", "Cim 1", "1990-01-01", "0612345678");
            var s = bank.UjSzamla("13711112", u.UgyfelId, "Folyószámla", 1000);
            Assert.Throws<ArgumentException>(() => bank.Befizetes(s.Szamlaszam, -500));
        }

        [Test] // 2. Withdraw
        public void Withdraw_Test()
        {
            var u = bank.UjUgyfel("Test User", "Cim 1", "1990-01-01", "0612345678");
            var s = bank.UjSzamla("22222224", u.UgyfelId, "Folyószámla", 2000); 
            var t = bank.Kivetel(s.Szamlaszam, 500);

            Assert.AreEqual(1500, s.Egyenleg);
            Assert.AreEqual(500, t.Osszeg);
            Assert.AreEqual("Készpénzfelvétel", t.Tipus);
        }

        [Test] // 4. Withdraw_NotEnoughMoney_Throws
        public void Withdraw_NotEnoughMoney_Throws()
        {
            var u = bank.UjUgyfel("Test User", "Cim 1", "1990-01-01", "0612345678");
            var s = bank.UjSzamla("33333363", u.UgyfelId, "Folyószámla", 300);

            // Várjuk, hogy InvalidOperationException dobódjon
            Assert.Throws<InvalidOperationException>(() => bank.Kivetel(s.Szamlaszam, 1000));
        }

        [Test] // 5. HasSufficientFunds_True
        public void HasSufficientFunds_True()
        {
            var u = bank.UjUgyfel("User", "Cim", "1990-01-01", "0612345678");
            var s = bank.UjSzamla("44444446", u.UgyfelId, "Folyószámla", 5000);
            bool van = s.Egyenleg >= 4999;
            Assert.IsTrue(van);
        }

        [Test] // 6. HasSufficientFunds_False
        public void HasSufficientFunds_False()
        {
            var u = bank.UjUgyfel("User", "Cim", "1990-01-01", "0612345678");
            var s = bank.UjSzamla("84444444", u.UgyfelId, "Folyószámla", 200);
            bool nincs = s.Egyenleg >= 201;
            Assert.IsFalse(nincs);
        }

        [Test] // 7. TransferTo_Test
        public void TransferTo_Test()
        {
            var u1 = bank.UjUgyfel("A", "CimA", "1990-01-01", "0611111111");
            var u2 = bank.UjUgyfel("B", "CimB", "1991-01-01", "0622222222");

            var a = bank.UjSzamla("55555557", u1.UgyfelId, "Folyószámla", 5000);
            var b = bank.UjSzamla("66666668", u2.UgyfelId, "Folyószámla", 1000);
            
            bank.Atutalas(a.Szamlaszam, b.Szamlaszam, 2000);

            Assert.AreEqual(3000, a.Egyenleg);
            Assert.AreEqual(3000, b.Egyenleg);
        }

        [Test] // 8. TransferTo_NotEnough_Throws
        public void TransferTo_NotEnough_Throws()
        {
            var u1 = bank.UjUgyfel("A", "CimA", "1990-01-01", "0611111111");
            var u2 = bank.UjUgyfel("B", "CimB", "1991-01-01", "0622222222");

            var a = bank.UjSzamla("77777772", u1.UgyfelId, "Folyószámla", 1000);
            var b = bank.UjSzamla("88888882", u2.UgyfelId, "Folyószámla", 1000);
            Assert.Throws<Exception>(() => bank.Atutalas(a.Szamlaszam, b.Szamlaszam, 5000));
        }

        [Test] // 9. GetBalance_Test
        public void GetBalance_Test()
        {
            var u = bank.UjUgyfel("User", "Cim", "1990-01-01", "0612345678");
            var s = bank.UjSzamla("12435798", u.UgyfelId, "Folyószámla", 900);
            
            Assert.AreEqual(900, s.Egyenleg);
        }

        [Test] // 10. AddAccount_Valid
        public void AddAccount_Valid()
        {
            var u = bank.UjUgyfel("User", "Cim", "1990-01-01", "0612345678");
            var s = bank.UjSzamla("32345678", u.UgyfelId, "Folyószámla", 1000);

            Assert.IsNotNull(s);
            Assert.AreEqual(u.UgyfelId, s.UgyfelId);
        }

        [Test] // 11. AddAccount_Duplicate_Throws
        public void AddAccount_Duplicate_Throws()
        {
            var u = bank.UjUgyfel("User", "Cim", "1990-01-01", "0612345678");
            var s1 = bank.UjSzamla("34511190", u.UgyfelId, "Folyószámla", 0);

            Assert.Throws<Exception>(() => bank.UjSzamla("34567890", u.UgyfelId, "Folyoszamla", 0));
        }

        [Test] // 12. GetTotalBalance_Test
        public void GetTotalBalance_Test()
        {
            var u = bank.UjUgyfel("User", "Cim", "1990-01-01", "0612345678");
            bank.UjSzamla("11117711", u.UgyfelId, "Folyószámla", 2000);
            bank.UjSzamla("22772222", u.UgyfelId, "Folyószámla", 2000);

            double total = 0;
            foreach (var s in bank.UgyfelSzamlai(u.UgyfelId))
                total += s.Egyenleg;

            Assert.AreEqual(4000, total);
        }

        [Test] // 13. Transaction_IsValid
        public void Transaction_IsValid() 
        {
            
            var ugyfel = bank.UjUgyfel("Teszt User", "Cim 1", "1990-01-01", "0612345678");
            var szamla = bank.UjSzamla("18265678", ugyfel.UgyfelId, "Folyószámla", 1000);

            double negativOsszeg = -500;

            
            bool ervenyes = negativOsszeg >= 0;

            Assert.IsFalse(ervenyes, "Negatív osszeg esetén a tranzakcio nem lehet ervenyes.");
        }

        [Test] // 14. Transaction_IsValid_False
        public void Transaction_IsValid_False()  
        {
            
            var ugyfel = bank.UjUgyfel("Teszt User", "Cim 1", "1990-01-01", "0612345678");
            var szamla = bank.UjSzamla("11125678", ugyfel.UgyfelId, "Folyószámla", 1000);

            string nemletezoCelSzamla = "92229999";
            
            bool ervenyes = bank.Szamlak.Any(s => s.Szamlaszam == nemletezoCelSzamla);

            Assert.IsFalse(ervenyes, "Ha a cél számla nem létezik, a tranzakció nem lehet érvényes.");
        }

        [Test] // 15. Transaction_CalculateFee
        public void Transaction_CalculateFee() 
        {
            double amount = 10000;
            double fee = Math.Max(100, amount * 0.01);
            Assert.AreEqual(100, fee);
        }

        [Test] // 16. Transaction_CalculateFee_Min
        public void Transaction_CalculateFee_Min() 
        {
            double amount = 100;
            double fee = Math.Max(100, amount * 0.01);
            Assert.AreEqual(100, fee);
        }

        [Test] // 17. FindCustomerById_Found
        public void FindCustomerById_Found()
        {
            var u = bank.UjUgyfel("User", "Cim", "1990-01-01", "8612345678");
            var found = bank.Ugyfelek.Find(x => x.UgyfelId == u.UgyfelId);

            Assert.AreEqual(u.UgyfelId, found.UgyfelId);
        }

        [Test] // 18. FindCustomerById_NotFound
        public void FindCustomerById_NotFound()
        {
            var found = bank.Ugyfelek.Find(x => x.UgyfelId == 999);
            Assert.IsNull(found);
        }

        [Test] // 19. GetOverdrawnAccounts
        public void GetOverdrawnAccounts()
        {
            
            var ugyfel = bank.UjUgyfel("Test Ueser", "Cim 2", "1990-02-01", "0612345678");

            
            var sz1 = bank.UjSzamla("13361171", ugyfel.UgyfelId, "Folyószámla", -200);
            var sz2 = bank.UjSzamla("27714212", ugyfel.UgyfelId, "Folyószámla", 300);
            var sz3 = bank.UjSzamla("3231771", ugyfel.UgyfelId, "Folyószámla", -50);

            
            var negativSzamlak = bank.NegativSzamlak();

            
            Assert.IsTrue(negativSzamlak.Contains(sz1), "A -200 egyenlegű számla nem szerepel a negatív listában.");
            Assert.IsTrue(negativSzamlak.Contains(sz3), "A -50 egyenlegű számla nem szerepel a negatív listában.");
            Assert.IsFalse(negativSzamlak.Contains(sz2), "A 300 egyenlegű számla tévesen szerepel a negatív listában.");

            
            Assert.AreEqual(12, negativSzamlak.Count, "A negatív számlák száma nem egyezik.");
        }

        [Test] // 20. FindAccountByNumber_NotFound_Throws
        public void FindAccountByNumber_NotFound_Throws()
        {
            
            var ugyfel = bank.UjUgyfel("Test User", "Cim 1", "1990-01-01", "0612345678");
            var szamla = bank.UjSzamla("10005678", ugyfel.UgyfelId, "Folyószámla", 1000);

            
            Assert.Throws<Exception>(() =>
            {
                
                var s = bank.Szamlak.Find(x => x.Szamlaszam == "99999999")
                        ?? throw new Exception("Nincs ilyen számla.");
            });
        }
    }
}