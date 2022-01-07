using SpaceRover.Entity.Rover.Abstracts;
using System.Collections.Generic;

namespace SpaceRover.Entity.Controllers.Rover
{
    public class RoverTextControllerResult
    {
        public bool IsSuccess { get; set; }

        public List<IRoverMoveMessage> Messages { get; set; }

        public RoverTextControllerResult()
        {
            this.Messages = new List<IRoverMoveMessage>();
        }
    }
}
