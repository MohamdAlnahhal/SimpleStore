using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Store.Models;
using Microsoft.AspNet.Identity;
using PagedList;

namespace Store.Controllers
{
    [Authorize]
    public class GoodsController : Controller
    {
        private StoreDBEntities db = new StoreDBEntities();

        // GET: Goods
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            


            ViewBag.CurrentSort = sortOrder;
            ViewBag.TitleSortParm = String.IsNullOrEmpty(sortOrder) ? "Title_desc" : "";


            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;


            // Searching 
            var goods = from c in db.Goods
                           select c;
            if (!string.IsNullOrEmpty(searchString))
            {
                goods = goods.Where(g => g.Title == searchString);
               
            }

            // Sorting
            switch (sortOrder)
            {
                case "Title_desc":
                    goods = goods.OrderByDescending(g => g.Title);
                    break;

                case "Type_desc":
                    goods = goods.OrderByDescending(g => g.Type);
                    break;

                case "Price_desc":
                    goods = goods.OrderByDescending(g => g.Price);
                    break;
            }

            //paging 
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(goods.ToList().ToPagedList(pageNumber, pageSize));


        }

        // GET: Goods/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Good good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        // GET: Goods/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Goods/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "GoodID,Title,Type,Price")] Good good)
        {
            if (ModelState.IsValid)
            {
                db.Goods.Add(good);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(good);
        }

        // GET: Goods/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Good good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        // POST: Goods/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "GoodID,Title,Type,Price")] Good good)
        {
            if (ModelState.IsValid)
            {
                db.Entry(good).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(good);
        }

        // GET: Goods/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Good good = db.Goods.Find(id);
            if (good == null)
            {
                return HttpNotFound();
            }
            return View(good);
        }

        // POST: Goods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Good good = db.Goods.Find(id);
            db.Goods.Remove(good);
            db.SaveChanges();
            return RedirectToAction("Index");
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
