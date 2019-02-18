using System;
using System.Collections.Generic;
using System.Net;
using System.Xml;
using Ags.Web.Areas.Admin.Models;
using Ags.Web.Framework.Components;
using Ags.Web.Models;
using Ags.Web.Models.Common;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Mvc;

namespace Ags.Web.Components
{
    public class DovizViewComponent : AgsViewComponent
    {
        public IViewComponentResult Invoke()
        {
            XmlDocument xmlVerisi = new XmlDocument();
            xmlVerisi.Load("http://www.tcmb.gov.tr/kurlar/today.xml");
            decimal dolar = Convert.ToDecimal(xmlVerisi.SelectSingleNode(
                "Tarih_Date/Currency[@Kod='USD']/ForexSelling").InnerText.Replace('.', ','));

            decimal euro = Convert.ToDecimal(xmlVerisi.SelectSingleNode(
                "Tarih_Date/Currency[@Kod='EUR']/ForexSelling").InnerText.Replace('.', ','));

            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlAgilityPack.HtmlDocument();
            htmldoc.LoadHtml(GetContent("http://finans.mynet.com/"));
            HtmlNodeCollection basliklar = htmldoc.DocumentNode.SelectNodes("//*[@id=\"tabs-0\"]/table/tbody/tr[2]/td[2]");
            List<string> liste = new List<string>();
           foreach (var baslik in basliklar)
            {
                liste.Add(baslik.InnerText);

            }

            DovizModel model = new DovizModel
            {
                Dolar = dolar.ToString("##.###"),
                Euro = euro.ToString("##.###"),
                Gold = liste[1]
            };
            return View(model);
        }


        private static string GetContent(string urlAddress)
        {
            Uri url = new Uri(urlAddress);
            WebClient client = new WebClient();
            client.Encoding = System.Text.Encoding.UTF8;

            string html = client.DownloadString(url);
            return html;
        }


    }
}