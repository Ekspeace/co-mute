using CoMute.Models;

namespace CoMute.ViewModels
{
    public class UserCarPoolViewModel
    {
        public static User? user = new User();
        public IEnumerable<CarPool> carPools { get; set; } = Enumerable.Empty<CarPool>();
        public IEnumerable<User> users { get; set; } = Enumerable.Empty<User>();
       
        public IEnumerable<CarPoolOpportunity> opportunities { get; set; } = Enumerable.Empty<CarPoolOpportunity>();
    }
}
