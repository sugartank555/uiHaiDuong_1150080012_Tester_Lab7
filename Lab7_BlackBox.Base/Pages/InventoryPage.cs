using OpenQA.Selenium;
using System;

namespace Lab7_BlackBox_Testing.Pages
{
    public class InventoryPage
    {
        private IWebDriver _driver;

        private By cartBadge = By.ClassName("shopping_cart_badge");
        private By cartLink = By.ClassName("shopping_cart_link");

        public InventoryPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void ThemNSanPhamDauTien(int n)
        {
            // Lấy TẤT CẢ các nút sản phẩm trên trang (SauceDemo dùng chung class btn_inventory)
            var buttons = _driver.FindElements(By.CssSelector(".btn_inventory"));

            for (int i = 0; i < n && i < buttons.Count; i++)
            {
                buttons[i].Click();
                // Cho vòng lặp nghỉ cứng 0.5s để ReactJS vẽ xong giao diện rồi mới chạy tiếp
                System.Threading.Thread.Sleep(500);
            }
        }

        public int LaySoLuongBadge()
        {
            try
            {
                string text = _driver.FindElement(cartBadge).Text;
                return int.Parse(text);
            }
            catch (NoSuchElementException)
            {
                return 0; // Nếu giỏ hàng trống sẽ không có badge
            }
        }

        public void ClickGioHang()
        {
            _driver.FindElement(cartLink).Click();
        }
    }
}