using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Repositories;
using DogGo.Models;
using DogGo.Models.ViewModels;

namespace DogGo.Controllers
{
    public class WalkersController : Controller
    {
        private readonly IWalkerRepository _walkerRepo;
        private readonly IWalkRepository _walkRepo;
        private readonly IDogRepository _dogRepo;

        // ASP.NET will give us an instance of our Walker Repository. This is called "Dependency Injection"
        public WalkersController(
            IWalkerRepository walkerRepository, 
            IWalkRepository walkRepository,
            IDogRepository dogRepository)
        {
            _walkerRepo = walkerRepository;
            _walkRepo = walkRepository;
            _dogRepo = dogRepository;
        }

        // GET: WalkersController
        // GET: Walkers
        // Method gets all the walkers in the Walker table, convert it to a List and pass it off to the view.
        public ActionResult Index()
        {
            List<Walker> walkers = _walkerRepo.GetAllWalkers();

            return View(walkers);
        }

        // GET: WalkersController/Details/5
        // GET: Walkers/Details/{id}
        public ActionResult Details(int id)
        {
            Walker walker = _walkerRepo.GetWalkerById(id);
            List<Walk> walks = _walkRepo.GetAllWalks();
            Walk walk = _walkRepo.GetWalkById(id);
            //List<Walk> walks = _walkRepo.GetWalksByWalkerId(walker.Id);
            Dog dog = _dogRepo.GetDogById(id);

            WalkerProfileViewModel vm = new WalkerProfileViewModel()
            {
                Walker = walker,
                Walks = walks,
                Walk = walk,
                Dog = dog
            };

            if (walker == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // GET: WalkersController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: WalkersController/Create
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

        // GET: WalkersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalkersController/Edit/5
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

        // GET: WalkersController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalkersController/Delete/5
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
