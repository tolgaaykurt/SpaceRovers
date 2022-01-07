using SpaceRover.Logging;
using SpaceRovers.Entity.PlanetPlateau;
using System;
using System.Windows.Forms;

namespace SpaceRover.Business.PlanetPlateau
{
    public class PlateauBussiness
    {
        #region METHODS
        public PlateauModel CreatePlateau(byte rowCount, byte columnCount)
        {
            var plateauModel = new PlateauModel(rowCount, columnCount);

            try
            {
                plateauModel.Name = "Plateau";
                plateauModel.AutoSize = true;
                plateauModel.Dock = DockStyle.Fill;
                plateauModel.BackColor = System.Drawing.SystemColors.Control;
                plateauModel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
                plateauModel.Padding = new System.Windows.Forms.Padding(5, 5, 5, 45);
                plateauModel.ColumnCount = columnCount;
                plateauModel.RowCount = rowCount;

                this.CreateColumns(plateauModel);
                this.CreateRows(plateauModel);

                plateauModel.LastRoverNumber = 0;
                plateauModel.RoversOnPlateau = new SpaceRoversOnPlateau();

                Logger.AddSystemLogToQueue($"Plato yaratıldı. X={plateauModel.ColumnCount} Y={plateauModel.RowCount}.");
            }
            catch (Exception ex)
            {
                Logger.AddSystemLogToQueue("Plato yaratılamadı.", ex.Message);
            }

            return plateauModel;
        }
        #endregion

        #region HELPER METHODS
        private void CreateColumns(PlateauModel plateauModel)
        {
            for (var columnIndex = 0; columnIndex < plateauModel.ColumnCount; columnIndex++)
            {
                plateauModel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, plateauModel.Width / plateauModel.ColumnCount));
            }
        }

        private void CreateRows(PlateauModel plateauModel)
        {
            for (var rowIndex = 0; rowIndex < plateauModel.RowCount; rowIndex++)
            {
                plateauModel.RowStyles.Add(new RowStyle(SizeType.Percent, plateauModel.Height / plateauModel.RowCount));
            }
        }
        #endregion
    }
}
