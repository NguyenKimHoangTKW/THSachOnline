using SachOnline.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.Web.UI;

namespace SachOnline.Controllers
{
    public class SachOnlineController : Controller
    {
        private SachOnlineEntities db = new SachOnlineEntities();
        // GET: SachOnline
        [HttpGet]
        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var lisSachMoi = db.SACHes.ToList();
            return View(lisSachMoi.ToPagedList(pageNumber,pageSize));
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
        public ActionResult SachTheoChuDe(int iMaCD, int? page)
        {
            ViewBag.MaCD = iMaCD;
            int pageSize = 3;
            var sach = (from l in db.SACHes
                         where l.MaCD == iMaCD
                         select l).OrderBy(x => x.MaSach);
            int pageNumber = (page ?? 1);          
            return View(sach.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult SachTheoNhaXuatBan(int iMaNXB, int? page)
        {
            ViewBag.MaNXB = iMaNXB;
            int pageSize = 3;
            var nxb = (from s in db.SACHes
                       where s.MaNXB == iMaNXB
                       select s).OrderBy(x => x.MaSach);
            int pageNumber = (page ?? 1);
            return View(nxb.ToPagedList(pageNumber, pageSize));
        }
   
        
    }
}