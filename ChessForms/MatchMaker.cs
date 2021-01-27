using ChessGame;
using ChessGame.Gamemodes;
using ChessGame.Players;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using ChessBots;

namespace ChessForms
{
    public enum PlayerType
    {
        Local,
        LichessPlayer,
        Bot,
        Network
    }
    public enum GamemodeType
    {
        Classic = 0,
        CheckMateTest = 1,
        PawnTest = 2,
        Tiny = 3,
    }

    public partial class MatchMaker : Form
    {
        private PlayerType white = PlayerType.Local;
        private PlayerType black = PlayerType.Local;
        private GamemodeType gamemodeType = GamemodeType.Classic;

        public MatchMaker()
        {
            InitializeComponent();
        }

        private void InstantiateMatch(Player playerWhite, Player playerBlack)
        {
            Gamemode gamemode = CreateGamemode(gamemodeType, playerWhite, playerBlack);
            BoardDisplay board = new BoardDisplay(gamemode, white == PlayerType.Local, black == PlayerType.Local || black == PlayerType.LichessPlayer);
            board.Show();
        }

        private void buttonStartMatch_Click(object sender, EventArgs e)
        {
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
                    return new Skakinator(name);
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
                case PlayerType.LichessPlayer:
                    return new LichessBotPlayer("lichess-player", "dH8TtzcSduH5qyJv", textBoxBlackLichessMatchID.Text);
                default:
                    return null;
            }
        }

        private static Gamemode CreateGamemode(GamemodeType gamemodeType, Player playerWhite, Player playerBlack)
        {
            switch (gamemodeType)
            {
                case GamemodeType.Classic:
                    return new ClassicChess(playerWhite, playerBlack);
                case GamemodeType.CheckMateTest:
                    return new CheckMateTest(playerWhite, playerBlack);
                case GamemodeType.PawnTest:
                    return new PawnTestChess(playerWhite, playerBlack);
                case GamemodeType.Tiny:
                    return new TinyChess(playerWhite, playerBlack);
            }

            return null;
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
            listBoxGamemode.Items.AddRange(Enum.GetNames(typeof(GamemodeType)));
            listBoxGamemode.SelectedIndex = 0;
        }

        private void radioButtonBlackLichessPlayer_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as RadioButton).Checked;

            textBoxBlackLichessMatchID.Enabled = isChecked;

            black = PlayerType.LichessPlayer;
        }

        private void listBoxGamemode_SelectedIndexChanged(object sender, EventArgs e)
        {
            gamemodeType = (GamemodeType)Enum.Parse(typeof(GamemodeType), (string)listBoxGamemode.SelectedItem);
        }
    }
}
