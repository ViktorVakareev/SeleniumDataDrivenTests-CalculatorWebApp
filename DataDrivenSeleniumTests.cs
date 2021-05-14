using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;
using System.Threading;

namespace SeleniumDataDrivenTests_CalculatorWebApp
{
    [TestFixture]
    public class DataDrivenSeleniumTests
    {
        RemoteWebDriver driver;
        IWebElement firtNumTextBox;
        IWebElement secondNumTextBox;
        IWebElement operationDropDown;
        IWebElement calculateButton;
        IWebElement resetButton;
        IWebElement resultField;

        [OneTimeSetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Url = "https://number-calculator.nakov.repl.co/";
            firtNumTextBox = driver.FindElement(By.Id("number1"));
            secondNumTextBox = driver.FindElement(By.Id("number2"));
            operationDropDown = driver.FindElement(By.Id("operation"));
            calculateButton = driver.FindElement(By.Id("calcButton"));
            resetButton = driver.FindElement(By.Id("resetButton"));
            resultField = driver.FindElement(By.Id("result"));            
        }

        // Valid integer cases
        [TestCase("88", "+", "13", "Result: 101")]
        [TestCase("88", "-", "13", "Result: 75")]
        [TestCase("88", "*", "13", "Result: 1144")]
        [TestCase("88", "/", "44", "Result: 2")]

        // Valid floating point cases
        [TestCase("88.88", "+", "13.12", "Result: 102")]
        [TestCase("88.88", "-", "13.12", "Result: 75.76")]
        [TestCase("88.88", "*", "13.12", "Result: 1166.1056")]
        [TestCase("88.88", "/", "44.44", "Result: 2")]

        // Invalid cases
        [TestCase("88.88", "", "13.12", "Result: invalid operation")]
        [TestCase("88.88", "#", "13.12", "Result: invalid operation")]
        [TestCase("88.88", "aa", "13.12", "Result: invalid operation")]
        [TestCase("88.88", "12", "44.44", "Result: invalid operation")]
        [TestCase("aaa", "=", "ttt", "Result: invalid input")]
        [TestCase("333", "+", "#$%", "Result: invalid input")]

        // Infinity cases
        [TestCase("Infinity", "+", "Infinity", "Result: Infinity")]
        [TestCase("Infinity", "-", "Infinity", "Result: invalid calculation")]
        [TestCase("Infinity", "*", "Infinity", "Result: Infinity")]
        [TestCase("Infinity", "/", "Infinity", "Result: invalid calculation")]
        [TestCase("Infinity", "+", "0", "Result: Infinity")]
        [TestCase("Infinity", "-", "0", "Result: Infinity")]
        [TestCase("Infinity", "*", "0", "Result: invalid calculation")]
        [TestCase("Infinity", "/", "0", "Result: Infinity")]
        [TestCase("0", "+", "Infinity", "Result: Infinity")]
        [TestCase("0", "-", "Infinity", "Result: -Infinity")]
        [TestCase("0", "*", "Infinity", "Result: invalid calculation")]
        [TestCase("0", "/", "Infinity", "Result: 0")]

        public void TestCalculatorWebApp
            (string num1, string operation, string num2, string expResult)
        {
            //Arrange
            resetButton.Click();
            if (num1 != "")
                firtNumTextBox.SendKeys(num1);
            if (num2 != "")
                secondNumTextBox.SendKeys(num2);
            if (operation != "")
                operationDropDown.SendKeys(operation);

            //Act
            calculateButton.Click();

            //Assert
            Assert.AreEqual(expResult, resultField.Text);
            Thread.Sleep(500);  // delays with 0.5 sec. after every test
        }

        [OneTimeTearDown]
        public void ShutDown()
        {
            driver.Quit();
        }
    }
}