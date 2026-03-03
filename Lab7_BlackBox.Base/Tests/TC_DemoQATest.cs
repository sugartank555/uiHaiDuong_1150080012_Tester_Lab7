using NUnit.Framework;
using OpenQA.Selenium;
using Lab7_BlackBox_Testing.Base;
using System.Threading;

namespace Lab7_BlackBox_Testing.Tests
{
    [TestFixture]
    public class TC_DemoQATest : BaseTest
    {
        [SetUp]
        public void MoTrangWeb()
        {
            GetDriver().Navigate().GoToUrl("https://demoqa.com/text-box");
        }

        [Test]
        [Description("TC_DEMOQA_001: Điền form với thông tin hợp lệ")]
        public void KiemThu_FormHopLe()
        {
            // 1. Điền dữ liệu
            GetDriver().FindElement(By.Id("userName")).SendKeys("Nguyen Van A");
            GetDriver().FindElement(By.Id("userEmail")).SendKeys("test@gmail.com");
            GetDriver().FindElement(By.Id("currentAddress")).SendKeys("123 Le Loi, Hanoi");
            GetDriver().FindElement(By.Id("permanentAddress")).SendKeys("456 Nguyen Trai, HCM");

            // 2. Xử lý cuộn trang để thấy nút Submit (Tránh bị quảng cáo che)
            IWebElement submitBtn = GetDriver().FindElement(By.Id("submit"));
            IJavaScriptExecutor js = (IJavaScriptExecutor)GetDriver();
            js.ExecuteScript("arguments[0].scrollIntoView(true);", submitBtn);
            Thread.Sleep(500); // Đợi hiệu ứng cuộn hoàn tất

            // 3. Click Submit
            submitBtn.Click();

            // 4. Assert - Kiểm tra khung kết quả xuất hiện và đúng dữ liệu
            Assert.IsTrue(GetDriver().FindElement(By.Id("output")).Displayed, "Khung kết quả không xuất hiện!");

            string outputName = GetDriver().FindElement(By.Id("name")).Text;
            string outputEmail = GetDriver().FindElement(By.Id("email")).Text;

            Assert.IsTrue(outputName.Contains("Nguyen Van A"), "Sai thông tin Name!");
            Assert.IsTrue(outputEmail.Contains("test@gmail.com"), "Sai thông tin Email!");
        }

        [Test]
        [Description("TC_DEMOQA_003: Điền sai định dạng Email")]
        public void KiemThu_EmailSaiDinhDang()
        {
            // 1. Điền dữ liệu với Email sai định dạng (thiếu @)
            GetDriver().FindElement(By.Id("userName")).SendKeys("Nguyen Van B");
            GetDriver().FindElement(By.Id("userEmail")).SendKeys("invalid_email.com");

            // 2. Cuộn và Click Submit
            IWebElement submitBtn = GetDriver().FindElement(By.Id("submit"));
            ((IJavaScriptExecutor)GetDriver()).ExecuteScript("arguments[0].scrollIntoView(true);", submitBtn);
            Thread.Sleep(500);
            submitBtn.Click();

            // 3. Assert - Kiểm tra class của ô input Email có chứa "field-error" (viền đỏ) hay không
            IWebElement emailField = GetDriver().FindElement(By.Id("userEmail"));
            string classAttribute = emailField.GetAttribute("class");

            Assert.IsTrue(classAttribute.Contains("field-error"), "Hệ thống không báo lỗi viền đỏ cho Email sai định dạng!");
        }
    }
}