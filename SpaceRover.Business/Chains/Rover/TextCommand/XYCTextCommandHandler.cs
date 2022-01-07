using SpaceRover.Business.Controllers.Rover;
using SpaceRover.Business.Rover;
using SpaceRover.Entity.Rover.Abstracts;
using SpaceRovers.Entity.Common;
using SpaceRovers.Entity.Observers.Rover;
using System.Collections.Generic;
using System.Drawing;

namespace SpaceRover.Business.Chains.Rover.TextCommand
{
    public class XYCTextCommandHandler : TextCommandHandlerBase
    {
        private SpaceRoverBusiness RoverBusiness;
        private Bitmap RoverImage;

        public XYCTextCommandHandler(Bitmap roverImage, List<IRoverMoveMessage> messages): base(messages)
        {
            this.RoverBusiness = new SpaceRoverBusiness();
            this.RoverImage = roverImage;
        }

        public XYCTextCommandHandler(Bitmap roverImage, List<IRoverMoveMessage> messages, bool isTest) : base(messages)
        {
            this.RoverBusiness = new SpaceRoverBusiness(isTest);
            this.RoverImage = roverImage;
        }

        public override void OnStatusChange(SpaceRoverStatusChangeEventArgs roverStatusChangeEventArgs)
        {
            // Rover'lar IsThereAnyRoverInAction property'sini kontrol etmeden harekete başlamadıkları için değerin ezilmesi konusunda endişelenmeye gerek yok.
            if (roverStatusChangeEventArgs.RoverStatus.Status == Enums.RoverStatusEnum.InAction)
            {
                RoverTextController.Plateau.RoversOnPlateau.IsThereAnyRoverInAction = true;
                RoverTextController.Plateau.RoversOnPlateau.RoverNameWhichInAction = roverStatusChangeEventArgs.RoverName;
            }
            else
            {
                RoverTextController.Plateau.RoversOnPlateau.IsThereAnyRoverInAction = false;
                RoverTextController.Plateau.RoversOnPlateau.RoverNameWhichInAction = null;
            }
        }

        public override void ProcessHandler(string textCommand)
        {
            if (textCommand.Length != 5 /* Gelen komut bu handler tarafından beklenen komut değilse, komut çalıştırılmak üzere sonraki handler'a aktarılıyor. */)
            {
                this.NextHandler?.ProcessHandler(textCommand);
            }
            else
            {
                XYC xyc;

                if (this.ValidateCommand(textCommand, out xyc) == true)
                {
                    RoverTextController.Plateau.LastRoverNumber++;

                    var rover = this.RoverBusiness.CreateRover(xyc.Column, xyc.Row, RoverTextController.Plateau.LastRoverNumber, this.RoverImage, RoverTextController.Plateau);
                    var landingIsSuccess = this.RoverBusiness.LandRover(rover, this.Messages);

                    if (landingIsSuccess == true)
                    {
                        this.RoverBusiness.Attach(this, rover);

                        RoverTextController.Plateau.RoversOnPlateau.Rovers.Add(rover);
                    }
                }
                else
                {
                    this.NextHandler?.ProcessHandler(textCommand);
                }
            }
        }

        private bool ValidateCommand(string textCommand, out XYC xyc)
        {
            var isValidated = false;
            xyc = new XYC();
            byte column = 0, row = 0;
            var directions = new List<string>() { "N", "W", "S", "E" };

            var piecesOfCommand = textCommand.Split(' ');

            isValidated =
                piecesOfCommand.Length == 3 &&
                byte.TryParse(piecesOfCommand[0], out column) &&
                byte.TryParse(piecesOfCommand[1], out row) &&
                directions.Contains(piecesOfCommand[2]);

            if (isValidated)
            {
                xyc.Row = row;
                xyc.Column = column;
                xyc.Direction = directions[2];
            }

            return isValidated;
        }

        class XYC
        {
            public byte Row { get; set; }

            public byte Column { get; set; }

            public string Direction { get; set; }
        }
    }
}
