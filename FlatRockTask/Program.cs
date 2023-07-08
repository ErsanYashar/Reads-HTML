using FlatRockTask;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Globalization;

HtmlDocument data = new HtmlDocument();
data.Load("data.html");

var productNames = data.DocumentNode.SelectNodes("//div[@class='item']//figure//a/img")
    .Select(x => x.GetAttributeValue("alt", ""))
    .Select(x => HtmlAgilityPack.HtmlEntity.DeEntitize(x))
    .ToList();

var productPrices = data.DocumentNode.SelectNodes("//div[@class='pricing']/p/span")
    .Select(x => x.InnerText)
    .ToList();

var productRatings = data.DocumentNode.SelectNodes("/div[@class='item']")
    .Select(x => x.GetAttributeValue("rating", ""))
    .Select(x => decimal.Parse(x, CultureInfo.InvariantCulture))
    .ToList();

List<decimal> normalRatings = new List<decimal>();
List<Product> products = new List<Product>();

foreach (var item in productRatings)
{

    if (item > 5.00M)
    {
        normalRatings.Add((item / 10) * 5.0M);
    }
    else
    {
        normalRatings.Add(item);
    }
}

for (int i = 0; i < productNames.Count; i++)
{
    var price = productPrices[i].Split("$", StringSplitOptions.RemoveEmptyEntries).ToArray();

    products.Add(new Product
    {
        ProductName = productNames[i],
        Price = price[1].Replace(",", ""),
        Rating = Math.Round(normalRatings[i], 1)
    });
}

string productsJson = JsonConvert.SerializeObject(products, Formatting.Indented);

Console.WriteLine(productsJson);