using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;
using SachOnline.Models;

namespace SachOnline.Controllers
{
    public class AccountController : Controller
    {
        private SachOnlineEntities db = new SachOnlineEntities();
        // GET: Account
        public ActionResult Index()
        {
            return View(db.KHACHHANGs.ToList());
        }
        [HttpGet]
        // GET: Account/Create
        public ActionResult DangKy()
        {
            return View();
        }

        // POST: Account/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(KHACHHANG kHACHHANG, FormCollection collection)
        {
            var sHoten = collection["HoTen"];
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            var sMatKhauNhapLai = collection["MatKhauNhapLai"];
            var sDiaChi = collection["DiaChi"];
            var sEmail = collection["Email"];
            var sDienThoai = collection["DienThoai"];
            var dNgaySinh = String.Format("{0:mm/dd/yyyy}", collection["NgaySinh"]);
            if (ModelState.IsValid)
            {
                if (String.IsNullOrEmpty(sHoten))
                {
                    ViewData["err1"] = "Họ tên không được để trống";
                }
                else if (String.IsNullOrEmpty(sTenDN))
                {
                    ViewData["err2"] = "Tên đăng nhập không được để trống";
                }
                else if (String.IsNullOrEmpty(sMatKhau))
                {
                    ViewData["err3"] = "Mật khẩu không được để trống";
                }
                else if (String.IsNullOrEmpty(sMatKhauNhapLai))
                {
                    ViewData["err4"] = "Mật khẩu không được để trống";
                }
                else if (sMatKhau != sMatKhauNhapLai)
                {
                    ViewData["err4"] = "Mật khẩu nhập lại không khớp";
                }
                else if (String.IsNullOrEmpty(sEmail))
                {
                    ViewData["err5"] = "Email không được để trống";
                }
                else if (String.IsNullOrEmpty(sDienThoai))
                {
                    ViewData["err6"] = "Số điện thoại không được để trống";
                }
                else if (db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN) != null)
                {
                    ViewBag.ThongBao = "Tên đăng nhập đã tồn tại";
                }
                else if (db.KHACHHANGs.SingleOrDefault(n => n.Email == sEmail) != null)
                {
                    ViewBag.ThongBao = "Email đã tồn tại";
                }
                else
                {
                    kHACHHANG.HoTen = sHoten;
                    kHACHHANG.TaiKhoan = sTenDN;
                    kHACHHANG.MatKhau = sMatKhau;
                    kHACHHANG.DienThoai = sDienThoai;
                    kHACHHANG.DiaChi = sDiaChi;
                    kHACHHANG.Email = sEmail;
                    kHACHHANG.NgaySinh = DateTime.Parse(dNgaySinh);
                    db.KHACHHANGs.Add(kHACHHANG);
                    db.SaveChanges();
                    return RedirectToAction("DangNhap");
                }
                
            }

            return View(kHACHHANG);
        }
        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection collection)
        {
            var sTenDN = collection["TenDN"];
            var sMatKhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(sTenDN))
            {
                ViewData["Err1"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(sMatKhau))
            {
                ViewData["Err2"] = "Mật khẩu không được để trống";
            }
            else
            {
                KHACHHANG kh = db.KHACHHANGs.SingleOrDefault(n => n.TaiKhoan == sTenDN && n.MatKhau == sMatKhau);
                if (kh != null)
                {
                    ViewBag.ThongBao = "Chúc mừng đăng nhập thành công";
                    Session["TaiKhoan"] = kh;
                    return RedirectToAction("GioHang","GioHang");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("Index","SachOnline");
        }
    }
}
