using SpaceRover.Business.Adapters;
using SpaceRover.Business.States.Rover.Directions;
using SpaceRover.Entity.Rover;
using SpaceRover.Entity.Rover.Abstracts;
using SpaceRover.Logging;
using SpaceRovers.Entity.Common;
using SpaceRovers.Entity.Observers.Rover;
using SpaceRovers.Entity.PlanetPlateau;
using SpaceRovers.Entity.Rover;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace SpaceRover.Business.Rover
{
    public class SpaceRoverBusiness
    {
        private bool IsTest = false;

        public SpaceRoverBusiness(bool isTest)
        {
            this.IsTest = isTest;
        }

        public SpaceRoverBusiness()
        {
            this.IsTest = false;
        }

        #region METHODS
        public SpaceRoverModel CreateRover(byte landingColumn, byte landingRow, byte roverNumber, Bitmap roverImage, PlateauModel plateau)
        {
            var rover = new SpaceRoverModel(new SpaceRoverStatus() { Status = Enums.RoverStatusEnum.Idle }, plateau);

            try
            {
                var plateauPositionAdapter = new PlateauPositionAdapter(new TableLayoutPanelCellPosition { Column = landingColumn, Row = landingRow }, plateau.RowCount);
                rover.Position = new TableLayoutPanelCellPosition { Column = plateauPositionAdapter.Column, Row = plateauPositionAdapter.Row };

                rover.RoverDirection = Enums.CompassEnum.North;
                rover.RoverDirectionState = new RoverDirectionNorthState(rover);
                rover.RoverNumber = roverNumber;
                rover.Name = $"Rover{rover.RoverNumber}";

                rover.RoverStatus.Status = Enums.RoverStatusEnum.Idle;

                var imgRover = new PictureBox();
                imgRover.Dock = System.Windows.Forms.DockStyle.Top;
                imgRover.Image = roverImage;
                imgRover.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
                imgRover.TabIndex = 1;
                imgRover.TabStop = false;

                rover.lblRoverProgress = new Label();
                rover.lblRoverProgress.AutoSize = true;
                rover.lblRoverProgress.Dock = System.Windows.Forms.DockStyle.Bottom;

                rover.Text = rover.Name;
                rover.Dock = DockStyle.Fill;

                rover.Controls.Add(imgRover);
                rover.Controls.Add(rover.lblRoverProgress);
            }
            catch (Exception ex)
            {
                Logger.AddSystemLogToQueue("Rover üretilemedi.", ex.Message);
            }

            return rover;
        }


        /// <summary>
        /// Rover'ı verilen koordinatlara indirir.
        /// </summary>
        public bool LandRover(SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            var isLandingSuccessful = false;
            var message = string.Empty;

            try
            {
                if (this.CheckLandingArea(rover.Position.Row, rover.Position.Column, rover, messages) == true)
                {
                    rover.Plateau.Controls.Add(rover, rover.Position.Column, rover.Position.Row);
                    if (this.IsTest == false) this.SetInfo(rover.Position, rover);

                    rover.Plateau.LastRoverNumber = rover.RoverNumber;
                    isLandingSuccessful = true;

                    message = $"{rover.Text} platoya iniş yaptı.";
                }
            }
            catch (Exception ex)
            {
                message = $"İniş sırasında bir kaza yaşandı. Kaza Raporu: {ex.Message}";
            }

            if(string.IsNullOrWhiteSpace(message) == false)
            {
                messages.Add(new RoverMoveMessage(rover.Name, message));
                Logger.AddSystemLogToQueue(message);
            }

            return isLandingSuccessful;
        }

        /// <summary>
        /// Rover'ı 90 derece sola döndürür.
        /// </summary>
        public bool TurnLeft(SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            Func<bool> func = () =>
            {
                return rover.RoverDirectionState.TurnLeft();
            };

            return this.TurnWrapperFunc(func, "sola", rover, messages);
        }

        /// <summary>
        /// Rover'ı 90 derece sağa döndürür.
        /// </summary>
        public bool TurnRight(SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            Func<bool> func = () =>
            {
                return rover.RoverDirectionState.TurnRight();
            };

            return this.TurnWrapperFunc(func, "sağa", rover, messages);
        }

        /// <summary>
        /// Rover'ı yönü doğrultusunda ilerlet.
        /// </summary>        
        public bool MoveRover(SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            var isSuccess = false;

            switch (rover.RoverDirection)
            {
                case Enums.CompassEnum.East:
                    isSuccess = this.MoveToEast(rover, messages);
                    break;
                case Enums.CompassEnum.North:
                    isSuccess = this.MoveToNorth(rover, messages);
                    break;
                case Enums.CompassEnum.South:
                    isSuccess = this.MoveToSouth(rover, messages);
                    break;
                case Enums.CompassEnum.West:
                    isSuccess = this.MoveToWest(rover, messages);
                    break;
            }

            return isSuccess;
        }

        /// <summary>
        /// String olarak gönderilen toplu komut seti ile rover'ı hareket ettirmek, döndürmek için kullanılır. Verilen hareket komutları sırası ile uygulanır.
        /// </summary>
        /// <param name="textCommand">Rover'ın uygulayacağı hareket komut seti. Örneğin; LRRMMLLM</param>
        /// <returns></returns>
        public bool OperateRoverByTextCommandSet(string textCommand, SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            var isSuccess = false;

            rover.RoverStatus.Status = Enums.RoverStatusEnum.InAction;
            rover.RoverStatus.WaitingFor = null;

            this.NotifyForStatusChange(new SpaceRoverStatusChangeEventArgs { RoverStatus = rover.RoverStatus, RoverName = rover.Name }, rover);

            foreach (char command in textCommand)
            {
                switch (command)
                {
                    case 'L':
                        isSuccess = this.TurnLeft(rover, messages);
                        break;
                    case 'R':
                        isSuccess = this.TurnRight(rover, messages);
                        break;
                    case 'M':
                        isSuccess = this.MoveRover(rover, messages);
                        break;
                }

                if (isSuccess == false) break;
            }

            rover.RoverStatus.Status = Enums.RoverStatusEnum.Idle;
            rover.RoverStatus.WaitingFor = null;

            this.NotifyForStatusChange(new SpaceRoverStatusChangeEventArgs { RoverStatus = rover.RoverStatus, RoverName = rover.Name }, rover);

            return isSuccess;
        }

        /// <summary>
        /// Rover'ı doğu (east) yönünde hareket ettirir.
        /// </summary>
        internal bool MoveToEast(SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            Func<bool> moveFunc = () =>
            {
                var isSuccess = true;
                var roverPosition = rover.Plateau.GetCellPosition(rover);
                var message = string.Empty;

                if ((roverPosition.Column + 1) > rover.Plateau.ColumnCount)
                {
                    isSuccess = false;
                    message = $"{rover.Name} platonun sınırları dışına çıkıyordu. Sınır ihlali engellendi.";
                }

                if (isSuccess == true && this.CheckRoverDirection(roverPosition.Column + 1, roverPosition.Row, rover) == false)
                {
                    isSuccess = false;
                    message = $"{rover.Name} başka bir rover ile çarpışmak üzereydi. Kaza engellendi.";
                }

                if (isSuccess)
                {
                    roverPosition.Column++;

                    if (this.IsTest == true)
                    {
                        rover.Plateau.SetCellPosition(rover, roverPosition);
                    }
                    else
                    {
                        rover.Invoke(new Action(() => { rover.Plateau.SetCellPosition(rover, roverPosition); }));
                        this.SetInfo(roverPosition, rover);
                    }
                }

                if (string.IsNullOrWhiteSpace(message) == false)
                {
                    messages.Add(new RoverMoveMessage(rover.Name, message));
                    Logger.AddSystemLogToQueue(message);
                }

                return isSuccess;
            };

            return this.MoveWrapperFunc(moveFunc, rover, messages);
        }

        /// <summary>
        /// Rover'ı kuzey (north) yönünde hareket ettirir.
        /// </summary>
        internal bool MoveToNorth(SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            Func<bool> moveFunc = () =>
            {
                var isSuccess = true;
                var roverPosition = rover.Plateau.GetCellPosition(rover);
                var message = string.Empty;

                if ((roverPosition.Row - 1) < 0)
                {
                    isSuccess = false;
                    message = $"{rover.Name} platonun sınırları dışına çıkıyordu. Sınır ihlali engellendi.";

                    isSuccess = true;
                }

                if (isSuccess == true && this.CheckRoverDirection(roverPosition.Column, roverPosition.Row - 1, rover) == false)
                {
                    isSuccess = false;
                    message = $"{rover.Name} başka bir rover ile çarpışmak üzereydi. Kaza engellendi.";
                }

                if (isSuccess == true)
                {
                    roverPosition.Row--;

                    if (this.IsTest == true)
                    {
                        rover.Plateau.SetCellPosition(rover, roverPosition);
                    }
                    else
                    {
                        rover.Invoke(new Action(() => { rover.Plateau.SetCellPosition(rover, roverPosition); }));
                        this.SetInfo(roverPosition, rover);
                    }
                }

                if (string.IsNullOrWhiteSpace(message) == false)
                {
                    messages.Add(new RoverMoveMessage(rover.Name, message));
                    Logger.AddSystemLogToQueue(message);
                }

                return isSuccess;
            };

            return this.MoveWrapperFunc(moveFunc, rover, messages);
        }

        /// <summary>
        /// Rover'ı batı (west) yönünde hareket ettirir.
        /// </summary>
        internal bool MoveToWest(SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            Func<bool> moveFunc = () =>
            {
                var isSuccess = true;
                var roverPosition = rover.Plateau.GetCellPosition(rover);
                var message = string.Empty;

                if ((roverPosition.Column - 1) < 0)
                {
                    isSuccess = false;
                    message = $"{rover.Name} platonun sınırları dışına çıkıyordu. Sınır ihlali engellendi.";
                }

                if (isSuccess == true && this.CheckRoverDirection(roverPosition.Column - 1, roverPosition.Row, rover) == false)
                {
                    isSuccess = false;
                    message = $"{rover.Name} başka bir rover ile çarpışmak üzereydi. Kaza engellendi.";
                }

                if (isSuccess)
                {
                    roverPosition.Column--;

                    if (this.IsTest == true)
                    {
                        rover.Plateau.SetCellPosition(rover, roverPosition);
                    }
                    else
                    {
                        rover.Invoke(new Action(() => { rover.Plateau.SetCellPosition(rover, roverPosition); }));
                        this.SetInfo(roverPosition, rover);
                    }
                }

                if (string.IsNullOrWhiteSpace(message) == false)
                {
                    messages.Add(new RoverMoveMessage(rover.Name, message));
                    Logger.AddSystemLogToQueue(message);
                }

                return isSuccess;
            };

            return this.MoveWrapperFunc(moveFunc, rover, messages);
        }

        /// <summary>
        /// Rover'ı güney (south) yönünde hareket ettirir.
        /// </summary>
        internal bool MoveToSouth(SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            Func<bool> moveFunc = () =>
            {
                var isSuccess = true;
                var roverPosition = rover.Plateau.GetCellPosition(rover);
                var message = string.Empty;

                if ((roverPosition.Row + 1) >= rover.Plateau.RowCount)
                {
                    isSuccess = false;
                    message = $"{rover.Name} platonun sınırları dışına çıkıyordu. Sınır ihlali engellendi.";
                }

                if (isSuccess == true && this.CheckRoverDirection(roverPosition.Column, roverPosition.Row + 1, rover) == false)
                {
                    isSuccess = false;
                    message = $"{rover.Name} başka bir rover ile çarpışmak üzereydi. Kaza engellendi.";
                }

                if (isSuccess)
                {
                    roverPosition.Row++;

                    if (this.IsTest == true)
                    {
                        rover.Plateau.SetCellPosition(rover, roverPosition);
                    }
                    else
                    {
                        rover.Invoke(new Action(() => { rover.Plateau.SetCellPosition(rover, roverPosition); }));
                        SetInfo(roverPosition, rover);
                    }
                }

                if (string.IsNullOrWhiteSpace(message) == false)
                {
                    messages.Add(new RoverMoveMessage(rover.Name, message));
                    Logger.AddSystemLogToQueue(message);
                }

                return isSuccess;
            };

            return this.MoveWrapperFunc(moveFunc, rover, messages);
        }

        public void Attach(RoverObserver roverObserver, SpaceRoverModel rover)
        {
            rover.Observers.Add(roverObserver);
        }

        public void Detach(RoverObserver roverObserver, SpaceRoverModel rover)
        {
            rover.Observers.Remove(roverObserver);
        }

        internal void NotifyForStatusChange(SpaceRoverStatusChangeEventArgs roverStatusChangeEventArgs, SpaceRoverModel rover)
        {
            foreach (var roverObserver in rover.Observers)
            {
                roverObserver.OnStatusChange(roverStatusChangeEventArgs);
            }
        }
        #endregion

        #region HELPERS
        /// <summary>
        /// Rover'ın yön ve pozisyon bilgilerini ekranda gösterir.
        /// </summary>
        /// <param name="roverPosition"></param>
        private void SetInfo(TableLayoutPanelCellPosition roverPosition, SpaceRoverModel rover)
        {
            var plateauPositionAdapter = new PlateauPositionAdapter(new TableLayoutPanelCellPosition(roverPosition.Column, roverPosition.Row), rover.Plateau.RowCount);

            rover.Invoke(new Action(() => { rover.Text = rover.Name; }));
            rover.lblRoverProgress.Invoke(new Action(() => { rover.lblRoverProgress.Text = $"X={plateauPositionAdapter.Column} Y={plateauPositionAdapter.Row} {rover.RoverDirection}"; }));
        }

        /// <summary>
        /// Rover'ın iniş yapacağı alanda başka bir rover olup olmadığını kontrol eder.
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private bool CheckLandingArea(int row, int column, SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            var isSuccess = true;
            var message = string.Empty;

            if (row >= rover.Plateau.RowCount || column >= rover.Plateau.ColumnCount || row < 0 || column < 0)
            {
                isSuccess = false;
                message = "Platonun dışına rover indirilemez.";
            }

            if (isSuccess == true && rover.Plateau.GetControlFromPosition(column, row) != null)
            {
                isSuccess = false;
                message = "İniş yapılmak istenen alanda başka bir rover var.";
            }

            if(string.IsNullOrWhiteSpace(message) == false)
            {
                messages.Add(new RoverMoveMessage(rover.Name, message));
                Logger.AddSystemLogToQueue(message);
            }

            return isSuccess;
        }

        /// <summary>
        /// Rover'ın hareket etmek istediği yönde başka bir rover olup olmadığını kontrol eder. Böylece olası çarpışmalar önlenmiş olur.
        /// </summary>
        /// <returns></returns>
        protected bool CheckRoverDirection(int column, int row, SpaceRoverModel rover)
        {
            return (rover.Plateau.GetControlFromPosition(column, row) == null);
        }

        /// <summary>
        /// Hareket komutları için bir wrapper sağlar. Bu sayede ortak gereksinimler tek bir yerden yönetilebilir.
        /// </summary>
        /// <param name="func">Yapılacak harekete ait kodları içeren function.</param>
        /// <returns></returns>
        private bool MoveWrapperFunc(Func<bool> func, SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            var isSuccess = false;
            var message = string.Empty;

            Thread.Sleep(500);

            if (rover.Plateau.RoversOnPlateau.IsThereAnyRoverInAction == false || rover.Plateau.RoversOnPlateau.RoverNameWhichInAction == rover.Name /* Hareket eden bir rover yoksa yada hareket eden kendisi ise hareketi gerçekleştir */)
            {
                try
                {
                    isSuccess = func.Invoke();

                    if (isSuccess == true) message = $"{rover.Name} {rover.RoverDirection} yönüne ilerledi.";

                    isSuccess = true;
                }
                catch (Exception ex)
                {
                    message = $"{rover.Name} {rover.RoverDirection} yönüne hareket edemedi. [Kara Kutu]: {ex.Message}";
                }
                finally
                {
                    if (isSuccess == false) message = $"{rover.Name} {rover.RoverDirection} yönüne hareket edemedi.";
                }
            }
            else
            {
                message = $"{rover.Plateau.RoversOnPlateau.RoverNameWhichInAction} hareketini tamamlayana kadar {rover.Name} harekete başlayamaz.";
            }

            if(string.IsNullOrWhiteSpace(message) == false)
            {
                messages.Add(new RoverMoveMessage(rover.Name, message));
                Logger.AddSystemLogToQueue(message);
            }

            return isSuccess;
        }

        /// <summary>
        /// Dönüş komutları için bir wrapper sağlar. Bu sayede ortak gereksinimler tek bir yerden yönetilebilir.
        /// </summary>
        /// <param name="func">Yapılacak dönüşe ait kodları içeren function.</param>
        /// <param name="turningSideForLog">Log için dönüş yönü. Örneğin; "sağa", "sola".</param>
        /// <returns></returns>
        private bool TurnWrapperFunc(Func<bool> func, string turningSideForLog, SpaceRoverModel rover, IList<IRoverMoveMessage> messages)
        {
            var isSuccess = false;
            var message = string.Empty;

            Thread.Sleep(500);

            if (rover.Plateau.RoversOnPlateau.IsThereAnyRoverInAction == false || rover.Plateau.RoversOnPlateau.RoverNameWhichInAction == rover.Name /* Hareket eden bir rover yoksa yada hareket eden kendisi ise dönüşü gerçekleştir */)
            {
                try
                {
                    isSuccess = func.Invoke();

                    if (isSuccess == true)
                    {
                        if (this.IsTest == false) this.SetInfo(rover.Plateau.GetCellPosition(rover), rover);
                        message = $"{rover.Name} {turningSideForLog} ({rover.RoverDirection}) döndü.";
                    }
                }
                catch (Exception ex)
                {
                    message = $"{rover.Name} {turningSideForLog} ({rover.RoverDirection}) dönemedi. Seyir Defterine Not: {ex.Message}";
                }
                finally
                {
                    if (isSuccess == false) message = $"{rover.Name} {turningSideForLog} ({rover.RoverDirection}) dönemedi.";
                }
            }

            if (string.IsNullOrWhiteSpace(message) == false)
            {
                messages.Add(new RoverMoveMessage(rover.Name, message));
                Logger.AddSystemLogToQueue(message);
            }
            
            return isSuccess;
        }
        #endregion
    }
}
