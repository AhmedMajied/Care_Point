using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;

namespace SeleniumTests
{
    [TestClass]
    public class AccountTests

    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private static string baseURL;
        private bool acceptNextAlert = true;
        public static ExtentReports report;
        public static ExtentTest signUpTest;
        private static ExtentHtmlReporter htmlReporter;
        private static String filePath = "../../Reports/SignUp Testing Report.html";

        public TestContext TestContext { get; set; }
        [ClassInitialize]
        public static void InitializeClass(TestContext testContext)
        {
            htmlReporter = new ExtentHtmlReporter(filePath);
            htmlReporter.Configuration().ChartVisibilityOnOpen = true;
            htmlReporter.Configuration().DocumentTitle = "Carepoint Test Report";
            htmlReporter.Configuration().ReportName = "Account Tests";
            report = new ExtentReports();
            report.AttachReporter(htmlReporter);
            signUpTest = report.CreateTest("SignUp");
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
            "SignUp TC$",
            DataAccessMethod.Sequential),
            TestMethod]
        public void TheSignUpTest()
        {
            ExtentTest child = signUpTest.CreateNode(TestContext.DataRow["Key"].ToString());
            driver.Navigate().GoToUrl("http://carepoint.com:3000/Account/Register");
            driver.FindElement(By.Id("itext-first-name")).Click();
            driver.FindElement(By.Id("itext-first-name")).Clear();
            driver.FindElement(By.Id("itext-first-name")).SendKeys(TestContext.DataRow["First Name"].ToString());
            driver.FindElement(By.Id("itext-middle-name")).Clear();
            driver.FindElement(By.Id("itext-middle-name")).SendKeys(TestContext.DataRow["Middle Name"].ToString());
            driver.FindElement(By.Id("itext-last-name")).Clear();
            driver.FindElement(By.Id("itext-last-name")).SendKeys(TestContext.DataRow["Last Name"].ToString());
            driver.FindElement(By.Id("itext-national-id")).Clear();
            driver.FindElement(By.Id("itext-national-id")).SendKeys(TestContext.DataRow["NationalId Number"].ToString());
            (new SelectElement(driver.FindElement(By.Id("iselect-blood-type")))).SelectByValue(TestContext.DataRow["BloodType"].ToString());
            (new SelectElement(driver.FindElement(By.Id("iselect-day")))).SelectByValue(TestContext.DataRow["Day"].ToString());
            (new SelectElement(driver.FindElement(By.Id("iselect-month")))).SelectByValue(TestContext.DataRow["Month"].ToString());
            (new SelectElement(driver.FindElement(By.Id("iselect-year")))).SelectByValue(TestContext.DataRow["Year"].ToString());
            driver.FindElement(By.Id("itext-phone")).Click();
            driver.FindElement(By.Id("itext-phone")).Clear();
            driver.FindElement(By.Id("itext-phone")).SendKeys(TestContext.DataRow["Phone Number"].ToString());
            driver.FindElement(By.Id("itext-mail")).Clear();
            driver.FindElement(By.Id("itext-mail")).SendKeys(TestContext.DataRow["Email"].ToString());
            driver.FindElement(By.Id("itext-passwd")).Clear();
            driver.FindElement(By.Id("itext-passwd")).SendKeys(TestContext.DataRow["Password"].ToString());
            driver.FindElement(By.Id("itext-passwd-confirm")).Clear();
            driver.FindElement(By.Id("itext-passwd-confirm")).SendKeys(TestContext.DataRow["Confirm Password"].ToString());
            if (!String.IsNullOrEmpty(TestContext.DataRow["NationalId Photo"].ToString()))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("$('#ifile-national-id').removeClass('hidden')");
                Thread.Sleep(500);
                driver.FindElement(By.Id("ifile-national-id")).SendKeys(TestContext.DataRow["NationalId Photo"].ToString());

            }
            (new SelectElement(driver.FindElement(By.Id("iselect-speciality")))).SelectByValue(TestContext.DataRow["Speciality"].ToString());
            driver.FindElement(By.Id("iselect-speciality")).Click();
            if (TestContext.DataRow["Speciality"].ToString() != "0" && !String.IsNullOrEmpty(TestContext.DataRow["Profession License"].ToString()))
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("$('#ifile-license').removeClass('hidden');");
                Thread.Sleep(500);
                driver.FindElement(By.Id("ifile-license")).SendKeys(TestContext.DataRow["Profession License"].ToString());
            }
            driver.FindElement(By.Id("ibtn-sign-up")).Click();
            try
            {
                if (IsElementPresent(By.Id("itext-first-name-error")))
                    Assert.AreEqual(TestContext.DataRow["First Name Error"].ToString().Trim(), driver.FindElement(By.Id("itext-first-name-error")).Text);
                if (IsElementPresent(By.Id("itext-middle-name-error")))
                    Assert.AreEqual(TestContext.DataRow["Middle Name Error"].ToString().Trim(), driver.FindElement(By.Id("itext-middle-name-error")).Text);
                if (IsElementPresent(By.Id("itext-last-name-error")))
                    Assert.AreEqual(TestContext.DataRow["Last Name Error"].ToString().Trim(), driver.FindElement(By.Id("itext-last-name-error")).Text);
                if (IsElementPresent(By.Id("itext-national-id-erro")))
                    Assert.AreEqual(TestContext.DataRow["NationalId Number Error"].ToString().Trim(), driver.FindElement(By.Id("itext-national-id-error")).Text);
                if (IsElementPresent(By.Id("iselect-blood-type-error")))
                    Assert.AreEqual(TestContext.DataRow["BloodType Error"].ToString().Trim(), driver.FindElement(By.Id("iselect-blood-type-error")).Text);
                if (IsElementPresent(By.Id("iselect-day-error")))
                    Assert.AreEqual(TestContext.DataRow["DateOfBirth Error"].ToString().Trim(), driver.FindElement(By.Id("iselect-day-error")).Text);
                if (IsElementPresent(By.Id("itext-phone-error")))
                    Assert.AreEqual(TestContext.DataRow["Phone Number Error"].ToString().Trim(), driver.FindElement(By.Id("itext-phone-error")).Text);
                if (IsElementPresent(By.Id("itext-mail-error")))
                    Assert.AreEqual(TestContext.DataRow["Email Error"].ToString().Trim(), driver.FindElement(By.Id("itext-mail-error")).Text);
                if (IsElementPresent(By.Id("itext-passwd-error")))
                    Assert.AreEqual(TestContext.DataRow["Password Error"].ToString().Trim(), driver.FindElement(By.Id("itext-passwd-error")).Text);
                if (IsElementPresent(By.Id("itext-passwd-confirm-error")))
                    Assert.AreEqual(TestContext.DataRow["Confirm Password Error"].ToString().Trim(), driver.FindElement(By.Id("itext-passwd-confirm-error")).Text);
                if (IsElementPresent(By.Id("ifile-national-id-error")))
                    Assert.AreEqual(TestContext.DataRow["NationalId Photo Error"].ToString().Trim(), driver.FindElement(By.Id("ifile-national-id-error")).Text);
                if (IsElementPresent(By.Id("iselect-speciality-error")))
                    Assert.AreEqual(TestContext.DataRow["Speciality Error"].ToString().Trim(), driver.FindElement(By.Id("iselect-speciality-error")).Text);
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
