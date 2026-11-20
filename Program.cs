using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;

namespace _7Pr
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string url = "https://www.labirint.ru/best/";

            using(HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

                Debug.WriteLine($"Отправка Get-запроса: {url}");
                HttpResponseMessage response = await client.GetAsync(url);
                Debug.WriteLine($"Код ответа сервера: {(int)response.StatusCode}");

            }
        }
    }
}
