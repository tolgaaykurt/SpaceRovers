using SpaceRover.Logging;
using SpaceRovers.Entity.Rover;
using SpaceRovers.Entity.Rover.Abstracts;
using System;

namespace SpaceRover.Business.States.Rover.Directions
{
    public abstract class RoverDirectionStateBase : IRoverDirectionState
    {
        #region MEMBERS
        protected readonly SpaceRoverModel Rover;
        #endregion

        #region CONSTRUCTORS
        public RoverDirectionStateBase(SpaceRoverModel rover)
        {
            this.Rover = rover;
        }
        #endregion

        #region METHODS
        public bool TurnLeft()
        {
            var isSuccess = false;

            try
            {
                this.ExecuteTurnLeft();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.AddLogToQueue($"Rover sola dönerken bir hata oluştu. Hata: {ex.Message}");
            }

            return isSuccess;
        }

        public bool TurnRight()
        {
            var isSuccess = false;

            try
            {
                this.ExecuteTurnRight();

                isSuccess = true;
            }
            catch (Exception ex)
            {
                Logger.AddLogToQueue($"Rover sağa dönerken bir hata oluştu. Hata: {ex.Message}");
            }

            return isSuccess;
        }

        protected abstract void ExecuteTurnLeft();

        protected abstract void ExecuteTurnRight();
        #endregion
    }
}
