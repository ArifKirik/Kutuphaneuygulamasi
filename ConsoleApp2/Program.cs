using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    
    public class Kitap
    {
        public string Ad { get; set; }
        public string Yazar { get; set; }
        public int YayınYılı { get; set; }
        public int Stok { get; set; }
    }

    
    public class Kiralama
    {
        public string KullanıcıAdı { get; set; }
        public string KitapAdı { get; set; }
        public DateTime IadeTarihi { get; set; }
    }

    static List<Kitap> kitaplar = new List<Kitap>();
    static List<Kiralama> kiralamaBilgileri = new List<Kiralama>();

    static void Main(string[] args)
    {
        while (true)
        {
            Console.WriteLine("--- Kütüphane Yönetim Sistemi ---");
            Console.WriteLine("1. Kitap Ekle");
            Console.WriteLine("2. Kitap Kirala");
            Console.WriteLine("3. Kitap İade");
            Console.WriteLine("4. Kitap Arama");
            Console.WriteLine("5. Raporlama");
            Console.WriteLine("6. Çıkış");
            Console.Write("Seçiminizi yapın: ");

            switch (Console.ReadLine())
            {
                case "1":
                    KitapEkle();
                    break;
                case "2":
                    KitapKirala();
                    break;
                case "3":
                    KitapIade();
                    break;
                case "4":
                    KitapArama();
                    break;
                case "5":
                    Raporlama();
                    break;
                case "6":
                    return;
                default:
                    Console.WriteLine("Geçersiz seçim! Lütfen tekrar deneyin.");
                    break;
            }
        }
    }

    static void KitapEkle()
    {
        Console.Write("Kitap adı: ");
        string ad = Console.ReadLine();
        var mevcutKitap = kitaplar.FirstOrDefault(k => k.Ad == ad);

        if (mevcutKitap != null)
        {
            Console.Write("Eklenecek stok miktarı: ");
            if (int.TryParse(Console.ReadLine(), out int miktar))
            {
                mevcutKitap.Stok += miktar;
                Console.WriteLine("Stok başarıyla güncellendi.");
            }
            else
            {
                Console.WriteLine("Geçersiz miktar!");
            }
        }
        else
        {
            Console.Write("Yazar adı: ");
            string yazar = Console.ReadLine();
            Console.Write("Yayın yılı: ");
            if (int.TryParse(Console.ReadLine(), out int yayinYili))
            {
                Console.Write("Stok miktarı: ");
                if (int.TryParse(Console.ReadLine(), out int stok))
                {
                    kitaplar.Add(new Kitap { Ad = ad, Yazar = yazar, YayınYılı = yayinYili, Stok = stok });
                    Console.WriteLine("Kitap başarıyla eklendi.");
                }
                else
                {
                    Console.WriteLine("Geçersiz stok miktarı!");
                }
            }
            else
            {
                Console.WriteLine("Geçersiz yayın yılı!");
            }
        }
    }

    static void KitapKirala()
    {
        if (kitaplar.Count == 0)
        {
            Console.WriteLine("Kütüphanede kitap yok.");
            return;
        }

        Console.WriteLine("--- Mevcut Kitaplar ---");
        for (int i = 0; i < kitaplar.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {kitaplar[i].Ad} ({kitaplar[i].Stok} adet)");
        }

        Console.Write("Kiralamak istediğiniz kitabın numarasını seçin: ");
        if (int.TryParse(Console.ReadLine(), out int secim) && secim >= 1 && secim <= kitaplar.Count)
        {
            var secilenKitap = kitaplar[secim - 1];
            if (secilenKitap.Stok > 0)
            {
                Console.Write("Kiralamak istediğiniz gün sayısı: ");
                if (int.TryParse(Console.ReadLine(), out int gunSayisi))
                {
                    int kiraUcreti = gunSayisi * 5;
                    Console.Write($"Bütçenizi girin (TL): ");
                    if (int.TryParse(Console.ReadLine(), out int butce))
                    {
                        if (butce >= kiraUcreti)
                        {
                            Console.Write("Adınızı girin: ");
                            string ad = Console.ReadLine();
                            secilenKitap.Stok--;
                            kiralamaBilgileri.Add(new Kiralama
                            {
                                KullanıcıAdı = ad,
                                KitapAdı = secilenKitap.Ad,
                                IadeTarihi = DateTime.Now.AddDays(gunSayisi)
                            });
                            Console.WriteLine("Kitap başarıyla kiralandı.");
                        }
                        else
                        {
                            Console.WriteLine("Bütçeniz yeterli değil!");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Geçersiz bütçe!");
                    }
                }
                else
                {
                    Console.WriteLine("Geçersiz gün sayısı!");
                }
            }
            else
            {
                Console.WriteLine("Stokta yeterli kitap yok.");
            }
        }
        else
        {
            Console.WriteLine("Geçersiz seçim!");
        }
    }

    static void KitapIade()
    {
        Console.Write("İade edeceğiniz kitabın adını girin: ");
        string ad = Console.ReadLine();
        var kiralama = kiralamaBilgileri.FirstOrDefault(k => k.KitapAdı == ad);

        if (kiralama != null)
        {
            var kitap = kitaplar.FirstOrDefault(k => k.Ad == ad);
            kitap.Stok++;
            kiralamaBilgileri.Remove(kiralama);
            Console.WriteLine("Kitap başarıyla iade edildi.");
        }
        else
        {
            Console.WriteLine("Bu kitap şu anda kirada değil.");
        }
    }

    static void KitapArama()
    {
        Console.WriteLine("1. Kitap adına göre ara");
        Console.WriteLine("2. Yazar adına göre ara");
        Console.Write("Seçiminiz: ");
        string secim = Console.ReadLine();

        if (secim == "1")
        {
            Console.Write("Kitap adı: ");
            string ad = Console.ReadLine();
            var sonuc = kitaplar.Where(k => k.Ad.Contains(ad, StringComparison.OrdinalIgnoreCase)).ToList();
            Listele(sonuc);
        }
        else if (secim == "2")
        {
            Console.Write("Yazar adı: ");
            string yazar = Console.ReadLine();
            var sonuc = kitaplar.Where(k => k.Yazar.Contains(yazar, StringComparison.OrdinalIgnoreCase)).ToList();
            Listele(sonuc);
        }
        else
        {
            Console.WriteLine("Geçersiz seçim!");
        }
    }

    static void Listele(List<Kitap> kitapListesi)
    {
        if (kitapListesi.Count == 0)
        {
            Console.WriteLine("Hiçbir sonuç bulunamadı.");
        }
        else
        {
            Console.WriteLine("--- Arama Sonuçları ---");
            foreach (var kitap in kitapListesi)
            {
                Console.WriteLine($"Ad: {kitap.Ad}, Yazar: {kitap.Yazar}, Yayın Yılı: {kitap.YayınYılı}, Stok: {kitap.Stok}");
            }
        }
    }

    static void Raporlama()
    {
        Console.WriteLine("1. Tüm kitapları listele");
        Console.WriteLine("2. Belirli bir yazara ait kitapları listele");
        Console.WriteLine("3. Belirli bir yayın yılına ait kitapları listele");
        Console.WriteLine("4. Kirada olan kitapları listele");
        Console.Write("Seçiminiz: ");
        string secim = Console.ReadLine();

        if (secim == "1")
        {
            Listele(kitaplar);
        }
        else if (secim == "2")
        {
            Console.Write("Yazar adı: ");
            string yazar = Console.ReadLine();
            var sonuc = kitaplar.Where(k => k.Yazar.Contains(yazar, StringComparison.OrdinalIgnoreCase)).ToList();
            Listele(sonuc);
        }
        else if (secim == "3")
        {
            Console.Write("Yayın yılı: ");
            if (int.TryParse(Console.ReadLine(), out int yil))
            {
                var sonuc = kitaplar.Where(k => k.YayınYılı == yil).ToList();
                Listele(sonuc);
            }
            else
            {
                Console.WriteLine("Geçersiz yıl!");
            }
        }
        else if (secim == "4")
        {
            if (kiralamaBilgileri.Count == 0)
            {
                Console.WriteLine("Kirada olan kitap yok.");
            }
            else
            {
                Console.WriteLine("--- Kirada Olan Kitaplar ---");
                foreach (var kiralama in kiralamaBilgileri)
                {
                    Console.WriteLine($"Kitap Adı: {kiralama.KitapAdı}, Kullanıcı: {kiralama.KullanıcıAdı}, İade Tarihi: {kiralama.IadeTarihi.ToShortDateString()}");
                }
            }
        }
        else
        {
            Console.WriteLine("Geçersiz seçim!");
        }
    }
}
