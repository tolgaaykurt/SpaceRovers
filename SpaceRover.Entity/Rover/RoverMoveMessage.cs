using SpaceRover.Entity.Rover.Abstracts;

namespace SpaceRover.Entity.Rover
{
    public class RoverMoveMessage : IRoverMoveMessage
    {
        public string RoverName { get; set; }

        public string Message { get; set; }

        public RoverMoveMessage(string roverName, string message)
        {
            this.RoverName = roverName;
            this.Message = message;
        }
    }
}
