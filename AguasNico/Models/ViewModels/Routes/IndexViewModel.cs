namespace AguasNico.Models.ViewModels.Routes
{
    public class IndexViewModel
    {
        public ApplicationUser User = null!;
        public IEnumerable<Route> Routes = new List<Route>();
    }
}
