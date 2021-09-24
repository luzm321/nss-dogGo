using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DogGo.Repositories;
using DogGo.Models;
using DogGo.Models.ViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace DogGo.Controllers
{
    public class OwnersController : Controller
    {
        private readonly IOwnerRepository _ownerRepo;
        private readonly IDogRepository _dogRepo;
        private readonly IWalkerRepository _walkerRepo;
        private readonly INeighborhoodRepository _neighborhoodRepo;

        // ASP.NET will give an instance of Owner Repository, a.k.a. "Dependency Injection"
        public OwnersController(
            IOwnerRepository ownerRepository,
            IDogRepository dogRepository,
            IWalkerRepository walkerRepository,
            INeighborhoodRepository neighborhoodRepository)
        {
            _ownerRepo = ownerRepository;
            _dogRepo = dogRepository;
            _walkerRepo = walkerRepository;
            _neighborhoodRepo = neighborhoodRepository;
        }

        // Authenticating user when logging in:
        // Creates razor view template for login page:
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        // Look up owner by email address and save some of the data into a cookie to track/store data for authentication:
        // Add owner's Id, email address, and role. Then take that license/cookie and give it back to whoever made the request.
        // A cookie is a way to store data on a user's browser. After logging in, every time an owner makes a request to the server,
        // the browser will automatically send up the value of that cookie to authenticate. 
        public async Task<ActionResult> Login(LoginViewModel viewModel)
        {
            Owner owner = _ownerRepo.GetOwnerByEmail(viewModel.Email);

            if (owner == null)
            {
                return Unauthorized();
            }

            List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, owner.Id.ToString()),
        new Claim(ClaimTypes.Email, owner.Email),
        new Claim(ClaimTypes.Role, "DogOwner"),
    };

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity));

            return RedirectToAction("Index", "Dogs");
        }

        public async Task<ActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: OwnersController
        // GET: Owners
        // Method gets all the owners in the Owner table, convert it to a List and pass it off to the view.
        [Authorize]
        public ActionResult Index()
        {
            List<Owner> owners = _ownerRepo.GetAllOwners();

            return View(owners);
        }

        // GET: OwnersController/Details/5
        // GET: Owners/Details/Id
        [Authorize]
        public ActionResult Details(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            List<Dog> dogs = _dogRepo.GetDogsByOwnerId(owner.Id);
            List<Walker> walkers = _walkerRepo.GetWalkersInNeighborhood(owner.NeighborhoodId);

            if (owner == null)
            {
                return NotFound();
            }

            ProfileViewModel vm = new ProfileViewModel()
            {
                Owner = owner,
                Dogs = dogs,
                Walkers = walkers
            };

            return View(vm);
        }

        // GET: OwnersController/Create
        // GET: Owners/Create
        // Creates a blank HTML form to be filled out with input from user with Create View:
        public ActionResult Create()
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAllNeighborhoods();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = new Owner(),
                Neighborhoods = neighborhoods
            };

            return View(vm);
        }

        // POST: Owners/Create
        [HttpPost] // Flag attribute informing app the kind of request it should handle
        //  Writes a unique value to an HTTP-only cookie and then the same value is written to the form.
        //  When the page is submitted, an error is raised if the cookie value doesn't match the form value.
        // Prevents cross site request forgeries. That is, a form from another site that posts to your site
        // in an attempt to submit hidden content using an authenticated user's credentials.
        // The attack involves tricking the logged in user into submitting a form, or by simply programmatically
        // triggering a form when the page loads.
        [ValidateAntiForgeryToken]
        public ActionResult Create(Owner owner)
        {
            try
            {
                _ownerRepo.AddOwner(owner);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(owner);
            }
        }

        // GET: OwnersController/Edit/5
        // GET: Owners/Edit/{Id}
        public ActionResult Edit(int id)
        {
            List<Neighborhood> neighborhoods = _neighborhoodRepo.GetAllNeighborhoods();

            OwnerFormViewModel vm = new OwnerFormViewModel()
            {
                Owner = _ownerRepo.GetOwnerById(id),
                Neighborhoods = neighborhoods
            };

            if (vm == null)
            {
                return NotFound();
            }

            return View(vm);
        }

        // POST: OwnersController/Edit/5
        // POST: Owners/Edit/{Id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Owner owner)
        {
            try
            {
                _ownerRepo.UpdateOwner(owner);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(owner);
            }
        }

        // GET: OwnersController/Delete/5
        // GET: Owners/Delete/{Id}
        // Create a view that asks the user to confirm the deletion:
        public ActionResult Delete(int id)
        {
            Owner owner = _ownerRepo.GetOwnerById(id);
            return View(owner);
        }

        // POST: OwnersController/Delete/5
        // POST: Owners/Delete/{Id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Owner owner)
        {
            try
            {
                _ownerRepo.DeleteOwner(id);
                return RedirectToAction("Index");
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return View(owner);
            }
        }
    }
}
