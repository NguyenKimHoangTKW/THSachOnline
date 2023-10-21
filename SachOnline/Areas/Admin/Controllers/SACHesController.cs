using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SachOnline.Models;
using PagedList;
using System.IO;
using SachOnline.App_Start;

namespace SachOnline.Areas.Admin.Controllers
{
    [AdminAuthorize]
    public class SACHesController : Controller
    {
        private SachOnlineEntities db = new SachOnlineEntities();

        // GET: Admin/SACHes
        public ActionResult Index(int? page)
        {
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            var sACHes = db.SACHes.Include(s => s.CHUDE).Include(s => s.NHAXUATBAN).OrderBy(s => s.MaSach).OrderByDescending(s => s.NgayCapNhat);
            return View(sACHes.ToPagedList(pageNumber, pageSize));
        }

        // GET: Admin/SACHes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        // GET: Admin/SACHes/Create
        public ActionResult Create()
        {
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe");
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB");
            return View();
        }

        // POST: Admin/SACHes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create(SACH sACH, HttpPostedFileBase AnhBia)
        {
            if (ModelState.IsValid)
            {
                if (AnhBia != null && AnhBia.ContentLength > 0)
                {
                    string _Head = Path.GetFileNameWithoutExtension(AnhBia.FileName);
                    string _Tail = Path.GetExtension(AnhBia.FileName);
                    string fullLink = _Head + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + _Tail;
                    string _path = Path.Combine(Server.MapPath("~/Style/Images"), fullLink);
                    AnhBia.SaveAs(_path);
                    sACH.AnhBia = fullLink;
                    db.SACHes.Add(sACH);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng chọn một tệp ảnh.");
                }
            }

            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // GET: Admin/SACHes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // POST: Admin/SACHes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "MaSach,TenSach,MoTa,AnhBia,NgayCapNhat,SoLuongBan,GiaBan,MaCD,MaNXB")] SACH sACH, HttpPostedFileBase AnhBia, FormCollection form)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (AnhBia != null)
                    {
                        string _Head = Path.GetFileNameWithoutExtension(AnhBia.FileName);
                        string _Tail = Path.GetExtension(AnhBia.FileName);
                        string fullLink = _Head + "-" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + _Tail;
                        string _path = Path.Combine(Server.MapPath("~/Style/Images"), fullLink);
                        AnhBia.SaveAs(_path);
                        sACH.AnhBia = fullLink;
                        _path = Path.Combine(Server.MapPath("~/Style/Images"), form["oldimage"]);

                        if (System.IO.File.Exists(_path))
                            System.IO.File.Delete(_path);

                    }
                    else
                    sACH.AnhBia = form["oldimage"];
                    db.Entry(sACH).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
                catch
                {
                    ViewBag.Message = "không thành công!!";
                }
                return RedirectToAction("Index");
            }
            ViewBag.MaCD = new SelectList(db.CHUDEs, "MaCD", "TenChuDe", sACH.MaCD);
            ViewBag.MaNXB = new SelectList(db.NHAXUATBANs, "MaNXB", "TenNXB", sACH.MaNXB);
            return View(sACH);
        }

        // GET: Admin/SACHes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SACH sACH = db.SACHes.Find(id);
            if (sACH == null)
            {
                return HttpNotFound();
            }
            return View(sACH);
        }

        // POST: Admin/SACHes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SACH sACH = db.SACHes.Find(id);
            if(db.CHITIETDATHANGs.Any(c => c.MaSach == sACH.MaSach))
            {
                ViewBag.ThongBao = "Không thể xóa sách này vì tồn tại khóa ngoại bên Chi Tiết đơn hàng.Nếu muốn xóa phải xóa hết Chi tiết đơn hàng liên quan đến sách này";
            }
            else if (db.VIETSACHes.Any(v => v.MaSach == sACH.MaSach))
            {
                ViewBag.ThongBao = "Không thể xóa sách này vì tồn tại khóa ngoại bên Người viết sách.Nếu muốn xóa phải xóa hết Người viết sách liên quan đến sách này";
            }
            else
            {
                db.SACHes.Remove(sACH);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sACH);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
