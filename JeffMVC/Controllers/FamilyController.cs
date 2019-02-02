using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JeffMVC.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JeffMVC.Controllers
{
    public class FamilyController : Controller
    {
		FamilyContext _ctx;

		public FamilyController(FamilyContext ctx)
		{
			_ctx = ctx; 
		}

        // GET: Family
        public ActionResult Index()
        {
            return View(_ctx);
        }

        // GET: Family/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Family/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Family/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Family/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Family/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Family/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Family/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}