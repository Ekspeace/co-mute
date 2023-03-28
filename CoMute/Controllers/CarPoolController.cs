using CoMute.Models;
using CoMute.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace CoMute.Controllers
{
    public class CarPoolController : Controller
    {
        private readonly CoMuteContext context;
        public CarPoolController(CoMuteContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(CarPool carPool)
        {
            if (ModelState.IsValid)
            {
                if (carPool != null)
                {
                    //check if the time of departure is greater than now
                    if (carPool.DepartureTime > DateTime.Now)
                    {
                        //check if the time of arrival is he time of departure
                        if (carPool.ArrivalTime > carPool.DepartureTime)
                        {
                            //check if the dates do not overlap -- if they overlap then the method return true, which then change to false  and dispay error message
                            if (!DateCreatedOverlap(carPool))
                            {
                                carPool.DateCreated = DateTime.Now;
                                carPool.Owner = UserCarPoolViewModel.user.Id;
                                context.Add(carPool);
                                context.SaveChanges();
                                return RedirectToAction("Search");
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "You cannot create a pool that overlap with another active pool";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "The Arrival time must always be greater that the Departure time";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "The Departure time must be greater than today";
                    }
                }
            }
            return View("Index");
        }
        [HttpGet]
        public IActionResult Search(string? searchString, string? sortOrder)
        {
            //------------------------------------------------------------------------------------API START----------------------------------------------------------------


            //var viewModel = new UserCarPoolViewModel();
            //using (var httpClient = new HttpClient())
            //{
            //    using (var response = await httpClient.GetAsync("https://localhost:44324/api/Carpool"))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //       viewModel.carPools = JsonConvert.DeserializeObject<List<CarPool>>(apiResponse);
            //    }
            //    using (var response = await httpClient.GetAsync("https://localhost:44324/api/User"))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //        viewModel.users = JsonConvert.DeserializeObject<List<User>>(apiResponse);
            //    }
            //    using (var response = await httpClient.GetAsync("https://localhost:44324/api/Opportunity"))
            //    {
            //        string apiResponse = await response.Content.ReadAsStringAsync();
            //        viewModel.opportunities = JsonConvert.DeserializeObject<List<CarPoolOpportunity>>(apiResponse);
            //    }
            //}

            //if (searchString != null) { viewModel.carPools = viewModel.carPools.Where(c => c.Destination.Contains(searchString) || c.Origin.Contains(searchString)).ToList(); }
            //else { viewModel.carPools = viewModel.carPools.ToList(); }

            //if (sortOrder != null){ viewModel.carPools = SortOrder(sortOrder, viewModel.carPools).ToList();}

            //return View(viewModel);

            //------------------------------------------------------------------------------------API END---------------------------------------------------------------------

            //these are viewbag which get sorting order
            ViewBag.OriginSortParm = String.IsNullOrEmpty(sortOrder) ? "origin_desc" : "origin";
            ViewBag.DestinationSortParm = String.IsNullOrEmpty(sortOrder) ? "destination_desc" : "destination";
            ViewBag.DepartureSortParm = sortOrder == "departure" ? "departure_desc" : "departure";
            ViewBag.ArrivalSortParm = sortOrder == "arrival" ? "arrival_desc" : "arrival";
            ViewBag.CreatedSortParm = sortOrder == "created" ? "created_desc" : "created";

            //view model that combine number of model to be used in a view
            var viewModel = new UserCarPoolViewModel();

            //check if the search string is assigned, then assign car pool list to a list with search results 
            if (searchString != null) { viewModel.carPools = context.Pools.Where(c => c.Destination.Contains(searchString) || c.Origin.Contains(searchString)).ToList(); }
            //else assign it to normal car pool list
            else { viewModel.carPools = context.Pools.ToList(); }

            //check if the sort string is assigned, then call SortOrder method with sort string and car pool list from above as parameters. which then returns a sorted car pool list.
            if(sortOrder != null) {
                viewModel.carPools = SortOrder(sortOrder, viewModel.carPools).ToList();
            }
            //get list of users from the database.
            viewModel.users = context.Users.ToList();
            //get a list of joined opportunities from the database.
            viewModel.opportunities = context.Opportunities.ToList();
            //then pass all the lists to the search view.
            return View(viewModel);
        }
        public IActionResult Edit(int id)
        {
            //check if the id is assigned
            if (id != 0)
            {
                //then get a car pool associated with the id from the database
                var carPool = context.Pools.Find(id);
                //then pass the car pool details to the edit view.
                return View(carPool);
            }
            else
            {
                return NotFound();
            }
        }
        [HttpPost]
        public IActionResult Edit(CarPool carPool)
        {
            //check if the required field are provided
            if (ModelState.IsValid)
            {
                if (carPool != null)
                {
                    //check if the time of departure is greater than now
                    if (carPool.DepartureTime > DateTime.Now)
                    {
                        //check if the time of arrival is he time of departure
                        if (carPool.ArrivalTime > carPool.DepartureTime)
                        {
                            //check if the dates do not overlap -- if they overlap then the method return true, which then change to false  and dispay error message
                            if (!DateCreatedOverlap(carPool))
                            {
                                carPool.Owner = UserCarPoolViewModel.user.Id;
                                context.Update(carPool);
                                context.SaveChanges();
                                return RedirectToAction("Search", "Home");
                            }
                            else
                            {
                                ViewBag.ErrorMessage = "You cannot create a pool that overlap with another active pool";
                            }
                        }
                        else
                        {
                            ViewBag.ErrorMessage = "The  Arrival time should always be greater that the Departure time";
                        }
                    }
                    else
                    {
                        ViewBag.ErrorMessage = "The Departure time must be greater than today";
                    }
                }
            }
            return Edit(carPool.Id);
        }
        public IActionResult Join(int id)
        {
            //check if the id is assigned
            if (id != 0)
            {
                //create an instance of join opportunities --- Best approach   var opportunity = new CarPoolOpportunity(id, userId, true, DateTime.Now)
                var opportunity = new CarPoolOpportunity();
                opportunity.CarPoolId = id;
                opportunity.UserId = UserCarPoolViewModel.user.Id;
                opportunity.Joined = true;
                opportunity.DateJoined = DateTime.Now;

                //get a car pool associated with the id from the database
                var carPool = context.Pools.Find(id);

                //check if the dates do not overlap -- if they overlap then the method return true, which then change to false  and dispay error message
                if (!DateJoinOverlap(carPool))
                {
                    //then one seats is removed from available seats.
                    carPool.AvailableSeats -= 1;
                    //making sure to only update what is been changed.
                    context.Entry(carPool).State = EntityState.Modified;
                   //then add the opportunity to joined opportunities.
                    context.Add(opportunity);
                    context.SaveChanges();
                }
                else
                {
                    //use this approached because viewbag only works with method that have corresponding view
                    Response.WriteAsync("<script>alert('You cannot join opportunities with overlapping time-frames');window.location = 'Search';</script>");
                }
            }
            return RedirectToAction("Search");
        }
        public IActionResult Leave(int id)
        {
            //check if the id is assigned
            if (id != 0)
            {
                //get joined opportunity of a specified car pool id and current user
                var opportunity = context.Opportunities.Where(o  => o.CarPoolId == id && o.UserId == UserCarPoolViewModel.user.Id).First();
                //get a car pool of a specified id
                var carPool = context.Pools.Find(id);
                //then one seats is added to available seats. 
                carPool.AvailableSeats += 1;
                //making sure to only update what is been changed.
                context.Entry(carPool).State = EntityState.Modified;
                //then delete the opportunity to joined opportunities.
                context.Remove(opportunity);
                context.SaveChanges();
            }
            return RedirectToAction("Search");
        }
        public IActionResult Delete(int id)
        {
            if (id != 0)
            {
                //get joined opportunity and car pool of a specified car pool id  
                var opportunity = context.Opportunities.Where(o => o.CarPoolId == id).First();
                var carPool = context.Pools.Find(id);

               //then delete both opportunity and car pool 
                context.RemoveRange(opportunity, carPool);
                context.SaveChanges();
            }
            return RedirectToAction("Search");
        }
        public bool DateCreatedOverlap(CarPool carPool)
        {
            var isOverlap = false;
            //get a car pool list of the current user except a car pool that is passed (used in edit)
            var pools = context.Pools.Where(p => p.Owner == UserCarPoolViewModel.user.Id && p.Id != carPool.Id).ToList();
            //loop through the list and check if the dates of car pools do not overlap with the new a car pool
            foreach (var pool in pools)
            {
                if ((carPool.DepartureTime.Date >= pool.DepartureTime.Date && carPool.DepartureTime.Date <= pool.ArrivalTime.Date) || (carPool.ArrivalTime.Date >= pool.DepartureTime.Date && carPool.ArrivalTime.Date <= pool.ArrivalTime.Date))
                {
                    isOverlap = true;
                    break;
                }
            }
            return isOverlap;
        }
        public bool DateJoinOverlap(CarPool carPool)
        {
            var isOverlap = false;
           //get a car pool list that exist in join opportunity table of the user sign in
            var pools = from p in context.Pools join o in context.Opportunities on p.Id equals o.CarPoolId where o.UserId ==  UserCarPoolViewModel.user.Id select p;
           //loop through the list and check if the dates of joined opportunities do not overlap with the new opportunity
            foreach (var pool in pools)
            {
                if ((carPool.DepartureTime.Date >= pool.DepartureTime.Date && carPool.DepartureTime.Date <= pool.ArrivalTime.Date) || (carPool.ArrivalTime.Date >= pool.DepartureTime.Date && carPool.ArrivalTime.Date <= pool.ArrivalTime.Date))
                {
                    isOverlap = true;
                    break;
                }
            }
            return isOverlap;
        }
        public IEnumerable<CarPool> SortOrder(string sortOrder, IEnumerable<CarPool> carPools)
        {
            switch (sortOrder)
            {
                case "origin_desc":
                    carPools = carPools.OrderByDescending(c => c.Origin).ToList();
                    break;
                case "destination_desc":
                    carPools = carPools.OrderByDescending(c => c.Destination).ToList();
                    break;
                case "origin":
                    carPools = carPools.OrderBy(c => c.Origin).ToList();
                    break;
                case "destination":
                    carPools = carPools.OrderBy(c => c.Destination).ToList();
                    break;
                case "arrival":
                    carPools = carPools.OrderBy(c => c.ArrivalTime).ToList();
                    break;
                case "arrival_desc":
                    carPools = carPools.OrderByDescending(c => c.ArrivalTime).ToList();
                    break;
                case "departure":
                    carPools = carPools.OrderBy(c => c.DepartureTime).ToList();
                    break;
                case "departure_desc":
                    carPools = carPools.OrderByDescending(c => c.DepartureTime).ToList();
                    break;
                case "created":
                    carPools = carPools.OrderBy(c => c.DateCreated).ToList();
                    break;
                case "created_desc":
                    carPools = carPools.OrderByDescending(c => c.DateCreated).ToList();
                    break;
                default:
                    break;
            }
            return carPools;
        }
    }
}
