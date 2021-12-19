using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace C__Web_Scraping_Amazon
{
    class Program
    {
        static void getData(IWebDriver driver)
        {
            int count=0;
            // Creating CSV Contents
            var csv = new StringBuilder();
            var newLine = "Product Name,Product Rating,Product Price";
            csv.AppendLine(newLine);

            var xpath = string.Format("//div[@class='sg-col sg-col-4-of-12 sg-col-8-of-16 sg-col-12-of-20']");
            ReadOnlyCollection<IWebElement> items = driver.FindElements(By.XPath(xpath));
            
            foreach(IWebElement item in items)
            {
                string product_title,product_rating,product_price;
               
                IWebElement title_element = item.FindElement(By.XPath("./div/div/div/h2"));
                product_title = title_element.Text.Replace(',',' ');
                IWebElement rating_element = item.FindElement(By.XPath("./div/div/div[2]/div/span"));
                product_rating = rating_element.GetAttribute("aria-label").ToString();
                IWebElement price_element = item.FindElement(By.XPath("./div/div/div[3]"));
                product_price = price_element.Text.Split("\n")[0].Replace(',',' ').Replace('₹',' ');
               
                // Writing to CSV file
                newLine = string.Format("{0},{1},{2}",product_title,product_rating,product_price);
                csv.AppendLine(newLine); 
                count++;
                if(count==10)
                {
                    break;
                }
            }
            File.WriteAllText(@"C:\Users\Anmol\OneDrive\Desktop\amazonData.csv", csv.ToString()); 
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the Search Term:");
            string searchTerm = Console.ReadLine();
            IWebDriver  driver = new ChromeDriver(@"C:\Users\Anmol\Downloads\");
            driver.Manage().Window.Maximize();
            driver.Navigate().GoToUrl("https://www.amazon.in");
            Thread.Sleep(2000);
            IWebElement passwordTextBox = driver.FindElement(By.Name("field-keywords"));
            passwordTextBox.SendKeys(searchTerm+Keys.Return);
            driver.FindElement(By.Id("a-autoid-0")).Click();
            driver.FindElement(By.Id("s-result-sort-select_3")).Click();
            Thread.Sleep(2000);
            getData(driver);
            driver.Quit();
            Console.WriteLine("AMAZON SCRAPPING COMPLETE");
        }
    }
}
