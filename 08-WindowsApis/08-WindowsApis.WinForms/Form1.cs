using System;
using System.Drawing;
using System.Windows.Forms;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;

namespace _08_WindowsApis.WinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            // Initialize the webcam
            MediaCapture captureManager = new MediaCapture();
            await captureManager.InitializeAsync();

            ImageEncodingProperties imgFormat = ImageEncodingProperties.CreateJpeg();
            // create storage file in local app storage
            StorageFile file = await KnownFolders.CameraRoll.CreateFileAsync("TestPhoto.jpg",
                                           CreationCollisionOption.GenerateUniqueName);

            // take photo
            await captureManager.CapturePhotoToStorageFileAsync(imgFormat, file);

            var image = Image.FromFile(file.Path);
            pictureBox1.Image = image;
        }
    }
}