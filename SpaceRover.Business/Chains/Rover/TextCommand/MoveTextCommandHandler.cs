using SpaceRover.Business.Controllers.Rover;
using SpaceRover.Business.Rover;
using SpaceRover.Entity.Rover;
using SpaceRover.Entity.Rover.Abstracts;
using SpaceRover.Logging;
using SpaceRovers.Entity.Common;
using SpaceRovers.Entity.Observers.Rover;
using SpaceRovers.Entity.Rover;
using System.Collections.Generic;
using System.Linq;

namespace SpaceRover.Business.Chains.Rover.TextCommand
{
    public class MoveTextCommandHandler : TextCommandHandlerBase
    {
        private string TextCommand;
        private SpaceRoverModel OperatingRover;
        private SpaceRoverBusiness RoverBusiness;

        public MoveTextCommandHandler(IList<IRoverMoveMessage> messages): base(messages)
        {
            this.RoverBusiness = new SpaceRoverBusiness();
        }

        public MoveTextCommandHandler(IList<IRoverMoveMessage> messages, bool isTest) : base(messages)
        {
            this.RoverBusiness = new SpaceRoverBusiness(isTest);
        }

        public override void OnStatusChange(SpaceRoverStatusChangeEventArgs roverStatusChangeEventArgs)
        {
            // Bizden önce platoya inen rover hareketini tamamladıktan sonra biz herekete geçiyoruz.
            if (roverStatusChangeEventArgs.RoverStatus.Status == Enums.RoverStatusEnum.Idle)
            {
                var message = $"{roverStatusChangeEventArgs.RoverName} hareketini tamamladı.";

                this.Messages.Add(new RoverMoveMessage(roverStatusChangeEventArgs.RoverName, message));
                Logger.AddSystemLogToQueue(message);

                this.RoverBusiness.OperateRoverByTextCommandSet(this.TextCommand, this.OperatingRover, this.Messages);
            }
        }

        public override void ProcessHandler(string textCommand)
        {
            this.TextCommand = textCommand;
            var operatingRover = RoverTextController.Plateau?.RoversOnPlateau.Rovers.LastOrDefault();

            if (this.ValidateCommand() == true)
            {
                if (operatingRover != default(SpaceRoverModel))
                {
                    this.OperatingRover = operatingRover;

                    if (RoverTextController.Plateau.RoversOnPlateau.IsThereAnyRoverInAction == true)
                    {
                        // Kendinden önce alana inen rover'ın hareketini tamamlamasını beklemek için gerekli işlemler.
                        var waitingForRoverNumber = RoverTextController.Plateau.RoversOnPlateau.Rovers.Count - 1; // 1 nolu rover'da bu case asla gerçekleşmeyeceği için ekstra validasyona gerek yok.
                        var waitingForRover = RoverTextController.Plateau.RoversOnPlateau.Rovers.Where(rover => rover.RoverNumber == waitingForRoverNumber).FirstOrDefault();

                        if (waitingForRover != default(SpaceRoverModel))
                        {
                            // Hareket halindeki rover hareketini tamamladıktan sonra biz harekete başlayacağız.
                            this.RoverBusiness.Attach(this, waitingForRover);

                            var message = $"{this.OperatingRover.Name} hareket edebilmek için {waitingForRover.Name}'in hareketini tamamlamasını bekliyor.";
                            this.Messages.Add(new RoverMoveMessage(this.OperatingRover.Name, message));
                            Logger.AddSystemLogToQueue(message);
                        }
                    }
                    else
                    {
                        this.RoverBusiness.OperateRoverByTextCommandSet(this.TextCommand, this.OperatingRover, this.Messages);
                    }
                }
            }
            else
            {
                this.NextHandler?.ProcessHandler(textCommand);
            }
        }

        private bool ValidateCommand()
        {
            var isValidated = true;
            var permitedCommand = new List<char> { 'M', 'L', 'R' };

            if (this.TextCommand.Length > 0)
            {
                foreach (char c in this.TextCommand)
                {
                    if (char.IsLetter(c) == false || permitedCommand.Contains(c) == false)
                    {
                        isValidated = false;
                        break;
                    }
                }
            }
            else
            {
                isValidated = false;
            }

            return isValidated;
        }
    }
}
