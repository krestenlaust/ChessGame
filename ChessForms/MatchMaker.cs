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

        private void buttonStartMatch_Click(object sender, EventArgs e)
        {
            if (black == PlayerType.NotSelected)
            {
                MessageBox.Show("You need to select a player to play black");
                return;
            }

            Player playerWhite = null;
            switch (white)
            {
                case PlayerType.Local:
                    playerWhite = new Player(textBoxWhiteName.Text);
                    break;
                case PlayerType.Bot:
                    playerWhite = new ChessGame.Bots.SimpletronBot().GeneratePlayer();
                    break;
                case PlayerType.Network:
                    if (!IPAddress.TryParse(textBoxWhiteHost.Text, out IPAddress ipaddress))
                    {
                        MessageBox.Show("Invalid IP Address...");
                        return;
                    }
                    TcpListener tcpListener = new TcpListener(ipaddress, 5050);
                    tcpListener.Start();

                    TcpClient tcpClient = tcpListener.AcceptTcpClient();
                    MessageBox.Show("Client connected");

                    NetworkedPlayer otherPlayer = new NetworkedPlayer(tcpClient.GetStream());
                    playerWhite = new Player(textBoxWhiteHost.Text);
                    playerWhite.onTurnStarted += otherPlayer.TurnStart;
                    break;
            }

            Player playerBlack = null;
            switch (black)
            {
                case PlayerType.Local:
                    playerBlack = new Player(textBoxWhiteName.Text);
                    break;
                case PlayerType.Bot:
                    playerBlack = new ChessGame.Bots.SimpletronBot().GeneratePlayer();
                    break;
                case PlayerType.Network:
                    if (!IPAddress.TryParse(textBoxBlackServerIP.Text, out IPAddress ipaddress))
                    {
                        MessageBox.Show("Invalid IP Address...");
                        return;
                    }

                    TcpClient tcpClient = new TcpClient();
                    tcpClient.Connect(ipaddress, 5050);

                    NetworkedPlayer otherPlayer = new NetworkedPlayer(tcpClient.GetStream());
                    playerBlack = new Player(textBoxBlackName.Text);
                    playerBlack.onTurnStarted += otherPlayer.TurnStart;
                    break;
            }

            BoardDisplay board = new BoardDisplay(playerWhite, playerBlack);
            board.Show();
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
    }
}
