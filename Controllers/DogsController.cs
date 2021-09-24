using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Repositories;
using DogGo.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace DogGo.Controllers
{
    public class DogsController : Controller
    {
        private readonly IDogRepository _dogRepo;

        // ASP.NET will give an instance of Dog Repository, a.k.a. "Dependency Injection"
        public DogsController(IDogRepository dogRepository)
        {
            _dogRepo = dogRepository;
        }

        // GET: DogsController
        [Authorize]
        public ActionResult Index()
        {
            int ownerId = GetCurrentUserId();

            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(ownerId);

            return View(dogs);
        }

        // GET: DogsController/Details/5
        // GET: Dogs/Details/Id
        [Authorize]
        public ActionResult Details(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                return NotFound();
            }

            return View(dog);
        }

        // GET: DogsController/Create
        // GET: Dogs/Create
        // Creates a blank HTML form to be filled out with input from user with Create View:
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: DogsController/Create
        // POST: Dogs/Create
        [Authorize]
        [HttpPost] // Flag attribute informing app the kind of request it should handle
        [ValidateAntiForgeryToken] // Flag attribute informing app the kind of request it should handle
        public ActionResult Create(Dog dog)
        {
            try
            {
                // update the dogs OwnerId to the current user's Id and check if user is authenticated:
                if (dog.OwnerId == GetCurrentUserId())
                {
                    _dogRepo.AddDog(dog);
                    return RedirectToAction("Index");
                }
                return StatusCode(403);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(dog);
            }
        }

        // GET: DogsController/Edit/5
        // GET: Dogs/Edit/Id
        [Authorize]
        public ActionResult Edit(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);

            if (dog == null)
            {
                //return NotFound();
                return StatusCode(403);
            }

            return View(dog);
        }

        // POST: DogsController/Edit/5
        // POST: Dogs/Edit/Id
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Dog dog)
        {
            try
            {
                if (dog.OwnerId == GetCurrentUserId())
                {
                    _dogRepo.UpdateDog(dog);
                    return RedirectToAction("Index");
                }
                return StatusCode(403);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(dog);
            }
        }

        // GET: DogsController/Delete/5
        // GET: Dogs/Delete/Id
        // Create a view that asks the user to confirm the deletion:
        [Authorize]
        public ActionResult Delete(int id)
        {
            Dog dog = _dogRepo.GetDogById(id);
            return View(dog);
        }

        // POST: DogsController/Delete/5
        // POST: Dogs/Delete/Id
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Dog dog)
        {
            try
            {
                if (dog.OwnerId == GetCurrentUserId())
                {
                    _dogRepo.DeleteDog(id);
                    return RedirectToAction("Index");
                }
                return StatusCode(403);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(dog);
            }
        }

        // Method for getting current user by id:
        private int GetCurrentUserId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
    }
}
