using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support;
using OpenQA.Selenium.Chrome;
using System.Drawing;
using System.Net.Sockets;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework.Internal;
using NUnit.Framework.Interfaces;

namespace Test_TanPhuc
{
    public class Tests
    {
        private static IWebDriver driver;
        // function test
        private static IEnumerable<TestCaseData> GetDataForNameRoom_CreateRoom_Test()
        {
            return ExcelProvider.GetDataForAddRoom(10,27);  
        }
        private static RoomInfo ParseStringDataToObject(string dataTest)
        {
            var lines = dataTest.Split('\n');

            return new RoomInfo
            {
                RoomName = lines[0].Split(':')[1].Trim() ?? "",
                Capacity = lines[1].Split(':')[1].Trim() ?? "",
                Quantity = lines[2].Split(':')[1].Trim() ?? "",
                Beds = lines[3].Split(':')[1].Trim() ?? "",
                Price = lines[4].Split(':')[1].Trim() ?? "",
                RoomType = lines[5].Split(':')[1].Trim() ?? "",
                HotelName = lines[6].Split(':')[1].Trim() ?? ""
            };


        }

        private void Login(string emailInput, string passwordInput)
        {
            IWebElement email = driver.FindElement(By.Id("email"));
            IWebElement password = driver.FindElement(By.Id("password"));
            IWebElement buttonLogin = driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[2]/div[1]/div[1]/div[1]/div[2]/div[1]/div[3]/form[1]/div[3]/div[1]/div[1]/div[1]/div[1]/button[1]"));

            email.SendKeys(emailInput);
            password.SendKeys(passwordInput);
            buttonLogin.Click();
            
            Thread.Sleep(1000);
        }
        public void GetValueInSelectorAntd(string value, IList<IWebElement> options)
        {

            bool valueFound = false;
            foreach (IWebElement option in options)
            {
                
                if (option.Text == value)
                {
                    option.Click();
                    valueFound = true;
                    break;
                }

            }
            if (!valueFound)
            {
                IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                js.ExecuteScript("arguments[0].scrollIntoView(true);", options.First());
                options = driver.FindElements(By.ClassName("ant-select-item-option"));

                foreach (IWebElement option in options)
                {
                    if (option.Text == value)
                    {
                        option.Click();
                        break;
                    }
                }
            }

        }
        [SetUp]
        public void Setup()
        {
            driver= new ChromeDriver();
            driver.Navigate().GoToUrl("http://localhost:3000/loginOwner");
        }

        [Test]
        [TestCaseSource(nameof(GetDataForNameRoom_CreateRoom_Test))]
        public void TestCreateRoom_NameRoom(string testData,string expResult)
        {
            Login("b@gmail.com", "123123123");
            Thread.Sleep(2000);
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[1]/div[1]/ul[1]/li[3]/span[1]/p[1]")).Click();// Click room in menu 
            driver.FindElement(By.XPath("/html[1]/body[1]/div[1]/div[1]/div[1]/div[2]/div[2]/div[1]/a[1]/button[1]/span[2]")).Click();//Click create room
            Thread.Sleep(2000);
            //Attribute for form create room
            IWebElement nameRoom = driver.FindElement(By.Id("createRoom_roomName"));
            IWebElement capacityRoom = driver.FindElement(By.Id("createRoom_capacity"));
            IWebElement quantityRoom = driver.FindElement(By.Id("createRoom_numberOfRooms"));
            IWebElement numberOfBed = driver.FindElement(By.Id("createRoom_numberOfBeds"));
            IWebElement priceRoom = driver.FindElement(By.Id("createRoom_money"));
            Thread.Sleep(1000);
            IWebElement typeRoom = driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/form[1]/div[6]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/span[1]/span[2]"));
            IWebElement belongHotel = driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/form[1]/div[7]/div[1]/div[2]/div[1]/div[1]/div[1]/div[1]/span[1]/span[2]"));
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));

            RoomInfo roomInfo = ParseStringDataToObject(testData);
            // enter value
            nameRoom.SendKeys(roomInfo.RoomName);
            capacityRoom.SendKeys(roomInfo.Capacity);
            quantityRoom.SendKeys(roomInfo.Quantity);
            numberOfBed.SendKeys(roomInfo.Beds);
            priceRoom.SendKeys(roomInfo.Price);
            // select type room
            typeRoom.Click();
            wait.Until(d => d.FindElement(By.ClassName("ant-select-dropdown")));
            IList<IWebElement> optionTypeRoom = driver.FindElements(By.ClassName("ant-select-item-option"));
            GetValueInSelectorAntd(roomInfo.RoomType, optionTypeRoom);
            // select hotel
            belongHotel.Click();
            wait.Until(d => d.FindElement(By.ClassName("ant-select-dropdown")));
            IList<IWebElement> optionHotel = driver.FindElements(By.ClassName("ant-select-item-option"));
            GetValueInSelectorAntd(roomInfo.HotelName, optionHotel);

            IWebElement buttonAdd = driver.FindElement(By.XPath("/html[1]/body[1]/div[2]/div[1]/div[2]/div[1]/div[1]/div[1]/div[2]/button[2]/span[1]"));
            buttonAdd.Click();
            Thread.Sleep(1000);
            IWebElement notification = driver.FindElement(By.Id("notification"));

            string actual = notification.GetAttribute("innerText");
          
            bool status = actual.Equals(expResult.Trim()) ? true : false;
            ExcelProvider.WriteResultToExcel("C:\\Users\\phuct\\OneDrive - Ho Chi Minh City University of Foreign Languages and Information Technology - HUFLIT\\TestCase_BDCLPM_HK2.xlsx", "TestCase_TanPhuc", actual, status.ToString());

            Assert.That(actual, Is.EqualTo(expResult.Trim()), "Khong thoa");

        }

        [TearDown]
        public void TearDown()
        {
            Thread.Sleep(5000);
            driver.Dispose();
        }
    }
}