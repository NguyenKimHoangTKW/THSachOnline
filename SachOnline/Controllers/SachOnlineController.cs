using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        private SachOnlineEntities db = new SachOnlineEntities();
        // GET: SachOnline
        [HttpGet]
        public ActionResult Index()
        {
            var lisSachMoi = LaySachMoi(6);
            return View(lisSachMoi);
        }
        private List<SACH> LaySachMoi(int count)
        {
            return db.SACHes.OrderByDescending(a => a.NgayCapNhat).Take(count).ToList();
        }
        public ActionResult ChiTietSach(int id)
        {
            var sach = from s in db.SACHes
                       where s.MaSach == id select s;
            return View(sach.Single());
        }
        public ActionResult SachBanNhieu()
        {
            var lisSachMoi = LaySachMoi(6);
            return PartialView(lisSachMoi);
        }
        public ActionResult ChuDePartialView()
        {
            var listchude = from cd in db.CHUDEs select cd;
            return PartialView(listchude);
        }
        public ActionResult NXBPartialView()
        {
            var listNXB = from nxb in db.NHAXUATBANs select nxb;
            return PartialView(listNXB);
        }
        public ActionResult SachTheoChuDe(int id)
        {
            var sach = from s in db.SACHes where s.MaCD == id select s;
            return View(sach);
        }
        public ActionResult SachTheoNhaXuatBan(int id)
        {
            var sachs = from nxb in db.SACHes where nxb.MaNXB == id select nxb;
            return View(sachs);
        }
    }
}