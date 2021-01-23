using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ChessGame;
using ChessGame.Bots;
using ChessGame.Gamemodes;

namespace ChessForms
{
    public enum PlayerType
    {
        Local,
        Bot,
        Network,
        NotSelected
    }

    public partial class MatchMaker : Form
    {
        private PlayerType white = PlayerType.Local; 
        private PlayerType black = PlayerType.NotSelected;

        public MatchMaker()
        {
            InitializeComponent();
        }

        private void InstantiateMatch(Player playerWhite, Player playerBlack)
        {
            //playerBlack = new ChessGame.Players.LichessBotPlayer("lichess-bot", "46nn2kjrrGTFaMN4", "GVZ4hkf7");

            Gamemode gamemode = new ClassicChess(playerWhite, playerBlack);
            BoardDisplay board = new BoardDisplay(gamemode, white == PlayerType.Local, black == PlayerType.Local);
            board.Show();
        }

        private void buttonStartMatch_Click(object sender, EventArgs e)
        {
            if (black == PlayerType.NotSelected)
            {
                MessageBox.Show("You need to select a player to play black");
                return;
            }

            Player playerWhite = CreatePlayer(white, textBoxWhiteName.Text, TeamColor.White);
            Player playerBlack = CreatePlayer(black, textBoxBlackName.Text, TeamColor.Black);

            if (playerWhite is null || playerBlack is null)
            {
                MessageBox.Show("Unknown exception");
                return;
            }

            InstantiateMatch(playerWhite, playerBlack);
        }

        private Player CreatePlayer(PlayerType type, string name, TeamColor color)
        {
            switch (type)
            {
                case PlayerType.Local:
                    return new Player(name);
                case PlayerType.Bot:
                    return new SimpletronBot(name);
                case PlayerType.Network:
                    if (color == TeamColor.White)
                    {
                        if (!IPAddress.TryParse(textBoxWhiteHost.Text, out IPAddress ipaddress))
                        {
                            MessageBox.Show("Invalid IP Address...");
                            return null;
                        }
                        TcpListener tcpListener = new TcpListener(ipaddress, 5050);
                        tcpListener.Start();

                        TcpClient tcpClient = tcpListener.AcceptTcpClient();
                        MessageBox.Show("Client connected");

                        return new NetworkedPlayer(textBoxWhiteName.Text, tcpClient.GetStream());
                    }
                    else
                    {
                        if (!IPAddress.TryParse(textBoxBlackServerIP.Text, out IPAddress ipaddress))
                        {
                            MessageBox.Show("Invalid IP Address...");
                            return null;
                        }

                        TcpClient tcpClient = new TcpClient();
                        tcpClient.Connect(ipaddress, 5050);

                        return new NetworkedPlayer(name, tcpClient.GetStream());
                    }
                case PlayerType.NotSelected:
                    return null;
                default:
                    return null;
            }
        }

        private void radioButtonWhiteNetworked_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as RadioButton).Checked;

            textBoxWhiteHost.Enabled = isChecked;

            white = PlayerType.Network;
        }

        private void radioButtonWhiteLocal_CheckedChanged(object sender, EventArgs e)
        {
            white = PlayerType.Local;
        }

        private void radioButtonBlackLocal_CheckedChanged(object sender, EventArgs e)
        {
            black = PlayerType.Local;
        }

        private void radioButtonWhiteBot_CheckedChanged(object sender, EventArgs e)
        {
            white = PlayerType.Bot;
        }

        private void radioButtonBlackBot_CheckedChanged(object sender, EventArgs e)
        {
            black = PlayerType.Bot;
        }

        private void radioButtonBlackNetworked_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as RadioButton).Checked;

            textBoxBlackServerIP.Enabled = isChecked;

            black = PlayerType.Network;
        }

        private void MatchMaker_Load(object sender, EventArgs e)
        {

        }
    }
}
