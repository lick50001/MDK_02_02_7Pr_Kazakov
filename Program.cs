using System;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using HtmlAgilityPack;
using System.Threading.Tasks;
using System.Reflection;
using System.Xml.Linq;

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

                if (!response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Ошибка загрузки старницы");
                    Console.WriteLine($"Ошибка: {response.StatusCode}");
                    return;
                }
                string html = await response.Content.ReadAsStringAsync();
                Debug.WriteLine($"Загружено символов HTML: {html.Length}");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html);

                var productNodes = doc.DocumentNode.SelectNodes("//div[@data-name and @data-price]");

                if (productNodes == null || productNodes.Count == 0)
                {
                    Debug.WriteLine("Товары не найдены.");
                    Console.WriteLine("Товары не найдены.");
                    return;
                }

                Debug.WriteLine($"Найдено товаров на странице: {productNodes.Count}");
                Console.WriteLine("Бестселлеры маркетплейсоа Лабиринт:\n");

                int index = 1;
                foreach ( var product in productNodes )
                {
                    string id = product.GetAttributeValue("data-product-id", "");
                    string name = WebUtility.HtmlDecode(product.GetAttributeValue("data-name", "Без названия"));
                    string price = product.GetAttributeValue("data-price", "0");
                    string discountPrice = product.GetAttributeValue("data-discount-price", "price");
                    string link = $"https://www.labirint.ru/books/{id}/";

                    Console.WriteLine($"{index}.{name}");
                    Console.WriteLine($"Цена: {discountPrice} руб.(без скидки: {price} руб)");
                    Console.WriteLine($"Ссылка: {link}\n");

                    Debug.WriteLine($"[{index}] ID={id} | {name} | {discountPrice} руб.");
                    index++;
                }
            }
            Console.WriteLine("Нажмите любую клавишу для входа...");
            Console.ReadKey();
        }
    }
}
