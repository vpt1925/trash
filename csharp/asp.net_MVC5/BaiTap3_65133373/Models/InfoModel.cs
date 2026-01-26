using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BaiTap3_65133373.Models
{
    public class InfoModel
    {
        [DisplayName("Mã số nhân viên")]
        public string EmpID { get; set; }
        [DisplayName("Họ tên nhân viên")]
        public string Name { get; set; }
        [DisplayName("Ngày sinh"), DataType(DataType.Date)]
        public DateTime BirthOfDate { get; set; }
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Mật khẩu"), DataType(DataType.Password)]
        public string Password { get; set; }
        [DisplayName("Đơn vị")]
        public string Department { get; set; }
        [DisplayName("Ảnh đại diện")]
        public string Avatar { get; set; }
    }
}