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
using OpenQA.Selenium.Chrome;

namespace SeleniumTests
{
    [TestClass]
    public class ServiceTests
    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private static string baseURL;
        private bool acceptNextAlert = true;
        public static ExtentReports report;
        public static ExtentTest addService;
        private static ExtentHtmlReporter htmlReporter;
        private static String filePath = "../../Reports/Services Testing Report.html";

        public TestContext TestContext { get; set; }
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            htmlReporter = new ExtentHtmlReporter(filePath);
            htmlReporter.Configuration().ChartVisibilityOnOpen = true;
            htmlReporter.Configuration().DocumentTitle = "Carepoint Test Report";
            htmlReporter.Configuration().ReportName = "Medical History Tests";
            report = new ExtentReports();
            report.AttachReporter(htmlReporter);
            addService = report.CreateTest("Add Service");
            driver = new ChromeDriver();
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
            "Add Service TC$",
            DataAccessMethod.Sequential),
            TestMethod]
        public void TheAddServiceTest()
        {
            ExtentTest child = addService.CreateNode(TestContext.DataRow["Key"].ToString());
            driver.Navigate().GoToUrl("http://carepoint.com:3000/");
            driver.FindElement(By.Id("itext-mail-phone")).Click();
            driver.FindElement(By.Id("itext-mail-phone")).Clear();
            driver.FindElement(By.Id("itext-mail-phone")).SendKeys("doctor@carepoint.com");
            driver.FindElement(By.Id("ipasswd")).Click();
            driver.FindElement(By.Id("ipasswd")).Clear();
            driver.FindElement(By.Id("ipasswd")).SendKeys("123456");
            driver.FindElement(By.Id("ifrm-login")).Submit();
            Thread.Sleep(5000);
            driver.FindElement(By.XPath("//form[@id='logoutForm']/ul/li/a/span")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("ilink-change-place")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.XPath("//button[@onclick=\"switchWorkPlace('6','Hospital','/MedicalPlace/ProfilePage?id=6')\"]")).Click();
            driver.FindElement(By.XPath("//div[2]/button")).Click();
            Thread.Sleep(1000);
            driver.FindElement(By.Id("iinp-service-name-new")).Click();
            driver.FindElement(By.Id("iinp-service-name-new")).Clear();
            driver.FindElement(By.Id("iinp-service-name-new")).SendKeys(TestContext.DataRow["Name"].ToString());
            (new SelectElement(driver.FindElement(By.Id("iinp-service-type-new")))).SelectByValue(TestContext.DataRow["CategoryId"].ToString());
            driver.FindElement(By.Id("iinp-service-cost-new")).Click();
            driver.FindElement(By.Id("iinp-service-cost-new")).Clear();
            driver.FindElement(By.Id("iinp-service-cost-new")).SendKeys(TestContext.DataRow["Cost"].ToString());
            driver.FindElement(By.Id("itextarea-service-desc-new")).Click();
            driver.FindElement(By.Id("itextarea-service-desc-new")).Clear();
            driver.FindElement(By.Id("itextarea-service-desc-new")).SendKeys(TestContext.DataRow["Description"].ToString());
            driver.FindElement(By.XPath("//input[@value='Done']")).Click();
            try
            {
                if(!String.IsNullOrEmpty(TestContext.DataRow["NameError"].ToString()))
                    Assert.AreEqual(TestContext.DataRow["NameError"].ToString(), driver.FindElement(By.Id("iinp-service-name-new-error")).Text);
                if(!String.IsNullOrEmpty(TestContext.DataRow["CostError"].ToString()))
                    Assert.AreEqual(TestContext.DataRow["CostError"].ToString(), driver.FindElement(By.Id("iinp-service-cost-new-error")).Text);
                if(!String.IsNullOrEmpty(TestContext.DataRow["CategoryIdError"].ToString()))
                    Assert.AreEqual(TestContext.DataRow["CategoryIdError"].ToString(), driver.FindElement(By.Id("iinp-service-type-new-error")).Text);
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
