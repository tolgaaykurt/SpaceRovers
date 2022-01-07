namespace SpaceRover.Entity.Rover.Abstracts
{
    public interface IRoverMoveMessage
    {
        string RoverName { get; set; }

        string Message { get; set; }
    }
}
