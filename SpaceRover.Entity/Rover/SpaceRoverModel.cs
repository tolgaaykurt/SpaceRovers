using SpaceRovers.Entity.Common;
using SpaceRovers.Entity.Observers.Rover;
using SpaceRovers.Entity.PlanetPlateau;
using SpaceRovers.Entity.Rover.Abstracts;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SpaceRovers.Entity.Rover
{
    public class SpaceRoverModel : GroupBox
    {
        #region MEMBERS
        public readonly PlateauModel Plateau;
        public Label lblRoverProgress;
        public readonly IList<RoverObserver> Observers;

        /// <summary>
        /// State object of direction of rover.
        /// </summary>
        public IRoverDirectionState RoverDirectionState { get; set; }

        public TableLayoutPanelCellPosition Position { get; set; }

        /// <summary>
        /// Direction of rover.
        /// </summary>
        public Enums.CompassEnum RoverDirection { get; set; }

        public byte RoverNumber { get; set; }

        /// <summary>
        /// Rover'ın hareket ve hareket edebilirlik durumunu tutar.
        /// </summary>
        public SpaceRoverStatus RoverStatus { get; private set; }
        #endregion

        #region CONSTRUCTORS
        public SpaceRoverModel(SpaceRoverStatus roverStatus, PlateauModel plateau)
        {
            this.RoverStatus = roverStatus;
            this.Plateau = plateau;
            this.Observers = new List<RoverObserver>();
        }
        #endregion

        #region EVENTS
        /// <summary>
        /// Rover E, N, W, S yönlerinde harekete başladığında yada dönüş yaptığında yada hareketi bitirdiğinde tetiklenir.
        /// </summary>
        public event EventHandler RoverMovingStatusChanged;
        #endregion
    }
}
