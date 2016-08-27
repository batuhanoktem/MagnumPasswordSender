using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace MagnumSifreYollayici
{
    class Program
    {
        static void Main(string[] args)
        {
            CookieAwareWebClient client = new CookieAwareWebClient();
            client.Encoding = Encoding.UTF8;

            Send(client);
        }

        static void Send(CookieAwareWebClient client)
        {
            try
            {
                string captchaUrl = "https://www.magnum.com.tr/maserati/Captcha/CaptchaImage.aspx?rnd=0.19846584059179606";
                string sendUrl = "https://www.magnum.com.tr/maserati";

                List<string> sifreler = new List<string>();

                StreamReader sifrelerFile = new StreamReader("Sifreler.txt");
                while (!sifrelerFile.EndOfStream)
                {
                    string sifre = sifrelerFile.ReadLine().Trim();
                    if (sifre.Length == 8)
                        sifreler.Add(sifre);
                }
                sifrelerFile.Close();

                StreamWriter result = new StreamWriter("Sonuc.txt");

                int i = 0;
                int count = sifreler.Count;

                foreach (string sifre in sifreler)
                {
                    string message = sifre + " - ";
                    try
                    {
                        tryAgain:
                        string captcha = GetCaptcha(captchaUrl, client);

                        NameValueCollection data = new NameValueCollection();
                        data["webenv"] = "2";
                        data["sifre"] = sifre;
                        data["ad"] = ConfigurationManager.AppSettings["ad"].Trim();
                        data["soyad"] = ConfigurationManager.AppSettings["soyad"].Trim();
                        data["tel"] = ConfigurationManager.AppSettings["tel"].Trim();
                        data["email"] = ConfigurationManager.AppSettings["email"].Trim();
                        data["sehir"] = ConfigurationManager.AppSettings["sehir"].Trim();
                        data["adres"] = ConfigurationManager.AppSettings["adres"].Trim();
                        data["security"] = captcha;
                        data["accept"] = "1";

                        if (ConfigurationManager.AppSettings["newsletter"].Trim().ToLower() == "on")
                        {
                            data["subscribe"] = "1";
                            data["newsletter"] = "on";
                        }
                        else
                            data["newsletter"] = "off";

                        string json = PostData(sendUrl, data, client);
                        if (json == "captcha")
                            goto tryAgain;

                        Response response = JsonConvert.DeserializeObject<Response>(json);
                        if (response.IsValid)
                            message += " Başarılı - " + response.Message + " çekiliş hakkı kazandınız.";
                        else
                            message += " Hata oluştu - " + response.Message;
                    }
                    catch (Exception ex)
                    {
                        message += " Hata oluştu - " + ex.Message;
                    }

                    result.WriteLine(message);
                    Console.WriteLine(++i + " / " + count + " - " + message);
                    Task.Delay(1000).Wait();
                }
                result.Close();
            }
            catch (Exception)
            {
                
            }
        }

        static string GetCaptcha(string url, WebClient client)
        {
            Bitmap bitmap = GetBitmap(url, client);
            return GetTextFromBitmap(bitmap);
        }

        static Bitmap GetBitmap(string url, WebClient client)
        {
            Stream stream = client.OpenRead(url);
            Bitmap bitmap = new Bitmap(stream);
            stream.Flush();
            stream.Close();
            return bitmap;
        }

        static string GetTextFromBitmap(Bitmap bitmap)
        {
            using (var engine = new TesseractEngine("tessdata", "eng", EngineMode.Default))
            {
                engine.SetVariable("tessedit_char_whitelist", "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789");
                using (var pix = PixConverter.ToPix(bitmap))
                {
                    using (var page = engine.Process(pix))
                    {
                        string text = page.GetText();
                        if (text != null && text.Length > 0)
                            return text.Substring(0, 5);
                        else
                            return text;
                    }
                }
            }
        }

        static string PostData(string url, NameValueCollection data, WebClient client)
        {
            byte[] response = client.UploadValues(url, data);
            return Encoding.UTF8.GetString(response);
        }
    }
}
