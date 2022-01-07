using System;
using System.Windows.Forms;

namespace SpaceRover.Business.Adapters
{
    /// <summary>
    /// Plato alanını simüle etmek için kullanılan TableLayoutPanel kontrolünün koordinat sistemini X, Y koordinat sistemine uygun hale getirir.
    /// </summary>
    internal class PlateauPositionAdapter
    {
        #region MEMBERS
        private TableLayoutPanelCellPosition TableLayoutPanelCellPosition;
        private int RowCount;

        public int Row { get; set; }

        public int Column { get; set; }
        #endregion

        #region CONSTRUCTORS
        public PlateauPositionAdapter(TableLayoutPanelCellPosition tableLayoutPanelCellPosition, int rowCount)
        {
            this.ValidatePosition(tableLayoutPanelCellPosition.Row, tableLayoutPanelCellPosition.Column);

            this.TableLayoutPanelCellPosition = tableLayoutPanelCellPosition;
            this.RowCount = rowCount;

            this.AdjustPosition();
        }
        #endregion

        #region HELPER METHODS
        /// <summary>
        /// TableLayoutPanel kontrolünün koordinat sistemini X, Y koordinat sistemine göre ayarlar.
        /// </summary>
        private void AdjustPosition()
        {
            this.Column = this.TableLayoutPanelCellPosition.Column;
            this.Row = this.RowCount - (this.TableLayoutPanelCellPosition.Row + 1);
        }

        private void ValidatePosition(int row, int column)
        {
            if (row < 0)
            {
                throw new ArgumentOutOfRangeException("Y", "Y değeri için sağladığınız input'u kontrol ediniz.");
            }

            if (column < 0)
            {
                throw new ArgumentOutOfRangeException("X", "X değeri için sağladığınız input'u kontrol ediniz.");
            }
        }
        #endregion
    }
}
