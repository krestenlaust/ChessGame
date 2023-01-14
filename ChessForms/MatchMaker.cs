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
        LichessPlayerSeek,
        Bot,
        Network,
    }
    public enum GamemodeType
    {
        Classic = 0,
        Horde = 1,
        CheckMateTest = 2,
        PawnTest = 3,
        Tiny = 4,
    }

    public partial class MatchMaker : Form
    {
        public static bool PlaySoundOnMove;
        PlayerType white = PlayerType.Local;
        PlayerType black = PlayerType.Local;
        GamemodeType gamemodeType = GamemodeType.Classic;

        public MatchMaker()
        {
            InitializeComponent();
        }

        void InstantiateMatch(Player playerWhite, Player playerBlack)
        {
            Gamemode gamemode = CreateGamemode(gamemodeType, playerWhite, playerBlack);
            BoardDisplay board = new BoardDisplay(gamemode, white == PlayerType.Local, black == PlayerType.Local || black == PlayerType.LichessPlayer);
            board.Show();
        }

        void buttonStartMatch_Click(object sender, EventArgs e)
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

        Player CreatePlayer(PlayerType type, string name, TeamColor color)
        {
            switch (type)
            {
                case PlayerType.Local:
                    return new Player(name);
                case PlayerType.Bot:
                    return new SkakinatorPlayer(name, true);
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
                    return new LichessBotPlayer("lichess-player", "", textBoxBlackLichessMatchID.Text);
                case PlayerType.LichessPlayerSeek:
                    return new LichessBotPlayer("lichess-player", "", TeamColor.White);
                default:
                    return null;
            }
        }

        static Gamemode CreateGamemode(GamemodeType gamemodeType, Player playerWhite, Player playerBlack)
        {
            switch (gamemodeType)
            {
                case GamemodeType.Classic:
                    return new ClassicChess(playerWhite, playerBlack);
                case GamemodeType.Horde:
                    return new Horde(playerWhite, playerBlack);
                case GamemodeType.CheckMateTest:
                    return new CheckMateTest(playerWhite, playerBlack);
                case GamemodeType.PawnTest:
                    return new PawnTestChess(playerWhite, playerBlack);
                case GamemodeType.Tiny:
                    return new TinyChess(playerWhite, playerBlack);
            }

            return null;
        }

        void radioButtonWhiteNetworked_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as RadioButton).Checked;

            textBoxWhiteHost.Enabled = isChecked;

            white = PlayerType.Network;
        }

        void radioButtonWhiteLocal_CheckedChanged(object sender, EventArgs e)
        {
            white = PlayerType.Local;
        }

        void radioButtonBlackLocal_CheckedChanged(object sender, EventArgs e)
        {
            black = PlayerType.Local;
        }

        void radioButtonWhiteBot_CheckedChanged(object sender, EventArgs e)
        {
            white = PlayerType.Bot;
        }

        void radioButtonBlackBot_CheckedChanged(object sender, EventArgs e)
        {
            black = PlayerType.Bot;
        }

        void radioButtonBlackNetworked_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as RadioButton).Checked;

            textBoxBlackServerIP.Enabled = isChecked;

            black = PlayerType.Network;
        }

        void MatchMaker_Load(object sender, EventArgs e)
        {
            listBoxGamemode.Items.AddRange(Enum.GetNames(typeof(GamemodeType)));
            listBoxGamemode.SelectedIndex = 0;
            PlaySoundOnMove = checkBoxSoundOnMove.Checked;
        }

        void radioButtonBlackLichessPlayer_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = (sender as RadioButton).Checked;

            textBoxBlackLichessMatchID.Enabled = isChecked;

            black = PlayerType.LichessPlayer;
        }

        void listBoxGamemode_SelectedIndexChanged(object sender, EventArgs e)
        {
            gamemodeType = (GamemodeType)Enum.Parse(typeof(GamemodeType), (string)listBoxGamemode.SelectedItem);
        }

        void checkBoxSoundOnMove_CheckedChanged(object sender, EventArgs e)
        {
            PlaySoundOnMove = (sender as CheckBox).Checked;
        }

        void radioButtonWhiteLichessPlayer_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        void radioButtonWhiteDistributedComputing_CheckedChanged(object sender, EventArgs e)
        {
        }

        void radioButtonBlackLichessPlayerSeek_CheckedChanged(object sender, EventArgs e)
        {
            black = PlayerType.LichessPlayerSeek;
        }
    }
}
