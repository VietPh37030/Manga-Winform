using System;
using System.Drawing;
using System.Windows.Forms;
using MangaReaderApp.Models;
using MangaReaderApp.Services;

namespace MangaReaderApp.Controls
{
    public class MangaCard : UserControl
    {
        private PictureBox _pbThumb;
        private Label _lblName;
        private Label _lblChapter;
        private Manga _manga;
        private OtruyenService _service;

        public event EventHandler<Manga> Clicked;

        public MangaCard(Manga manga)
        {
            _manga = manga;
            _service = new OtruyenService();
            InitializeComponent();
            LoadData();
        }

        private void InitializeComponent()
        {
            this.Size = new Size(160, 240);
            this.BackColor = Color.FromArgb(28, 35, 51); // Surface color
            this.Cursor = Cursors.Hand;
            this.Margin = new Padding(10);

            _pbThumb = new PictureBox
            {
                Size = new Size(160, 190),
                Location = new Point(0, 0),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.FromArgb(16, 20, 30)
            };
            _pbThumb.Click += (s, e) => Clicked?.Invoke(this, _manga);

            _lblName = new Label
            {
                Location = new Point(5, 195),
                Size = new Size(150, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Text = "Manga Name",
                AutoEllipsis = true
            };
            _lblName.Click += (s, e) => Clicked?.Invoke(this, _manga);

            _lblChapter = new Label
            {
                Location = new Point(5, 215),
                Size = new Size(150, 15),
                ForeColor = Color.FromArgb(176, 184, 196),
                Font = new Font("Segoe UI", 8, FontStyle.Regular),
                Text = "Chapter 0"
            };
            _lblChapter.Click += (s, e) => Clicked?.Invoke(this, _manga);

            this.Controls.Add(_pbThumb);
            this.Controls.Add(_lblName);
            this.Controls.Add(_lblChapter);
            
            this.Click += (s, e) => Clicked?.Invoke(this, _manga);
        }

        private void LoadData()
        {
            if (_manga == null) return;

            _lblName.Text = _manga.Name;
            
            if (_manga.ChaptersLatest != null && _manga.ChaptersLatest.Count > 0)
            {
                _lblChapter.Text = $"Latest: {_manga.ChaptersLatest[0].ChapterName}";
            }
            else
            {
                _lblChapter.Text = "Updating...";
            }

            string thumbUrl = _service.GetThumbUrl(_manga.ThumbUrl);
            if (!string.IsNullOrEmpty(thumbUrl))
            {
                try 
                {
                    _pbThumb.LoadAsync(thumbUrl);
                }
                catch { /* Ignore load errors */ }
            }
        }
    }
}
