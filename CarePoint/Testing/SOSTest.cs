using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace SeleniumTests
{
    [TestClass]
    public class SOSTest
    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private static string baseURL;
        private bool acceptNextAlert = true;
        public static ExtentReports report;
        public static ExtentTest sosTest;
        private static ExtentHtmlReporter htmlReporter;
        private static String filePath = "../../Reports/SOS Testing Report.html";

        public TestContext TestContext { get; set; }
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            htmlReporter = new ExtentHtmlReporter(filePath);
            htmlReporter.Configuration().ChartVisibilityOnOpen = true;
            htmlReporter.Configuration().DocumentTitle = "Carepoint Test Report";
            htmlReporter.Configuration().ReportName = "SOS Tests";
            report = new ExtentReports();
            report.AttachReporter(htmlReporter);
            sosTest = report.CreateTest("SOS");
            driver = new FirefoxDriver();
            baseURL = "https://www.katalon.com/";
        }

        [ClassCleanup]
        public static void CleanupClass()
        {
            try
            {
                //driver.Quit();// quit does not close the window
                driver.Close();
                driver.Dispose();
                report.Flush();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        [TestInitialize]
        public void InitializeTest()
        {
            verificationErrors = new StringBuilder();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            Assert.AreEqual("", verificationErrors.ToString());
        }

        [DataSource("System.Data.Odbc",
            "Dsn=Excel Files;dbq=|DataDirectory|\\Testing.xlsx;driverid=1046;maxbuffersize=2048;pagetimeout=5",
            "SOS TC$",
            DataAccessMethod.Sequential),
            TestMethod]
        public void TheSOSTest()
        {
            ExtentTest child = sosTest.CreateNode(TestContext.DataRow["Key"].ToString());
            driver.Navigate().GoToUrl("http://carepoint.com:3000/");
            driver.FindElement(By.Id("itext-mail-phone")).Click();
            driver.FindElement(By.Id("itext-mail-phone")).Clear();
            driver.FindElement(By.Id("itext-mail-phone")).SendKeys("citizen@carepoint.com");
            driver.FindElement(By.Id("ipasswd")).Click();
            driver.FindElement(By.Id("ipasswd")).Clear();
            driver.FindElement(By.Id("ipasswd")).SendKeys("123456");
            driver.FindElement(By.Id("ifrm-login")).Submit();
            Thread.Sleep(5000);
            driver.FindElement(By.Id("ibtn-sos-pop")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//div[@id='idiv-sos-pop']/form/textarea")).Click();
            driver.FindElement(By.XPath("//div[@id='idiv-sos-pop']/form/textarea")).Clear();
            driver.FindElement(By.XPath("//div[@id='idiv-sos-pop']/form/textarea")).SendKeys(TestContext.DataRow["Description"].ToString());
            if (!Boolean.Parse(TestContext.DataRow["IsHospital"].ToString()))
            {
                driver.FindElement(By.XPath("//div[@id='idiv-sos-pop']/form/span[2]/label")).Click();
            }
            if (Boolean.Parse(TestContext.DataRow["IsFamily"].ToString()))
            {
                driver.FindElement(By.XPath("//div[@id='idiv-sos-pop']/form/span[3]/label")).Click();
            }
            if (Boolean.Parse(TestContext.DataRow["IsFriends"].ToString()))
            {
                driver.FindElement(By.XPath("//div[@id='idiv-sos-pop']/form/span[4]/label")).Click();
            }
            driver.FindElement(By.Id("iisend-sos")).Click();
            Thread.Sleep(1000);
            try
            {
                if(!String.IsNullOrEmpty(TestContext.DataRow["Description Error"].ToString()))
                    Assert.AreEqual(TestContext.DataRow["Description Error"].ToString(), driver.FindElement(By.XPath("//div[@id='idiv-sos-pop']/form/span")).Text);
                if (!String.IsNullOrEmpty(TestContext.DataRow["Options Error"].ToString()))
                    Assert.AreEqual(TestContext.DataRow["Options Error"].ToString(), driver.FindElement(By.XPath("//div[@id='idiv-sos-pop']/form/span[5]")).Text);
                child.Log(Status.Pass, "Test Passed");

            }

            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
                child.Log(Status.Fail, e.Message);
            }
        }
        private bool IsElementPresent(By by)
        {
            try
            {
                driver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }

        private bool IsAlertPresent()
        {
            try
            {
                driver.SwitchTo().Alert();
                return true;
            }
            catch (NoAlertPresentException)
            {
                return false;
            }
        }

        private string CloseAlertAndGetItsText()
        {
            try
            {
                IAlert alert = driver.SwitchTo().Alert();
                string alertText = alert.Text;
                if (acceptNextAlert)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }
                return alertText;
            }
            finally
            {
                acceptNextAlert = true;
            }
        }
    }
}
