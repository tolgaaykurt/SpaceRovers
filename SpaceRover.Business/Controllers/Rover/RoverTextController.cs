using SpaceRover.Business.Chains.Rover.TextCommand;
using SpaceRover.Entity.Chains.Rover.TextCommand;
using SpaceRover.Entity.Controllers.Rover;
using SpaceRover.Entity.Rover;
using SpaceRover.Logging;
using SpaceRovers.Entity.PlanetPlateau;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace SpaceRover.Business.Controllers.Rover
{
    public class RoverTextController
    {
        #region MEMBERS
        private readonly Panel PlateauPanelControl;

        public static PlateauModel Plateau;

        private Bitmap RoverImage;

        private bool IsTest = false;
        #endregion

        #region CONTRUCTORS
        public RoverTextController(Panel pnlPlateau, Bitmap roverImage)
        {
            this.PlateauPanelControl = pnlPlateau;
            this.RoverImage = roverImage;
        }

        public RoverTextController(Panel pnlPlateau, Bitmap roverImage, bool isTest): this(pnlPlateau, roverImage)
        {
            this.IsTest = isTest;
        }
        #endregion

        #region METHODS
        public RoverTextControllerResult Execute(string command)
        {
            var result = new RoverTextControllerResult();
            result.IsSuccess = false;

            try
            {
                ITextCommandHandler xycTextCommandHandler = new XYCTextCommandHandler(this.RoverImage, result.Messages, this.IsTest);
                ITextCommandHandler xyTextCommandHandler = new XYTextCommandHandler(this.PlateauPanelControl, result.Messages);
                ITextCommandHandler moveTextCommandHandler = new MoveTextCommandHandler(result.Messages, this.IsTest);
                ITextCommandHandler stopperTextCommandHandler = new StopperTextCommandHandler(result.Messages); // Eğer komut stopper'e kadar geldiyse bilinmeyen bir komut demektir.

                xycTextCommandHandler.NextHandler = xyTextCommandHandler;
                xyTextCommandHandler.NextHandler = moveTextCommandHandler;
                moveTextCommandHandler.NextHandler = stopperTextCommandHandler;

                xycTextCommandHandler.ProcessHandler(command);

                result.IsSuccess = true;
            }
            catch (Exception ex)
            {
                var message = $"Komutların yürütülmesi sırasında bir hata oluştu.";
                result.Messages.Add(new RoverMoveMessage("", message));
                Logger.AddSystemLogToQueue(message, ex.Message);
            }

            return result;
        }
        #endregion
    }
}
