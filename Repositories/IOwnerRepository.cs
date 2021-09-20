using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public class IOwnerRepository : Controller
    {
        // GET: IOwnerRepository
        public ActionResult Index()
        {
            return View();
        }

        // GET: IOwnerRepository/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: IOwnerRepository/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: IOwnerRepository/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IOwnerRepository/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: IOwnerRepository/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: IOwnerRepository/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: IOwnerRepository/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
