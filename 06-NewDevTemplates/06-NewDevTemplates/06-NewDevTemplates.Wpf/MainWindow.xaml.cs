using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using _06_NewDevTemplates.GrpcService;
using Grpc.Core;
using Grpc.Net.Client;

namespace _06_NewDevTemplates.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point _previousLocation;
        private readonly AsyncDuplexStreamingCall<StreamRequest, StreamResponse> _duplexStream;
        private readonly string _clientId = $"{Guid.NewGuid()}";
        private bool _isDragging;

        public MainWindow()
        {
            InitializeComponent();

            Thread.Sleep(5000);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Streamer.StreamerClient(channel);

            _duplexStream = client.Do(new CallOptions());
            _duplexStream.RequestStream.WriteAsync(new StreamRequest { ClientId = _clientId });
        }

        private async void MainWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            await foreach (var message in _duplexStream.ResponseStream.ReadAllAsync())
            {
                _draggableRectangle.SetValue(Canvas.LeftProperty, (double)message.X);
                _draggableRectangle.SetValue(Canvas.TopProperty, (double)message.Y);
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var position = e.GetPosition(_draggableRectangle);
            _previousLocation = position;
        }

        private async void UIElement_OnMouseMove(object sender, MouseEventArgs e)
        {
            var position = e.GetPosition(_draggableRectangle);

            if (!_isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                _isDragging = true;

                double xOffset = position.X - _previousLocation.X;
                double yOffset = position.Y - _previousLocation.Y;

                var left = (double)_draggableRectangle.GetValue(Canvas.LeftProperty);
                var top = (double)_draggableRectangle.GetValue(Canvas.TopProperty);

                _draggableRectangle.SetValue(Canvas.LeftProperty, left + xOffset);
                _draggableRectangle.SetValue(Canvas.TopProperty, top + yOffset);

                try
                {
                    await _duplexStream.RequestStream.WriteAsync(
                        new StreamRequest { X = (int)(left + xOffset), Y = (int)(top + yOffset), ClientId = _clientId });
                }
                finally
                {

                }
                
                _isDragging = false;
            }
        }
    }
}