using System;
using System.Windows.Forms;
using System.IO;
using System.Drawing;


namespace LipSyncApp
{
    // Modified picture box with buttons
    class PictureBoxControl : UserControl 
    {
        public PictureBox PictureBox = new PictureBox();
        private OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
        public Button btn_ChangeImage = new Button();
        public Button btn_AddToTrack = new Button();

        public PictureBoxControl() : base()
        {
            this.openFileDialog1.DefaultExt = "png";
            this.openFileDialog1.Filter = "Image Files (*.jpg;*.jpeg;*.png;)|*.jpg;*.jpeg;*.png;";

            this.Size = new System.Drawing.Size(130, 150);
            this.PictureBox.Size = new System.Drawing.Size(105, 105);
            this.PictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
            this.PictureBox.BorderStyle = BorderStyle.Fixed3D;
            this.PictureBox.Margin = new Padding(10, 5, 10, 30);

            this.btn_ChangeImage.Text = "...";
            this.btn_ChangeImage.Size = new System.Drawing.Size(25, 25);
            this.btn_ChangeImage.Location = new System.Drawing.Point(this.PictureBox.Width - this.btn_ChangeImage.Width, this.PictureBox.Height + 10);

            this.btn_AddToTrack.Text = "+";
            this.btn_AddToTrack.Size = new System.Drawing.Size(75, 25);
            this.btn_AddToTrack.Location = new System.Drawing.Point(this.btn_ChangeImage.Location.X - this.btn_AddToTrack.Width - 5, this.PictureBox.Height + 10);

            this.btn_ChangeImage.Click += new System.EventHandler(ChangeImageButton_OpenFileDialog);
            this.Controls.Add(PictureBox);
            this.Controls.Add(btn_ChangeImage);
            this.Controls.Add(btn_AddToTrack);
        }

        public void ChangeImageButton_OpenFileDialog(Object sender, EventArgs args)
        {
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                try
                {
                    this.PictureBox.Image = new Bitmap(openFileDialog1.FileName);
                    this.PictureBox.ImageLocation = openFileDialog1.FileName;
                    this.PictureBox.Size = new Size(106, 106);
                }
                catch (IOException)
                {
                }
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // PictureBoxControl
            // 
            this.Name = "PictureBoxControl";
            this.ResumeLayout(false);

        }
    }
}
