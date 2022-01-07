using System.Windows.Forms;

namespace SpaceRovers.Entity.PlanetPlateau
{
    public class PlateauModel : TableLayoutPanel
    {
        #region MEMBERS
        public byte LastRoverNumber { get; set; }

        public SpaceRoversOnPlateau RoversOnPlateau { get; set; }
        #endregion

        #region CONSTRUNCTORS
        public PlateauModel(byte rowCount, byte columnCount)
        {
            this.RowCount = rowCount;
            this.ColumnCount = columnCount;

            this.RoversOnPlateau = new SpaceRoversOnPlateau();
        }
        #endregion

    }
}
