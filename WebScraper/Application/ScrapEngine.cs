﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Text;
using WebScraper.Common.Model;

namespace WebScraper.Application
{
    public class ScrapEngine
    {
        private IWebDriver _driver;

        private void Open(string url)
        {
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments("headless");

            _driver = new ChromeDriver(chromeOptions);

            _driver.Navigate().GoToUrl(url);
            _driver.Navigate().Refresh();
        }

        private void Close()
        {
            _driver.Close();
        }

        public List<Produto> Coletar(string url)
        {
            var produtos = new List<Produto>();

            Open(url);

            for (int x = 1; x <= 15; x++)
            {
                var produto = new Produto();
                IWebElement divProduto;

                try
                {
                    divProduto = _driver.FindElement(By.XPath($"/html/body/div[6]/div/div[4]/div[3]/div/div[3]/div[1]/g-scrolling-carousel/div[1]/div/div/div[{x}]"));
                }
                catch (NoSuchElementException)
                {
                    break;
                }

                try
                {
                    //com promo
                    produto.Preco = divProduto.FindElement(By.XPath("a/div[3]/div/div[2]/span/b")).Text;
                    produto.Texto = divProduto.FindElement(By.XPath("a/div[3]/div/div[1]")).Text;
                    produto.Site = divProduto.FindElement(By.XPath("a/div[3]/div/div[3]/span")).Text;
                    produto.Link = divProduto.FindElement(By.XPath("a")).GetAttribute("href");
                }
                catch (NoSuchElementException)
                {
                    //sem promo
                    produto.Preco = divProduto.FindElement(By.XPath("a/div[2]/div/div[2]/span/b")).Text;
                    produto.Texto = divProduto.FindElement(By.XPath("a/div[2]/div/div[1]")).Text;
                    produto.Site = divProduto.FindElement(By.XPath("a/div[2]/div/div[3]/span")).Text;
                    produto.Link = divProduto.FindElement(By.XPath("a")).GetAttribute("href");
                }
                produto.DataPesquisa = DateTime.Now;

                produtos.Add(produto);
            }

            Close();

            return produtos;
        }

    }
}
