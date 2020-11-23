using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using _06_NewDevTemplates.GrpcService;
using Grpc.Core;
using Grpc.Net.Client;

namespace _06_NewDevTemplates.WinForms
{
    public partial class MainForm : Form
    {
        private readonly PictureBox _draggableBox = new PictureBox();
        private Point _previousLocation;
        private readonly AsyncDuplexStreamingCall<StreamRequest, StreamResponse> _duplexStream;
        private readonly string _clientId = $"{Guid.NewGuid()}";
        private bool _isDragging;

        public MainForm()
        {
            InitializeComponent();

            Thread.Sleep(5000);

            _draggableBox.Left = 10;
            _draggableBox.Top = 10;
            _draggableBox.Width = 100;
            _draggableBox.Height = 100;
            _draggableBox.BackColor = Color.Red;
            _draggableBox.MouseDown += DraggableBox_MouseDown;
            _draggableBox.MouseMove += DraggableBox_MouseMove;
            Controls.Add(_draggableBox);
            Load += MainForm_Load;

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Streamer.StreamerClient(channel);

            _duplexStream = client.Do(new CallOptions());
            _duplexStream.RequestStream.WriteAsync(new StreamRequest { ClientId = _clientId });
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            await foreach (var message in _duplexStream.ResponseStream.ReadAllAsync())
            {
                _draggableBox.SetBounds(message.X, message.Y, 100, 100);
            }
        }

        private void DraggableBox_MouseDown(object sender, MouseEventArgs e)
        {
            _previousLocation = e.Location;
        }

        private async void DraggableBox_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_isDragging && e.Button == MouseButtons.Left)
            {
                _isDragging = true;
                int xOffset = e.X - _previousLocation.X;
                int yOffset = e.Y - _previousLocation.Y;
                _draggableBox.SetBounds(_draggableBox.Left + xOffset, _draggableBox.Top + yOffset, 100, 100);

                try
                {
                    await _duplexStream.RequestStream.WriteAsync(
                        new StreamRequest { X = _draggableBox.Left, Y = _draggableBox.Top, ClientId = _clientId });
                }
                finally
                {

                }

                _isDragging = false;
            }
        }
    }
}