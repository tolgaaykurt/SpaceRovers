using SpaceRover.Business.Controllers.Rover;
using SpaceRover.Business.PlanetPlateau;
using SpaceRover.Entity.Rover.Abstracts;
using SpaceRovers.Entity.Observers.Rover;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpaceRover.Business.Chains.Rover.TextCommand
{
    /// <summary>
    /// "X Y" ("1 2" gibi) pattern'ine uyan komutları işler.
    /// </summary>
    public class XYTextCommandHandler : TextCommandHandlerBase
    {
        Panel PlateauPanel;

        public XYTextCommandHandler(Panel pnlPlateau, IList<IRoverMoveMessage> messages): base(messages)
        {
            this.PlateauPanel = pnlPlateau;
        }

        public override void OnStatusChange(SpaceRoverStatusChangeEventArgs roverStatusChangeEventArgs)
        {
        }

        public override void ProcessHandler(string textCommand)
        {
            if (textCommand.Length != 3 /* Gelen komut bu handler tarafından beklenen komut değilse, komut çalıştırılmak üzere sonraki handler'a aktarılıyor. */)
            {
                this.NextHandler?.ProcessHandler(textCommand);
            }
            else
            {
                XY xy;

                if (this.ValidatCommand(textCommand, out xy) == true)
                {
                    this.PlateauPanel.Controls.Clear();

                    var plateauBusiness = new PlateauBussiness();
                    var plateau = plateauBusiness.CreatePlateau(xy.Row, xy.Column);

                    this.PlateauPanel.Controls.Add(plateau);

                    RoverTextController.Plateau = plateau;
                }
                else
                {
                    this.NextHandler?.ProcessHandler(textCommand);
                }
            }
        }

        private bool ValidatCommand(string textCommand, out XY xy)
        {
            var isValidated = false;
            byte column = 0, row = 0;

            xy = new XY();
            var piecesOfCommand = textCommand.Split(' ');

            isValidated =
                piecesOfCommand.Length == 2 &&
                byte.TryParse(piecesOfCommand[0], out column) &&
                byte.TryParse(piecesOfCommand[1], out row);

            if (isValidated)
            {
                xy.Row = row;
                xy.Column = column;
            }

            return isValidated;
        }

        class XY
        {
            public byte Row { get; set; }

            public byte Column { get; set; }
        }
    }
}
