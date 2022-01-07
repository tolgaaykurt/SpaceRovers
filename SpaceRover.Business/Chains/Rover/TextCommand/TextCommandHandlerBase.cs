using SpaceRover.Entity.Chains.Rover.TextCommand;
using SpaceRover.Entity.Rover.Abstracts;
using SpaceRovers.Entity.Observers.Rover;
using System.Collections.Generic;

namespace SpaceRover.Business.Chains.Rover.TextCommand
{
    public abstract class TextCommandHandlerBase : RoverObserver, ITextCommandHandler
    {
        protected IList<IRoverMoveMessage> Messages { get; set; }

        public ITextCommandHandler NextHandler { get; set; }

        public TextCommandHandlerBase(IList<IRoverMoveMessage> messages)
        {
            this.Messages = messages;
        }

        public abstract void ProcessHandler(string textCommand);
    }
}
