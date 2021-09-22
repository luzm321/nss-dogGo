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
        // GET: Walks/Create
        // Creates a blank HTML form to be filled out with input from user with Create View:
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalksController/Create
        // POST: Walks/Create
        [HttpPost] // Flag attribute informing app the kind of request it should handle
        [ValidateAntiForgeryToken] // Flag attribute informing app the kind of request it should handle
        public ActionResult Create(Walk walk)
        {
            try
            {
                _walkRepo.AddWalk(walk);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(walk);
            }
        }

        // GET: WalksController/Edit/5
        // GET: Walks/Edit/Id
        public ActionResult Edit(int id)
        {
            Walk walk = _walkRepo.GetWalkById(id);

            if (walk == null)
            {
                return NotFound();
            }

            return View(walk);
        }

        // POST: WalksController/Edit/5
        // POST: Walks/Edit/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Walk walk)
        {
            try
            {
                _walkRepo.UpdateWalk(walk);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(walk);
            }
        }

        // GET: WalksController/Delete/5
        // GET: Walks/Delete/Id
        // Create a view that asks the user to confirm the deletion:
        public ActionResult Delete(int id)
        {
            Walk walk = _walkRepo.GetWalkById(id);
            return View(walk);
        }

        // POST: WalksController/Delete/5
        // POST: Walks/Delete/Id
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Walk walk)
        {
            try
            {
                _walkRepo.DeleteWalk(id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(walk);
            }
        }
    }
}
