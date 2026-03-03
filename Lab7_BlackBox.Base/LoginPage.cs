using OpenQA.Selenium;

namespace Lab7_BlackBox_Testing.Pages
{
    public class LoginPage
    {
        private IWebDriver _driver;

        // Khai báo các Locators
        private By userNameField = By.Id("user-name");
        private By passwordField = By.Id("password");
        private By loginButton = By.Id("login-button");
        private By errorMessage = By.CssSelector("[data-test='error']");

        public LoginPage(IWebDriver driver)
        {
            _driver = driver;
        }

        public void NhapUsername(string username) => _driver.FindElement(userNameField).SendKeys(username);
        public void NhapPassword(string password) => _driver.FindElement(passwordField).SendKeys(password);
        public void ClickDangNhap() => _driver.FindElement(loginButton).Click();

        // Thực hiện đăng nhập đầy đủ
        public void DangNhap(string user, string pass)
        {
            if (!string.IsNullOrEmpty(user)) NhapUsername(user);
            if (!string.IsNullOrEmpty(pass)) NhapPassword(pass);
            ClickDangNhap();
        }

        // Lấy thông báo lỗi, trả về null nếu không có lỗi
        public string LayThongBaoLoi()
        {
            try
            {
                return _driver.FindElement(errorMessage).Text;
            }
            catch (NoSuchElementException)
            {
                return null;
            }
        }

        // Kiểm tra đã chuyển sang trang inventory chưa
        public bool IsDangTrangSanPham()
        {
            return _driver.Url.Contains("inventory.html");
        }
    }
}