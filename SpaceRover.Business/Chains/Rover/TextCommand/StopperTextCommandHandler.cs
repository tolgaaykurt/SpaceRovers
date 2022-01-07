using SpaceRover.Entity.Rover;
using SpaceRover.Entity.Rover.Abstracts;
using SpaceRover.Logging;
using SpaceRovers.Entity.Observers.Rover;
using System.Collections.Generic;

namespace SpaceRover.Business.Chains.Rover.TextCommand
{
    /// <summary>
    /// Zincir için sonlandırıcı görevini üstlenir. Eper command işlenmek için buraya kadar geldiyse tanınmayan bir komut içeriyor demektir.
    /// </summary>
    public class StopperTextCommandHandler : TextCommandHandlerBase
    {
        public StopperTextCommandHandler(IList<IRoverMoveMessage> messages) : base(messages)
        {

        }

        public override void OnStatusChange(SpaceRoverStatusChangeEventArgs roverStatusChangeEventArgs)
        {
        }

        public override void ProcessHandler(string textCommand)
        {
            var message = $"{textCommand} bilinmeyen komut.";
            this.Messages.Add(new RoverMoveMessage("", message));
            Logger.AddSystemLogToQueue(message);
        }
    }
}
