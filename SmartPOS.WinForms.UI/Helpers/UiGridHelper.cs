using System.Drawing;
using System.Windows.Forms;

namespace SmartPOS.WinForms.UI.Helpers
{
    internal static class UiGridHelper
    {
        public static void ApplyResponsiveStyle(DataGridView grid)
        {
            if (grid == null)
            {
                return;
            }

            grid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            grid.ColumnHeadersHeight = 38;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 252);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(22, 32, 72);
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9F, FontStyle.Bold);
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.ForeColor = Color.FromArgb(40, 50, 80);
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(90, 110, 200);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 251, 253);
            grid.DefaultCellStyle.Padding = new Padding(2, 4, 2, 4);
            grid.GridColor = Color.FromArgb(232, 235, 244);
            grid.RowTemplate.Height = 34;
        }
    }
}
