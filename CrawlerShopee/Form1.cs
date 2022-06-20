using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text.Json;
using System.Net;
using System.IO;

namespace CrawlerShopee
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)
        {            
            var url = "https://shopee.com.br/api/v4/search/search_items?by=relevancy&keyword=edredom%20de%20malha%20queen.html";
           
            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Headers["authority"] = "shopee.com.br";
            httpRequest.Headers["accept-language"] = "pt-BR;q=0.9";
            httpRequest.Headers["sec-ch-ua-platform"] = "\"Windows\"";
            httpRequest.Headers["x-api-source"] = "pc";
            httpRequest.Headers["x-requested-with"] = "XMLHttpRequest";
            httpRequest.Headers["x-shopee-language"] = "br";

            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                JsonDocument doc = JsonDocument.Parse(result);
                JsonElement root = doc.RootElement;

                int resultCount = root.GetProperty("total_count").GetInt32();
                if (resultCount == 0)
                {
                    Console.WriteLine("No item found !");
                }
                else
                {                    
                    List<Tabela> lstTabela = new List<Tabela>();
                    int foundLength = root.GetProperty("items").GetArrayLength();
                    for (int i = 0; i < foundLength; i++)
                    {
                        JsonElement item = root.GetProperty("items").EnumerateArray().ElementAt(i);
                        var nome = item.GetProperty("item_basic").GetProperty("name").ToString();
                        var preco = item.GetProperty("item_basic").GetProperty("price").ToString();
                        preco = "R$" + preco.Remove(preco.Length - 3);
                        preco = preco.Insert(preco.Length - 2, ",");
                        var fabricante = item.GetProperty("item_basic").GetProperty("brand").ToString();                        
                        var vendedor = item.GetProperty("item_basic").GetProperty("brand").ToString();
                        var modelo = "";

                        lstTabela.Add(new Tabela { Nome = nome, Preco = preco, Fabricante = fabricante, Modelo = modelo, Vendedor = vendedor });
                    }
                    dataGridView1.DataSource = lstTabela;
                }
            }
        }
    }
}