using CoMute.Models;
using CoMute.Models.Dto;
using CoMute.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace CoMute.Controllers
{
    public class HomeController : Controller
    {
        private readonly CoMuteContext context;
        public HomeController(CoMuteContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            var viewModel = new UserCarPoolViewModel();
            //get a list of car pools from the database.
            viewModel.carPools = context.Pools.ToList();
            //get a list of users from the database.
            viewModel.users = context.Users.ToList();
            //get a list of joined opportunities of the current user from the database.
            viewModel.opportunities = context.Opportunities.Where(o => o.UserId == UserCarPoolViewModel.user.Id).ToList();
            //then pass all the lists to the Joined opportunities view.
            return View(viewModel);
        }
        public IActionResult About()
        {
            return View();
        }
        public IActionResult Leave(int id)
        {
            //check if the id is assigned
            if (id != 0)
            {
                //get joined opportunity of a specified id
                var opportunity = context.Opportunities.Where(o => o.Id == id).First();
                //then get a car pool associated with the opportunity above.
                var carPool = context.Pools.Find(opportunity.CarPoolId);
                //then one seats is added to available seats. 
                carPool.AvailableSeats += 1;
                //making sure to only update what is been changed.
                context.Entry(carPool).State = EntityState.Modified;
                //then delete the opportunity to joined opportunities.
                context.Remove(opportunity);
                context.SaveChanges();
            }
            return RedirectToAction("Index"); ;
        }

    }
}