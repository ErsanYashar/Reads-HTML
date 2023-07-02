using FlatRockTask;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System.Globalization;

HtmlDocument data = new HtmlDocument();
data.Load("data.html");

var productNames = data.DocumentNode.SelectNodes("//div[@class='item']//figure//a/img")
  .Select(x => x.GetAttributeValue("alt", "")).ToList();

var names = productNames.Select(x => HtmlAgilityPack.HtmlEntity.DeEntitize(x));

var productPrices = data.DocumentNode.SelectNodes("//div[@class='pricing']/p/span")
 .Select(x => x.InnerText).ToList();

var productRatings = data.DocumentNode.SelectNodes("/div[@class='item']")
    .Select(x => x.GetAttributeValue("rating", ""))
    .Select(x => decimal.Parse(x, CultureInfo.InvariantCulture))
    .ToList();

List<decimal> normalRatings = new List<decimal>();
List<Product> products = new List<Product>();

foreach (var item in productRatings)
{
    if (item > 10.00M)
    {
        normalRatings.Add((item / 100) * 5.0M);
    }
    else if (item > 5.00m)
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
    var price = productPrices.ElementAt(i).Split("$", StringSplitOptions.RemoveEmptyEntries).ToArray();

    products.Add(new Product
    {
        ProductName = names.ElementAt(i),
        Price = price[1].Replace(",", ""),
        Rating = Math.Round(normalRatings.ElementAt(i), 1)
    });
}

string productsJson = JsonConvert.SerializeObject(products, Formatting.Indented);

Console.WriteLine(productsJson);