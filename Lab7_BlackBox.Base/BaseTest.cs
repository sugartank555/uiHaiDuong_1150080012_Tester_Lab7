using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Threading;

namespace Lab7_BlackBox_Testing.Base
{
    public abstract class BaseTest
    {
        // Sử dụng ThreadLocal để hỗ trợ chạy Parallel Execution an toàn
        private ThreadLocal<IWebDriver> driver = new ThreadLocal<IWebDriver>();

        [SetUp]
        public void SetUp()
        {
            // Khởi tạo ChromeDriver (Selenium 4 tự động tải driver phù hợp)
            driver.Value = new ChromeDriver();
            driver.Value.Manage().Window.Maximize();
            driver.Value.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            TestContext.Progress.WriteLine($"[START] Đang chạy test: {TestContext.CurrentContext.Test.Name}");
        }

        [TearDown]
        public void TearDown()
        {
            if (driver.Value != null)
            {
                // TODO: Bạn có thể thêm code chụp màn hình ở đây nếu TestContext báo lỗi
                driver.Value.Quit();
                driver.Value.Dispose();
            }
        }

        public IWebDriver GetDriver()
        {
            return driver.Value;
        }
    }
}