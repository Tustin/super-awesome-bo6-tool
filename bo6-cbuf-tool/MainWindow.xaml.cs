using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using LibDebug;
using System.Linq;
using bo6_cbuf_tool.ViewModels;

namespace bo6_cbuf_tool
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static Socket? _socket;

        public static Debugger? ps4;

        public static Process? gameProcess;

        public bool fullBright = true;

        public KeyValuePair<string, string>? dvarList = new();

        private MainWindowViewModel _mainWindowViewModel = new MainWindowViewModel();


        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = _mainWindowViewModel;
        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            var ip = txtIp.Text;
            try
            {
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                _socket.ReceiveTimeout = 3000;
                _socket.SendTimeout = 3000;
                _socket.Connect(new IPEndPoint(IPAddress.Parse(ip), 9090));

                _socket.SendFile("ps4debug.bin");

                _socket.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAttach_Click(object sender, RoutedEventArgs e)
        {
            var ip = txtIp.Text;

            try
            {
                ps4 = new Debugger(IPAddress.Parse(ip));

                ps4.Connect();

                var processList = ps4.GetProcessList();

                gameProcess = processList.FindProcess("eboot.bin");

                if (gameProcess is null)
                {
                    MessageBox.Show("Unable to find game process. Make sure the game is running.");
                }

                //rpcStub = ps4.InstallRPC(gameProcess.pid);

                ps4.Notify(222, "Loaded");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            var command = txtCommand.Text;

            CBuf_AddText(command);
        }

        void CBuf_AddText(string text)
        {
            var length = text.Length;

            UIntPtr cmd_textArray = new UIntPtr(0x400000 + 0x4D6C350);

            int commandSize = ps4.ReadInt32(gameProcess.pid, cmd_textArray + 0x10004);

            int bufferSize = ps4.ReadInt32(gameProcess.pid, cmd_textArray + 0x10000);

            if (length + commandSize > bufferSize)
            {
                MessageBox.Show("Overflow");
                return;
            }

            ps4.WriteString(gameProcess.pid, cmd_textArray + (ulong)commandSize, text);

            ps4.WriteInt32(gameProcess.pid, cmd_textArray + 0x10004, ps4.ReadInt32(gameProcess.pid, cmd_textArray + 0x10004) + length);

            ps4.Notify(222, text + " executed");
        }


        void CallDvar(string dvarHash, object? value = null)
        {
            if (value == null)
            {
                CBuf_AddText("#x3" + dvarHash);
            }
            else
            {
                CBuf_AddText("#x3" + dvarHash + " " + value);
            }
        }

        private void btnStartLobby_Click(object sender, RoutedEventArgs e)
        {
            CBuf_AddText("xstartlobby");
        }

        private void btnSetMap_Click(object sender, RoutedEventArgs e)
        {
            var map = cbMap.Text;
            CBuf_AddText("#x3ef237da69bb64ef6 " + map);
        }

        private void btnGoPartyGo_Click(object sender, RoutedEventArgs e)
        {
            CBuf_AddText("xstartlobby;xpartygo");
        }

        private void btnCallDvar_Click(object sender, RoutedEventArgs e)
        {
            CallDvar(_mainWindowViewModel.SelectedDvar, _mainWindowViewModel.SelectedDvarValue);
        }

        private void btnFullBright_Click(object sender, RoutedEventArgs e)
        {
            fullBright = !fullBright;

            var fullBrightText = fullBright ? "1" : "0";

            CallDvar("704554F429DAB488", fullBrightText);
            CallDvar("53D347C4D236E028", fullBrightText);
            CallDvar("AD42CA33A427DE58", fullBrightText);
            CallDvar("8667C0BB90C5BFC3", fullBrightText);
            CallDvar("DF200A089A3B3FEB", fullBrightText);
        }

        //C00E244EA59D530E - third person
        //4871F220778A4649 - allow spectate
        // D58083F509013576 - fx

        //704554F429DAB488 => "sm_sunAllow",
        //53D347C4D236E028 => "sm_spotAllow",
        //AD42CA33A427DE58 => "sm_spotEnable",
        //8667C0BB90C5BFC3 => "sm_spotDistCull",
        //DF200A089A3B3FEB => "r_fog",


    }
}