using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;

namespace C__Web_Scraping_Indeed
{
    class Program
    {
        static void getData(IWebDriver driver,int pgno,StringBuilder csv)
        {
            string newLine;
            IJavaScriptExecutor jse = (IJavaScriptExecutor) driver;
            ReadOnlyCollection<IWebElement> posts = driver.FindElements(By.CssSelector("a[data-hiring-event='false']"));
            foreach (IWebElement post in posts)
            {
	            string str_title, str_company, str_link, str_location, str_date;
 
                IWebElement elem_post_title = post.FindElement(By.XPath("./div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[1]/h2/span"));
	            str_title = string.Format(@"{0}",elem_post_title.Text);

                IWebElement elem_post_company = post.FindElement(By.XPath("./div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[2]/pre/span"));
	            str_company = string.Format(@"{0}",elem_post_company.Text);

                IWebElement elem_post_location = post.FindElement(By.XPath("./div[1]/div/div[1]/div/table[1]/tbody/tr/td/div[2]/pre/div"));
	            str_location = string.Format(@"{0}",elem_post_location.Text).Replace('•',' ').Replace("\n","").Replace("\r","");

                str_link = post.GetAttribute("href").ToString();             
                str_date = post.FindElement(By.ClassName("date")).Text.Split("\n")[1];
                if(str_date=="Just posted" || str_date=="Today" || str_date=="1 day ago" || str_date=="2 days ago" || str_date=="3 days ago" || str_date=="Active 3 days ago")
                {
                     // Writing to CSV file
                    newLine = string.Format("{0},{1},{2},{3}",str_title,str_company,str_location,str_link);
                    csv.AppendLine(newLine);
                    continue;
                }
                else
                {
                    File.WriteAllText(@"C:\Users\Anmol\OneDrive\Desktop\indeedData.csv", csv.ToString());
                    return;
                }
                

            }
            var xpath = string.Format("//*[@id='resultsCol']/nav/div/ul/li[{0}]/a",pgno);
            var link = driver.FindElement(By.XPath(xpath)).GetAttribute("href");
            driver.Navigate().GoToUrl(link);
            if(pgno==2)
            {
                pgno+=2;
            }
            else
            {
                pgno++;
            }
            getData(driver,pgno,csv);

        }
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the Job Search Term:");
            string searchTerm = Console.ReadLine();
            IWebDriver  driver = new ChromeDriver(@"C:\Users\Anmol\Downloads\");
            driver.Navigate().GoToUrl("https://be.indeed.com/");
            Thread.Sleep(2000);
            IWebElement searchTextBox = driver.FindElement(By.Name("q"));
            searchTextBox.SendKeys(searchTerm+Keys.Return);
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//*[@id='resultsCol']/div[3]/div[4]/div[1]/span[2]/a")).Click();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//*[@id='popover-x']/button")).Click();
            int ind=2;
            // Creating CSV Contents
            var csv = new StringBuilder();
            var newLine = "Title,Company,Location,Link";
            csv.AppendLine(newLine);
            getData(driver,ind,csv);
            Console.WriteLine("INDEED SCRAPPING COMPLETE!!!");

            driver.Quit();
        }
    }
}
