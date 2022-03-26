using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Collections.Generic;

namespace VulnerabilitiesScrapingTests
{
    public class VulnerabilitiesImplementation
    {
        public static List<string> scrapVulnerabilitiesOf(IWebDriver driver, string vulnPageUrl, string wpVersion)
        {
            driver.Navigate().GoToUrl(vulnPageUrl);
            List<string> vulnList = new List<string>();

            foreach (IWebElement row in driver.FindElements(By.XPath("//table//tr[position()>1]")))
            {
                var cols = row.FindElements(By.TagName("td"));
                if (wpVersion == cols[4].Text)
                {
                    vulnList.Add(cols[0].Text);
                }
            }
            return vulnList;
        }

        public static string findHighestScoredVulnerability(IWebDriver driver, string vulnPageUrl, string wpVersion)
        {
            driver.Navigate().GoToUrl(vulnPageUrl);

            string highestScoredCVEID = "";
            string highestScore = "";

            foreach (IWebElement row in driver.FindElements(By.XPath("//table//tr[position()>1]")))
            {
                var cols = row.FindElements(By.TagName("td"));

                if (wpVersion == cols[4].Text && highestScore.CompareTo(cols[3].Text) < 0)
                {
                    highestScore = cols[3].Text;
                    highestScoredCVEID = cols[0].Text;
                }
            }
            return highestScoredCVEID;
        }
    }
}
