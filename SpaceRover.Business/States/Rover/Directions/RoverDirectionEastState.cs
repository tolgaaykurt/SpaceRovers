using SpaceRovers.Entity.Common;
using SpaceRovers.Entity.Rover;

namespace SpaceRover.Business.States.Rover.Directions
{
    public class RoverDirectionEastState : RoverDirectionStateBase
    {
        public RoverDirectionEastState(SpaceRoverModel rover) : base(rover) { }

        protected override void ExecuteTurnLeft()
        {
            this.Rover.RoverDirection = Enums.CompassEnum.North;
            this.Rover.RoverDirectionState = new RoverDirectionNorthState(base.Rover);
        }

        protected override void ExecuteTurnRight()
        {
            this.Rover.RoverDirection = Enums.CompassEnum.South;
            this.Rover.RoverDirectionState = new RoverDirectionSouthState(base.Rover);
        }
    }
}
