using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Repositories;
using DogGo.Models;

namespace DogGo.Controllers
{
    public class WalksController : Controller
    {
        private readonly IWalkRepository _walkRepo;

        // ASP.NET will give us an instance of our Walk Repository. This is called "Dependency Injection"
        public WalksController(IWalkRepository walkRepository)
        {
            _walkRepo = walkRepository;
        }

        // GET: Walks
        // Method gets all the walks in the Walks table, convert it to a List and pass it off to the view.
        public ActionResult Index()
        {
            List<Walk> walks = _walkRepo.GetAllWalks();

            return View(walks);
        }

        // GET: WalksController/Details/5
        // GET: Walks/Details/{Id}
        public ActionResult Details(int id)
        {
            Walk walk = _walkRepo.GetWalkById(id);

            if (walk == null)
            {
                return NotFound();
            }

            return View(walk);
        }

        // GET: WalksController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalksController/Create
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

        // GET: WalksController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalksController/Edit/5
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

        // GET: WalksController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalksController/Delete/5
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
