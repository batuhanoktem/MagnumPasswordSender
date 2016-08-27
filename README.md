# Magnum Şifre Yollayıcı

## Bağış
  - Yapılan bağışlar projenin gelişmesine katkıda bulunacaktır.
  - Bitcoin: 1FHFU14DRWWHaJyq8fK1coKR57KqQJg2ZH

## Kullanım
  - [Visual Studio 2015 için Visual C++ Yeniden Dağıtılabilir Paketleri](https://www.microsoft.com/tr-TR/download/details.aspx?id=48145 "Visual Studio 2015 için Visual C++ Yeniden Dağıtılabilir Paketleri") kurulu değilse kurun.
  - `Sifreler.txt` dosyasına alt alta olacak şekilde şifreleri girin.
    - `XXXXXXXX`
    - `YYYYYYYY`
    - `ZZZZZZZZ`
    - şeklinde
  - `MagnumSifreYollayici.exe.config` dosyasında
    - `<add key="ad" value=""/>` kısmına `ad` girin.
    - `<add key="soyad" value=""/>` kısmına `soyad` girin.
    - `<add key="tel" value="(xxx) xxx-xxxx"/>` kısmına aynı `(xxx) xxx-xxxx` formatında 10 haneli `telefon` girin.
    - `<add key="email" value=""/>` kısmına `e-posta` girin
    - `<add key="sehir" value=""/>` kısmına `şehrin plaka kodunu` başlangıçta 0 kullanmayarak girin.
    - `<add key="adres" value=""/>` kısmına `adres` girin.
    - `<add key="newsletter" value=""/>` kısmına Magnum'dan SMS ve E-posta aracılığıyla haberdar olmak istiyorsanız `on` istemiyorsanız `off` girin.
  - `MagnumSifreYollayici.exe` dosyasını çalıştırın.
  - Programın çalışması bittiğinde `Sonuc.txt` dosyasının oluştuğunu göreceksiniz.

## Kullanılan Kütüphaneler:
  - [Json.NET](https://github.com/JamesNK/Newtonsoft.Json "Json.NET")
  - [A .Net wrapper for tesseract-ocr](https://github.com/charlesw/tesseract "A .Net wrapper for tesseract-ocr")
  