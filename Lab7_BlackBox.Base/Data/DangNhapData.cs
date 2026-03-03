using NUnit.Framework;
using System.Collections;

namespace Lab7_BlackBox_Testing.Data
{
    public class DangNhapData
    {
        public static IEnumerable DuLieuDangNhap()
        {
            // Cấu trúc: Username, Password, Kết quả mong đợi, Mô tả
            yield return new TestCaseData("standard_user", "secret_sauce", "THÀNH_CÔNG", "Tài khoản hợp lệ");
            yield return new TestCaseData("locked_out_user", "secret_sauce", "BỊ_KHÓA", "Tài khoản bị khóa");
            yield return new TestCaseData("invalid_user", "wrong_pass", "SAI_THÔNG_TIN", "Tài khoản không tồn tại");
            yield return new TestCaseData("", "secret_sauce", "TRƯỜNG_TRỐNG", "Để trống Username");
            yield return new TestCaseData("standard_user", "", "TRƯỜNG_TRỐNG", "Để trống Password");
            yield return new TestCaseData("", "", "TRƯỜNG_TRỐNG", "Để trống cả hai");
        }
    }
}