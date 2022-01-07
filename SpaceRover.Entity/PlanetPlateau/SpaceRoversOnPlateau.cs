using SpaceRovers.Entity.Rover;
using System.Collections.Generic;

namespace SpaceRovers.Entity.PlanetPlateau
{
    public class SpaceRoversOnPlateau
    {
        public IList<SpaceRoverModel> Rovers;

        public string RoverNameWhichInAction { get; set; }

        public bool IsThereAnyRoverInAction { get; set; }

        public SpaceRoversOnPlateau()
        {
            this.Rovers = new List<SpaceRoverModel>();
            this.IsThereAnyRoverInAction = false;
        }
    }
}
