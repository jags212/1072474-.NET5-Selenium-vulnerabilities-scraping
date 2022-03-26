using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VulnerabilitiesScraping;

namespace VulnerabilitiesScrapingTests.TestSetUp
{
    [TestClass]
    public class IntegrationTestFixtures
    {
        int versionSeed = 0;
        int rowSeed = 0;
        string vulnPageUrl = "";
        ChromeDriver driver;
        int wpVersion = 0;
        [TestInitialize]
        public void OnTestInitialize()
        {
            _systemUnderTest = null;

            versionSeed = new Random().Next(5) + 5;
            rowSeed = new Random().Next(5) + 5;
            driver = SetUpDriver();
            wpVersion = versionSeed;
            vulnPageUrl =
                SystemUnderTest.GetServerAddressForRelativeUrl("Home/Index?versionSeed=" + versionSeed + "&rowSeed=" + rowSeed);

        }

        [TestCleanup]
        public void OnTestCleanup()
        {
            _systemUnderTest?.Dispose();
        }

        private CustomWebApplicationFactory<Startup> _systemUnderTest;
        public CustomWebApplicationFactory<Startup> SystemUnderTest
        {
            get
            {
                if (_systemUnderTest == null)
                {
                    _systemUnderTest = new CustomWebApplicationFactory<Startup>();
                }

                return _systemUnderTest;
            }
        }

        private ChromeDriver SetUpDriver()
        {
            var driverOptions = new ChromeOptions();

            driverOptions.AddArgument("headless");

            string driverPath = Environment.CurrentDirectory;
            var driver = new ChromeDriver(driverPath.Split("bin")[0].ToString(), driverOptions);
            return driver;
        }

        [TestMethod]
        public void TestScrapVulnerabilitiesOf()
        {
            List<string> actual = VulnerabilitiesImplementation.scrapVulnerabilitiesOf(driver, vulnPageUrl, "" + wpVersion);
            List<string> expected = new List<string>();
            for (int j = 1; j <= rowSeed; j++)
            {
                expected.Add(versionSeed + "" + j);
            }
            Console.WriteLine(" Actual: \n" + actual + "\n Expected: \n" + expected);
            Assert.AreEqual(expected.Count(), actual.Count());
            foreach (var actualCveId in actual)
            {
                Assert.IsTrue(expected.Contains(actualCveId));
            }
        }

        [TestMethod]
        public void TestFindHighestScoredVulnerability()
        {
            string actual = VulnerabilitiesImplementation.findHighestScoredVulnerability(driver, vulnPageUrl, "" + versionSeed);
            string expected = wpVersion + "" + rowSeed;
            Console.WriteLine(" Actual: \n" + actual + "\n Expected: \n" + expected);
            Assert.AreEqual(expected, actual);
        }
    }
}
