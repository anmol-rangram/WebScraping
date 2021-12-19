using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
namespace selenium
{
class Program
{

static void getData(IWebDriver driver)
{
    // Creating CSV
    var csv = new StringBuilder();
    var newLine = "Title Of Video,Upload Time,Channel Name,Views";
    csv.AppendLine(newLine);

    // Finding Views, Title, Upload Time and Channel Name
    int count=0;
    By elem_video_link = By.CssSelector("ytd-video-renderer.style-scope.ytd-item-section-renderer");
    ReadOnlyCollection<IWebElement> videos = driver.FindElements(elem_video_link);
    
    foreach (IWebElement video in videos)
    {
	    string str_title, str_views, str_rel, str_uploader;
 
        IWebElement elem_video_views = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[1]"));
	    str_views = elem_video_views.Text.Replace(',',' ');
 
        IWebElement elem_video_reldate = video.FindElement(By.XPath(".//*[@id='metadata-line']/span[2]"));
        str_rel = elem_video_reldate.Text.Replace(',',' ');
 
        IWebElement elem_video_title = video.FindElement(By.CssSelector("#video-title"));
        str_title = elem_video_title.Text.Replace(',',' ');

        IWebElement elem_video_uploader = video.FindElement(By.Id("channel-info"));
        str_uploader = elem_video_uploader.Text.Replace(',',' ');
    
        // Writing to CSV file
        newLine = string.Format("{0},{1},{2},{3}",str_title,str_rel,str_uploader,str_views);
        csv.AppendLine(newLine); 
        count++;
        if(count==5)
        {
            break;
        }
    }
    File.WriteAllText(@"C:\Users\Anmol\OneDrive\Desktop\youtubeData.csv", csv.ToString());
}
static void Main(string[] args)
{
Console.WriteLine("Enter the Search Term:");
string searchTerm = Console.ReadLine();
IWebDriver  driver = new ChromeDriver(@"C:\Users\Anmol\Downloads\"); 
driver.Navigate().GoToUrl("https://www.youtube.com");
Thread.Sleep(2000);
driver.FindElement(By.XPath("//*[@id='content']/div[2]/div[5]/div[2]/ytd-button-renderer[2] ")).Click();
Thread.Sleep(2000);
IWebElement passwordTextBox = driver.FindElement(By.Name("search_query"));
passwordTextBox.SendKeys(searchTerm);
driver.FindElement(By.Id("search-icon-legacy")).Click();
Thread.Sleep(5000);
IJavaScriptExecutor jse = (IJavaScriptExecutor) driver; 
jse.ExecuteScript("document.getElementsByClassName('tp-yt-paper-button')[0].click()");
Thread.Sleep(2000);
driver.FindElement(By.XPath("/html/body/ytd-app/div/ytd-page-manager/ytd-search/div[1]/ytd-two-column-search-results-renderer/div/ytd-section-list-renderer/div[1]/div[2]/ytd-search-sub-menu-renderer/div[1]/iron-collapse/div/ytd-search-filter-group-renderer[5]/ytd-search-filter-renderer[2]/a")).Click();
jse.ExecuteScript("document.getElementsByClassName('yt-simple-endpoint style-scope ytd-search-filter-renderer')[23].click()");
Thread.Sleep(2000);
getData(driver);
Console.WriteLine("Youtube Scraping Complete!!!");
driver.Quit();
}
}
}
