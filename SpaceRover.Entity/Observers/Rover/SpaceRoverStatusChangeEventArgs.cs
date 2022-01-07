using SpaceRovers.Entity.Rover;
using System;

namespace SpaceRovers.Entity.Observers.Rover
{
    public class SpaceRoverStatusChangeEventArgs : EventArgs
    {
        /// <summary>
        /// Rover'ın komut işleme durumunu verir.
        /// </summary>
        public SpaceRoverStatus RoverStatus { get; set; }

        /// <summary>
        /// Hareketi takip edilen rover'ın adı.
        /// </summary>
        public string RoverName { get; set; }
    }
}
