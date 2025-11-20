using System;
using System.Drawing;
using System.Windows.Forms;

namespace MangaReaderApp.Controls
{
    public class LoadingControl : UserControl
    {
        private Label _lblLoading;
        private System.Windows.Forms.Timer _timer;
        private int _dotCount = 0;

        public LoadingControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(200, 50);
            this.BackColor = Color.Transparent;

            _lblLoading = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.FromArgb(59, 130, 246), // Accent Blue
                Text = "Loading"
            };

            this.Controls.Add(_lblLoading);

            _timer = new System.Windows.Forms.Timer { Interval = 300 };
            _timer.Tick += (s, e) =>
            {
                _dotCount = (_dotCount + 1) % 4;
                _lblLoading.Text = "Loading" + new string('.', _dotCount);
            };
        }

        public void Start()
        {
            this.Visible = true;
            _timer.Start();
        }

        public void Stop()
        {
            _timer.Stop();
            this.Visible = false;
        }
    }
}
