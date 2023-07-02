using HtmlAgilityPack;

HtmlDocument data = new HtmlDocument();
data.Load("data.html");

var productNames = data.DocumentNode.SelectNodes("//div[@class='item']//figure//a/img")
  .Select(x => x.GetAttributeValue("alt", "")).ToList();

var names = productNames.Select(x => HtmlAgilityPack.HtmlEntity.DeEntitize(x));

var productPrices = data.DocumentNode.SelectNodes("//div[@class='pricing']/p/span")
 .Select(x => x.InnerText).ToList();

var productRatings = data.DocumentNode.SelectNodes("/div[@class='item']")
    .Select(x => x.GetAttributeValue("rating", ""))
    .ToList();