namespace SpaceRover.Entity.Chains.Rover.TextCommand
{
    public interface ITextCommandHandler
    {
        ITextCommandHandler NextHandler { get; set; }

        void ProcessHandler(string textCommand);
    }
}
