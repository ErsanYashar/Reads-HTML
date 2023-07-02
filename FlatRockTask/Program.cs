using FlatRockTask;
using HtmlAgilityPack;
using System.Globalization;

HtmlDocument data = new HtmlDocument();
data.Load("data.html");

var productNames = data.DocumentNode.SelectNodes("//div[@class='item']//figure//a/img")
  .Select(x => x.GetAttributeValue("alt", "")).ToList();

var names = productNames.Select(x => HtmlAgilityPack.HtmlEntity.DeEntitize(x));

var productPrices = data.DocumentNode.SelectNodes("//div[@class='pricing']/p/span")
 .Select(x => x.InnerText).ToList();

var productRatings = data.DocumentNode.SelectNodes("/div[@class='item']")
    .Select(x => x.GetAttributeValue("rating", "")).Select(x => decimal.Parse(x, CultureInfo.InvariantCulture))
    .ToList();

//List<decimal> ratings = productRatings.Select(x => decimal.Parse(x, CultureInfo.InvariantCulture)).ToList();
List<decimal> normalRatings = new List<decimal>();

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

var product = new List<Product>();