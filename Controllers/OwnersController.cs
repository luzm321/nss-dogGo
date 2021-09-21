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
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;

        // ASP.NET will give an instance of Owner Repository, a.k.a. "Dependency Injection"
        public OwnersController(IOwnerRepository ownerRepository)
        {
            _ownerRepo = ownerRepository;
        }

        // GET: OwnersController
        // GET: Owners
        // Method gets all the owners in the Owner table, convert it to a List and pass it off to the view.
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }

        // GET: OwnersController/Details/5
        // GET: Owners/Details/5
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);

            if (owner == null)
            {
                return NotFound();
            }

            return View(owner);
        }

        // GET: OwnersController/Create
        // Creates a blank HTML form to be filled out with input from user with Create View:
        public ActionResult Create()
        {
            return View();
        }

        // POST: OwnersController/Create
        // POST: Owners/Create
        [HttpPost] // Flag attribute informing app the kind of request it should handle
        [ValidateAntiForgeryToken] // Flag attribute informing app the kind of request it should handle
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                return View(owner);
            }
        }

        // GET: OwnersController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: OwnersController/Edit/5
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

        // GET: OwnersController/Delete/5
        // GET: Owners/Delete/Id
        // Create a view that asks the user to confirm the deletion:
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            return View(owner);
        }

        // POST: OwnersController/Delete/5
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
