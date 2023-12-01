using ChessGame.logic;

namespace ChessGame
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Board gameBoard = Board.GetBoard();
            gameBoard.DisplayBoard();
        }
    }
}