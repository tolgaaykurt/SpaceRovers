using SpaceRover.Business.Controllers.Rover;
using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceRovers
{
    public partial class frmPlanetForTextCommands : Form
    {
        private RoverTextController RoverTextController;

        #region CONSTRUCTORS
        public frmPlanetForTextCommands()
        {
            InitializeComponent();

            this.RoverTextController = new RoverTextController(pnlPlateau, global::SpaceRovers.Properties.Resources.space_rover_png);
        }
        #endregion

        #region EVENTS
        private void btnWaitingForAnotherRoverTest_Click(object sender, EventArgs e)
        {
            this.Waiting_For_Another_Rover_Test();
        }

        private void brnCrashTest_Click(object sender, EventArgs e)
        {
            this.Crash_Test();
        }

        private void btnBorderViolation_Click(object sender, EventArgs e)
        {
            this.Border_Violation_Test();
        }

        private void btnMistakeCommand_Click(object sender, EventArgs e)
        {
            this.Mistake_Command_Test();
        }
        #endregion

        #region TEST METHODS
        /// <summary>
        /// Bir rover'ın platonun dışına çıkmaya çalışması durumu.
        /// </summary>
        private void Border_Violation_Test()
        {
            this.RoverTextController.Execute("9 5");
            this.RoverTextController.Execute("0 0 N");
            this.RoverTextController.Execute("LM");
        }

        /// <summary>
        /// Platoda hareket eden bir rover varken diğer rover'ların hareket halindeki rover'ın hareketlerini tamamlanmasını beklemeleri.
        /// </summary>
        private async void Waiting_For_Another_Rover_Test()
        {
            this.RoverTextController.Execute("9 5");
            this.RoverTextController.Execute("0 0 N");

            var simulateHardWork = Task.Run(() =>
            {
                this.RoverTextController.Execute("RLRLRLMMRMMMLMM");
            });

            this.RoverTextController.Execute("0 1 N");
            this.RoverTextController.Execute("MMRMMMMMMMMLMLM");

            this.RoverTextController.Execute("1 1 E");
            this.RoverTextController.Execute("MMLLMRMMMRMMMMMMMMLMLMRMM");

            await simulateHardWork;
        }

        /// <summary>
        /// Rover'ların çarpışma testi. 2 rover bir birlerine çarptırılmaya çalışılıyor.
        /// </summary>
        private void Crash_Test()
        {
            this.RoverTextController.Execute("9 5");
            this.RoverTextController.Execute("0 0 N");
            this.RoverTextController.Execute("RM");

            this.RoverTextController.Execute("2 0 N");
            this.RoverTextController.Execute("LM");
        }

        /// <summary>
        /// Rover'lara hatalı komut gönderiliyor.
        /// </summary>
        private void Mistake_Command_Test()
        {
            this.RoverTextController.Execute("9 5");
            this.RoverTextController.Execute("0 0 N");
            this.RoverTextController.Execute("95 BC"); // Hatalı komut.
            this.RoverTextController.Execute("RM");
        }
        #endregion
    }
}
