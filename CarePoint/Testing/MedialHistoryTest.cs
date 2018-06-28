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
    public class MedicalHistoryTest
    {
        private static IWebDriver driver;
        private StringBuilder verificationErrors;
        private static string baseURL;
        private bool acceptNextAlert = true;
        public static ExtentReports report;
        public static ExtentTest addPrescriptionTest;
        private static ExtentHtmlReporter htmlReporter;
        private static String filePath = "../../Reports/MedicalHistory Testing Report.html";

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
            addPrescriptionTest = report.CreateTest("AddPrescription");
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
            "AddPrescription TC$",
            DataAccessMethod.Sequential),
            TestMethod]
        public void TheAddPrescriptionTest()
        {
            ExtentTest child = addPrescriptionTest.CreateNode(TestContext.DataRow["Key"].ToString());
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
            Thread.Sleep(500);
            driver.FindElement(By.Id("ilink-patient-list")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.LinkText("Females")).Click();
            Thread.Sleep(500);
            driver.FindElement(By.XPath("//div[@id='itab-females']/div[3]/div[3]/a/button")).Click();
            driver.FindElement(By.XPath("//button[@id='ibtn-add-prescription']/span")).Click();
            Thread.Sleep(1000);
            if (Int32.Parse(TestContext.DataRow["NumberOfSymptoms"].ToString()) > 0)
            {
                driver.FindElement(By.Name("symptomName")).Click();
                driver.FindElement(By.Name("symptomName")).Clear();
                driver.FindElement(By.Name("symptomName")).SendKeys(TestContext.DataRow["Symptom1"].ToString());
                if (Int32.Parse(TestContext.DataRow["NumberOfSymptoms"].ToString()) > 1)
                {
                    driver.FindElement(By.XPath("//div[@id='idiv-step-1']/div/div[2]/button[2]")).Click();
                    Thread.Sleep(500);

                    driver.FindElement(By.XPath("(//input[@name='symptomName'])[2]")).Click();
                    driver.FindElement(By.XPath("(//input[@name='symptomName'])[2]")).Clear();
                    driver.FindElement(By.XPath("(//input[@name='symptomName'])[2]")).SendKeys(TestContext.DataRow["Symptom2"].ToString());
                }
            }
            driver.FindElement(By.Id("ibtn-nxt")).Click();
            Thread.Sleep(1000);

            if (Int32.Parse(TestContext.DataRow["NumberOfDiseases"].ToString()) > 0)
            {
                driver.FindElement(By.Name("diseaseName")).Click();
                driver.FindElement(By.Name("diseaseName")).Clear();
                driver.FindElement(By.Name("diseaseName")).SendKeys(TestContext.DataRow["Disease1"].ToString());
                if (Boolean.Parse(TestContext.DataRow["IsGenetic1"].ToString()))
                {
                    driver.FindElement(By.XPath("//div[@id='idiv-step-2']/div/span/label")).Click();
                }
                if (Int32.Parse(TestContext.DataRow["NumberOfDiseases"].ToString()) > 1)
                {
                    driver.FindElement(By.XPath("(//button[@type='button'])[8]")).Click();
                    Thread.Sleep(500);

                    driver.FindElement(By.XPath("(//input[@name='diseaseName'])[2]")).Click();
                    driver.FindElement(By.XPath("(//input[@name='diseaseName'])[2]")).Clear();
                    driver.FindElement(By.XPath("(//input[@name='diseaseName'])[2]")).SendKeys(TestContext.DataRow["Disease2"].ToString());
                    if (Boolean.Parse(TestContext.DataRow["IsGenetic2"].ToString()))
                    {
                        driver.FindElement(By.XPath("//div[@id='idiv-step-2']/div[2]/span/label")).Click();
                    }
                }
            }
            driver.FindElement(By.Id("ibtn-nxt")).Click();
            Thread.Sleep(1000);

            if (Int32.Parse(TestContext.DataRow["NumberOfMedicines"].ToString()) > 0)
            {
                driver.FindElement(By.Name("drugName")).Click();
                driver.FindElement(By.Name("drugName")).Clear();
                driver.FindElement(By.Name("drugName")).SendKeys(TestContext.DataRow["Medicine1"].ToString());
                driver.FindElement(By.Name("dose")).Click();
                driver.FindElement(By.Name("dose")).Clear();
                driver.FindElement(By.Name("dose")).SendKeys(TestContext.DataRow["Dose1"].ToString());
                if (Int32.Parse(TestContext.DataRow["NumberOfMedicines"].ToString()) > 1)
                {
                    driver.FindElement(By.XPath("//div[@id='idiv-step-3']/div/div[3]/button[2]/span")).Click();
                    Thread.Sleep(500);

                    driver.FindElement(By.XPath("(//input[@name='drugName'])[2]")).Click();
                    driver.FindElement(By.XPath("(//input[@name='drugName'])[2]")).Clear();
                    driver.FindElement(By.XPath("(//input[@name='drugName'])[2]")).SendKeys(TestContext.DataRow["Medicine2"].ToString());
                    driver.FindElement(By.XPath("(//input[@name='dose'])[2]")).Click();
                    driver.FindElement(By.XPath("(//input[@name='dose'])[2]")).Clear();
                    driver.FindElement(By.XPath("(//input[@name='dose'])[2]")).SendKeys(TestContext.DataRow["Dose2"].ToString());
                }
            }
            driver.FindElement(By.Id("ibtn-nxt")).Click();
            Thread.Sleep(1000);

            try
            {
                if(!String.IsNullOrEmpty(TestContext.DataRow["Warning"].ToString()))
                    Assert.AreEqual(TestContext.DataRow["Warning"].ToString().Trim(), driver.FindElement(By.Id("idiv-warning")).Text);
                child.Log(Status.Pass, "Test Passed");

            }
            catch (Exception e)
            {
                verificationErrors.Append(e.Message);
                child.Log(Status.Fail, "Test Failed");

            }
            driver.FindElement(By.Id("itxtarea-remarks")).Click();
            driver.FindElement(By.Id("itxtarea-remarks")).Clear();
            driver.FindElement(By.Id("itxtarea-remarks")).SendKeys(TestContext.DataRow["Remarks"].ToString());
            driver.FindElement(By.Id("ibtn-submit")).Click();
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
