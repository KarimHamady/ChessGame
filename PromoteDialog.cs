using ChessGame.Statics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChessGame
{
    public partial class PromoteDialog : Form
    {
        public PromoteDialog()
        {
            InitializeComponent();
            AddButtons();
        }

        public PieceType SelectedPieceType { get; private set; }

        private void AddButtons()
        {
            AddPieceTypeButton(PieceType.Rook);
            AddPieceTypeButton(PieceType.Knight);
            AddPieceTypeButton(PieceType.Bishop);
            AddPieceTypeButton(PieceType.Queen);

            // Handle form events as needed
            this.Text = "Promote to";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.ControlBox = false;
        }

        private void AddPieceTypeButton(PieceType pieceType)
        {
            var button = new Button
            {
                Text = Enum.GetName(typeof(Statics.PieceType), pieceType),
                Size = new Size(262, 30),
                Location = new Point(10, 10),  // Adjust button position
            };

            button.Click += (sender, e) =>
            {
                SelectedPieceType = pieceType;
                DialogResult = DialogResult.OK;
                Close();
            };

            Controls.Add(button);

            // Adjust vertical position for the next button
            int verticalSpacing = 35;
            foreach (Control control in Controls)
            {
                control.Location = new System.Drawing.Point(control.Location.X, control.Location.Y + verticalSpacing);
            }
        }
    }
}
