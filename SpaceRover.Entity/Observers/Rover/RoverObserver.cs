namespace SpaceRovers.Entity.Observers.Rover
{
    public abstract class RoverObserver
    {
        public abstract void OnStatusChange(SpaceRoverStatusChangeEventArgs roverStatusChangeEventArgs);
    }
}
