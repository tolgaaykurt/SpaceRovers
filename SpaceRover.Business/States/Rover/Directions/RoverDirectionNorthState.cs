using SpaceRovers.Entity.Common;
using SpaceRovers.Entity.Rover;

namespace SpaceRover.Business.States.Rover.Directions
{
    public class RoverDirectionNorthState : RoverDirectionStateBase
    {
        public RoverDirectionNorthState(SpaceRoverModel rover) : base(rover) { }

        protected override void ExecuteTurnLeft()
        {
            this.Rover.RoverDirection = Enums.CompassEnum.West;
            this.Rover.RoverDirectionState = new RoverDirectionWestState(this.Rover);
        }

        protected override void ExecuteTurnRight()
        {
            this.Rover.RoverDirection = Enums.CompassEnum.East;
            this.Rover.RoverDirectionState = new RoverDirectionEastState(this.Rover);
        }
    }
}
