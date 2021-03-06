﻿using System;
using System.Collections.Generic;
using DotnetSpider.Extension;
using DotnetSpider.Extension.Model;
using DotnetSpider.Extension.Model.Attribute;

using DotnetSpider.Core.Selector;
using DotnetSpider.Extension.Downloader;
using DotnetSpider.Core.Infrastructure;
using DotnetSpider.Core;

namespace DotnetSpider.Sample
{
	public class JdSkuWebDriverSample : EntitySpider
	{
		public JdSkuWebDriverSample() : base("JdSkuWebDriver", new Site())
		{
		}

		protected override void MyInit(params string[] arguments)
		{
			Identity = ("JD sku/store test " + DateTime.Now.ToString("yyyy-MM-dd HHmmss"));
			AddStartUrl("http://list.jd.com/list.html?cat=9987,653,655&page=2&JL=6_0_0&ms=5#J_main", new Dictionary<string, object> { { "name", "手机" }, { "cat3", "655" } });
			AddEntityType<Product>();
			Downloader = new WebDriverDownloader(Browser.Chrome);
		}

		[TargetUrlsSelector(XPaths = new[] { "//span[@class=\"p-num\"]" }, Patterns = new[] { @"&page=[0-9]+&" })]
		[EntityTable("test", "sku", EntityTable.Today, Indexs = new[] { "CategoryName" }, Uniques = new[] { "CategoryName,Sku", "Sku" })]
		[EntitySelector(Expression = "//li[@class='gl-item']/div[contains(@class,'j-sku-item')]")]
		class Product : SpiderEntity
		{
			[PropertyDefine(Expression = "name", Type = SelectorType.Enviroment, Length = 20)]
			public string CategoryName { get; set; }

			[PropertyDefine(Expression = "cat3", Type = SelectorType.Enviroment, Length = 20)]
			public int CategoryId { get; set; }

			[PropertyDefine(Expression = "./div[1]/a/@href", Length = 20)]
			public string Url { get; set; }


			[PropertyDefine(Expression = "./@data-sku", Length = 20)]
			public string Sku { get; set; }

			[PropertyDefine(Expression = "./div[5]/strong/a", Length = 20)]
			public long CommentsCount { get; set; }

			[PropertyDefine(Expression = ".//div[@class='p-shop']/@data-shop_name", Length = 20)]
			public string ShopName { get; set; }

			[PropertyDefine(Expression = ".//div[@class='p-name']/a/em", Length = 20)]
			public string Name { get; set; }

			[PropertyDefine(Expression = "./@venderid", Length = 20)]
			public string VenderId { get; set; }

			[PropertyDefine(Expression = "./@jdzy_shop_id", Length = 20)]
			public string JdzyShopId { get; set; }

			[PropertyDefine(Expression = "Monday", Type = SelectorType.Enviroment)]
			public DateTime RunId { get; set; }
		}
	}
}