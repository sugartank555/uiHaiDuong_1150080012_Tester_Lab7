using NUnit.Framework;
using Lab7_BlackBox_Testing.Base;
using Lab7_BlackBox_Testing.Pages;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Globalization;

namespace Lab7_BlackBox_Testing.Tests
{
    [TestFixture]
    public class TC_GioHangTest : BaseTest
    {
        InventoryPage inventoryPage;

        [SetUp]
        public void ChuanBi()
        {
            GetDriver().Navigate().GoToUrl("https://www.saucedemo.com/");
            LoginPage loginPage = new LoginPage(GetDriver());
            loginPage.DangNhap("standard_user", "secret_sauce");

            inventoryPage = new InventoryPage(GetDriver());
        }

        [Test, Category("smoke")]
        [Description("TC_CART_001: Thêm 1 sản phẩm -> badge = 1")]
        public void ThemMotSanPham()
        {
            inventoryPage.ThemNSanPhamDauTien(1);
            Assert.AreEqual(1, inventoryPage.LaySoLuongBadge(), "Badge giỏ hàng không đúng!");
        }

        [Test, Category("smoke")]
        [Description("TC_CART_002: Thêm 3 sản phẩm -> badge = 3")]
        public void Them3SanPham()
        {
            inventoryPage.ThemNSanPhamDauTien(3);
            Assert.AreEqual(3, inventoryPage.LaySoLuongBadge(), "Badge giỏ hàng không đúng khi thêm 3 SP!");
        }

        [Test, Category("regression")]
        [Description("TC_CART_010: Kiểm tra tổng tiền chính xác")]
        public void KiemTraTongTien()
        {
            // Thiết lập WebDriverWait (tối đa 10 giây)
            WebDriverWait wait = new WebDriverWait(GetDriver(), TimeSpan.FromSeconds(10));

            // 1. Thêm 2 sản phẩm
            inventoryPage.ThemNSanPhamDauTien(2);
            inventoryPage.ClickGioHang();

            // 2. CHỜ và Click Checkout
            IWebElement checkoutBtn = wait.Until(d => d.FindElement(By.Id("checkout")));
            checkoutBtn.Click();

            // 3. CHỜ ô điền tên xuất hiện rồi mới điền form
            IWebElement firstNameInput = wait.Until(d => d.FindElement(By.Id("first-name")));
            firstNameInput.SendKeys("Test");
            GetDriver().FindElement(By.Id("last-name")).SendKeys("User");
            GetDriver().FindElement(By.Id("postal-code")).SendKeys("12345");
            GetDriver().FindElement(By.Id("continue")).Click();

            // 4. CHỜ nhãn tổng tiền xuất hiện
            IWebElement subtotalLabel = wait.Until(d => d.FindElement(By.ClassName("summary_subtotal_label")));

            // 5. Lấy text hiển thị
            string itemTotalStr = subtotalLabel.Text;
            string taxStr = GetDriver().FindElement(By.ClassName("summary_tax_label")).Text;
            string totalStr = GetDriver().FindElement(By.ClassName("summary_total_label")).Text;

            // 6. Ép kiểu
            double itemTotal = double.Parse(itemTotalStr.Replace("Item total: $", "").Trim(), CultureInfo.InvariantCulture);
            double tax = double.Parse(taxStr.Replace("Tax: $", "").Trim(), CultureInfo.InvariantCulture);
            double total = double.Parse(totalStr.Replace("Total: $", "").Trim(), CultureInfo.InvariantCulture);

            // 7. Kiểm tra kết quả
            Assert.IsTrue(Math.Abs(tax - (itemTotal * 0.08)) < 0.01, $"Thuế tính sai! Web: {tax}, Thực tế tính: {itemTotal * 0.08}");
            Assert.IsTrue(Math.Abs(total - (itemTotal + tax)) < 0.01, $"Tổng tiền tính sai! Web: {total}, Thực tế: {itemTotal + tax}");
        }
    }
}