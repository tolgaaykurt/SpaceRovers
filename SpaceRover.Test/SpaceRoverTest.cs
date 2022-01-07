using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpaceRover.Business.Controllers.Rover;
using SpaceRover.Entity.Rover.Abstracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpaceRover.Test
{
    [TestClass]
    public class SpaceRoverTest
    {
        /// <summary>
        /// Rover'ın plato dışına çıkma girişimi testi. Rover plato dışına çıkamamalı.
        /// </summary>
        [TestMethod]
        public void Border_Violation_Test()
        {
            // Arrange
            var textCommandController = new RoverTextController(new System.Windows.Forms.Panel(), new System.Drawing.Bitmap(10, 10), true);
            var messages = new List<IRoverMoveMessage>();

            // Action
            messages.AddRange(textCommandController.Execute("5 9").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("0 0 N").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("LM").Messages as List<IRoverMoveMessage>);

            // Assert
            Assert.IsTrue(messages.Where(message => message.Message.Contains("sınır")).Count() > 0);
        }

        /// <summary>
        /// Rover'ların birbirlerine çarpabilme testi. Çarpışma gerçekleşmemeli.
        /// </summary>
        [TestMethod]
        public void Crash_Test()
        {
            // Arrange
            var textCommandController = new RoverTextController(new System.Windows.Forms.Panel(), new System.Drawing.Bitmap(10, 10), true);
            var messages = new List<IRoverMoveMessage>();

            // Action
            messages.AddRange(textCommandController.Execute("5 9").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("0 0 N").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("RM").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("2 0 N").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("LM").Messages as List<IRoverMoveMessage>);

            // Assert
            Assert.IsTrue(messages.Where(message => message.Message.Contains("çarpışma")).Count() > 0);
        }

        /// <summary>
        /// Rover'ların birbirlerini beklemelerini simüle ediyor. Bir rover hareketini tamamlamadan başka bir rover harekete başlayamaz.
        /// </summary>
        [TestMethod]
        public void Waiting_For_Another_Rover_Test()
        {
            // Arrange
            var textCommandController = new RoverTextController(new System.Windows.Forms.Panel(), new System.Drawing.Bitmap(10, 10), true);
            var messages = new List<IRoverMoveMessage>();
            
            // Action
            messages.AddRange(textCommandController.Execute("5 9").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("0 0 N").Messages as List<IRoverMoveMessage>);
            var rover1Task = Task.Run(() =>
            {
                messages.AddRange(textCommandController.Execute("RLRLRLMMRMMMLMM").Messages as List<IRoverMoveMessage>);
            });

            messages.AddRange(textCommandController.Execute("0 1 N").Messages as List<IRoverMoveMessage>);
            var rover2Task = Task.Run(() =>
            {
                messages.AddRange(textCommandController.Execute("MMRMMMMMMMMLMLM").Messages as List<IRoverMoveMessage>);
            });

            messages.AddRange(textCommandController.Execute("1 1 E").Messages as List<IRoverMoveMessage>);
            var rover3Task = Task.Run(() =>
            {
                messages.AddRange(textCommandController.Execute("MMLLMRMMMRMMMMMMMMLMLMRMM").Messages as List<IRoverMoveMessage>);
            });

            Task.WaitAll(new Task[] { rover1Task, rover2Task, rover3Task });

            // Assert
            Assert.IsTrue(messages.Any(message => message.Message.Contains("hareketini tamamlamasını bekliyor")));
        }

        /// <summary>
        /// Rover'ların üst üste inip inemediklerini test ediyor. Rover'lar birbirleri üzerine iniş yapamamalı.
        /// </summary>
        [TestMethod]
        public void Stacked_Rover_Test()
        {
            // Arrange
            var textCommandController = new RoverTextController(new System.Windows.Forms.Panel(), new System.Drawing.Bitmap(10, 10), true);
            var messages = new List<IRoverMoveMessage>();

            // Action
            messages.AddRange(textCommandController.Execute("5 9").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("0 0 N").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("0 0 N").Messages as List<IRoverMoveMessage>);

            // Assert
            Assert.IsTrue(messages.Any(message => message.Message.Contains("başka bir rover var")));
        }

        /// <summary>
        /// Rover'ın plato sınırları dışına iniş yapıp yapamadığını test eder. Rover plato sınırları dışına iniş yapamamalı.
        /// </summary>
        [TestMethod]
        public void Landing_Out_Of_Border_Test()
        {
            // Arrange
            var textCommandController = new RoverTextController(new System.Windows.Forms.Panel(), new System.Drawing.Bitmap(10, 10), true);
            var messages = new List<IRoverMoveMessage>();

            // Action
            messages.AddRange(textCommandController.Execute("1 1").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("2 2 N").Messages as List<IRoverMoveMessage>);

            // Assert
            Assert.IsTrue(messages.Any(message => message.Message.Contains("Platonun dışına")));
        }

        /// <summary>
        /// Rover'a tanımadığı bir komut gönderildiğinde stabilitesini kaybetmeden diğer komutları çalıştırmaya devam edip edemediği testi. Rover'a bilmediği bir komut geldiğinde bu komutu atlayıp diğer komutlara devam edebilmeli.
        /// </summary>
        [TestMethod]
        public void Unknow_Command_Test()
        {
            // Arrange
            var textCommandController = new RoverTextController(new System.Windows.Forms.Panel(), new System.Drawing.Bitmap(10, 10), true);
            var messages = new List<IRoverMoveMessage>();

            // Action
            messages.AddRange(textCommandController.Execute("5 5").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("0 0 N").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("HepsiBurada").Messages as List<IRoverMoveMessage>);
            messages.AddRange(textCommandController.Execute("RMMLM").Messages as List<IRoverMoveMessage>);

            // Assert
            Assert.IsTrue(messages[0].Message.Contains("iniş yaptı"));
            Assert.IsTrue(messages[1].Message.Contains("bilinmeyen komut"));
            Assert.IsTrue(messages[2].Message.Contains("döndü"));
            Assert.IsTrue(messages[3].Message.Contains("ilerledi"));
            Assert.IsTrue(messages[4].Message.Contains("ilerledi"));
            Assert.IsTrue(messages[5].Message.Contains("döndü"));
            Assert.IsTrue(messages[6].Message.Contains("ilerledi"));
        }
    }
}
