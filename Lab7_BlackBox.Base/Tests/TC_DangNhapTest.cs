using NUnit.Framework;
using Lab7_BlackBox_Testing.Base;
using Lab7_BlackBox_Testing.Pages;
using Lab7_BlackBox_Testing.Data;

namespace Lab7_BlackBox_Testing.Tests
{
    [TestFixture]
    public class TC_DangNhapTest : BaseTest
    {
        [Test]
        [TestCaseSource(typeof(DangNhapData), nameof(DangNhapData.DuLieuDangNhap))]
        [Description("Kiểm thử đăng nhập với nhiều bộ dữ liệu")]
        public void KiemThuDangNhap(string username, string password, string ketQuaMongDoi, string moTa)
        {
            // 1. Mở trang web
            GetDriver().Navigate().GoToUrl("https://www.saucedemo.com/");

            // 2. Khởi tạo Page Object
            LoginPage loginPage = new LoginPage(GetDriver());

            // 3. Thực hiện đăng nhập
            loginPage.DangNhap(username, password);

            // 4. Kiểm tra kết quả
            if (ketQuaMongDoi == "THÀNH_CÔNG")
            {
                Assert.IsTrue(loginPage.IsDangTrangSanPham(), $"[{moTa}] Lỗi: Không thể chuyển sang trang Inventory.");
            }
            else
            {
                string actualError = loginPage.LayThongBaoLoi();
                Assert.IsNotNull(actualError, $"[{moTa}] Lỗi: Form không hiển thị thông báo lỗi nào.");

                // Assert các loại lỗi khác nhau
                switch (ketQuaMongDoi)
                {
                    case "BỊ_KHÓA":
                        Assert.IsTrue(actualError.Contains("locked out"), $"[{moTa}] Sai thông báo. Thực tế: {actualError}");
                        break;
                    case "SAI_THÔNG_TIN":
                        Assert.IsTrue(actualError.Contains("do not match"), $"[{moTa}] Sai thông báo. Thực tế: {actualError}");
                        break;
                    case "TRƯỜNG_TRỐNG":
                        Assert.IsTrue(actualError.Contains("is required"), $"[{moTa}] Sai thông báo. Thực tế: {actualError}");
                        break;
                }
            }
        }
    }
}