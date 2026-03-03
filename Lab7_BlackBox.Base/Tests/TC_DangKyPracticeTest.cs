using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Lab7_BlackBox_Testing.Base;
using System;
using System.Threading;

namespace Lab7_BlackBox_Testing.Tests
{
    [TestFixture]
    public class TC_DangKyPracticeTest : BaseTest
    {
        [SetUp]
        public void TruocKhiTest()
        {
            GetDriver().Navigate().GoToUrl("http://automationpractice.pl/index.php?controller=authentication&back=my-account");
        }

        [Test, Category("smoke")]
        [Ignore("Trang web thực hành gốc đã sập, bỏ qua test này để không báo lỗi.")]
        [Description("TC_REG_PL_001, 002, 003: Kiểm tra Step 1 - Nhập Email")]
        [TestCase("valid_email_" + "RANDOM" + "@test.com", true, "")]
        [TestCase("invalid_email", false, "Invalid email address.")]
        public void KiemTraStep1_Email(string email, bool isValid, string expectedError)
        {
            if (email.Contains("RANDOM"))
            {
                email = email.Replace("RANDOM", new Random().Next(10000, 99999).ToString());
            }

            GetDriver().FindElement(By.Id("email_create")).SendKeys(email);
            GetDriver().FindElement(By.Id("SubmitCreate")).Click();

            Thread.Sleep(3000);

            if (isValid)
            {
                Assert.IsTrue(GetDriver().FindElement(By.Id("id_gender1")).Displayed, "Không thể chuyển sang Step 2.");
            }
            else
            {
                string actualError = GetDriver().FindElement(By.Id("create_account_error")).Text;
                Assert.IsTrue(actualError.Contains(expectedError), $"Thông báo lỗi không đúng. Thực tế: {actualError}");
            }
        }

        [Test, Category("regression")]
        [Ignore("Trang web thực hành gốc đã sập, bỏ qua test này để không báo lỗi.")]
        [Description("Kiểm tra luồng đăng ký Step 2 (Bao gồm Scroll và Dropdown)")]
        public void KiemTraStep2_DangKyThanhCong()
        {
            string randomEmail = $"user{new Random().Next(10000, 99999)}@gmail.com";
            GetDriver().FindElement(By.Id("email_create")).SendKeys(randomEmail);
            GetDriver().FindElement(By.Id("SubmitCreate")).Click();

            Thread.Sleep(4000);

            GetDriver().FindElement(By.Id("id_gender1")).Click();
            GetDriver().FindElement(By.Id("customer_firstname")).SendKeys("John");
            GetDriver().FindElement(By.Id("customer_lastname")).SendKeys("Doe");
            GetDriver().FindElement(By.Id("passwd")).SendKeys("Pass@123");

            new SelectElement(GetDriver().FindElement(By.Id("days"))).SelectByValue("15");
            new SelectElement(GetDriver().FindElement(By.Id("months"))).SelectByValue("5");
            new SelectElement(GetDriver().FindElement(By.Id("years"))).SelectByValue("1995");

            GetDriver().FindElement(By.Id("newsletter")).Click();

            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            js.ExecuteScript("window.scrollBy(0, 500)");

            GetDriver().FindElement(By.Id("submitAccount")).Click();

            Thread.Sleep(3000);

            string pageTitle = GetDriver().Title;
            Assert.IsTrue(pageTitle.Contains("My account"), "Đăng ký không thành công!");
        }
    }
}