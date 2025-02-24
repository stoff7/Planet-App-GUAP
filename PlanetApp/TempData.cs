using System.Collections.ObjectModel;
using PlanetLib;
namespace PlanetApp;

public class TempData
{
        public ObservableCollection<Island> Islands { get; set; } = new ObservableCollection<Island>();
        public ObservableCollection<Mainland> Mainlands { get; set; } = new ObservableCollection<Mainland>();
        public ObservableCollection<Ocean> Oceans { get; set; } = new ObservableCollection<Ocean>();

}
