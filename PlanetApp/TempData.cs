using System;
using PlanetLib;
namespace PlanetApp;

public class TempData
{
        public List<Island> Islands { get; set; } = new List<Island>();
        public List<Mainland> Mainlands { get; set; } = new List<Mainland>();
        public List<Ocean> Oceans { get; set; } = new List<Ocean>();

}
