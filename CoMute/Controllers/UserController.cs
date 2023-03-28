
using CoMute.Models;
using CoMute.Models.Dto;
using CoMute.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CoMute.Controllers
{
    public class UserController : Controller
    {
        private readonly CoMuteContext context;
        public UserController(CoMuteContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginRequest user)
        {
            //check if the required field are provided
            if (ModelState.IsValid)
            {
                //check if the passed user is assigned.
                if (user != null)
                {
                    //then get user details if the email and password match the user in the database.
                    var userDetails = context.Users.Where(u => u.Email == user.Email && u.Password == user.Password).ToList();
                    //check if the user exist
                    if (userDetails.Count() > 0)
                    {
                        //then assigned the user details to a static property
                        UserCarPoolViewModel.user = userDetails[0];
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        //use this approached because viewbag only works with method that have corresponding view
                        Response.WriteAsync("<script>alert('Please check your credentials and try again');</script>");
                    }
                }
            }
            return View("Index");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(User user)
        {
            //check if the required field are provided
            if (ModelState.IsValid)
            {
                //check if the passed user is assigned.
                if (user != null)
                {
                    //then add user to the database
                    context.Add(user);
                    context.SaveChanges();
                    return RedirectToAction("Index");
                }
            }
            return View();
        }
        [HttpGet]
        public IActionResult Profile()
        {
            //get an id from the current user
            var id = UserCarPoolViewModel.user.Id;
            //then get user details from the id
            var user = context.Users.Find(id);
            //then pass user details to the profile view.
            return View(user);
        }
        [HttpPost]
        public IActionResult Profile(User user)
        {
            //check if the required field are provided
            if (ModelState.IsValid)
            {
                //check if the passed user is assigned.
                if (user != null)
                {
                    //then update user in the database
                    context.Update(user);
                    context.SaveChanges();
                    ViewBag.Message = "Updated Information";
                   
                }
            }
            return Profile();
        }

        public IActionResult Logout()
        {
            //assigned current user to a new instance
            UserCarPoolViewModel.user = new User();
            return RedirectToAction("Index");
        }
    }
}
