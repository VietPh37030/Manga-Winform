using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using MangaReaderApp.Services;
using MangaReaderApp.Models;
using MangaReaderApp.Controls;
using System.Collections.Generic;

namespace MangaReaderApp.Views
{
    public class ReaderView : UserControl
    {
        private OtruyenService _service;
        private HistoryService _historyService;
        private FlowLayoutPanel _pnlImages;
        private LoadingControl _loading;
        private Button _btnBack;
        private Button _btnPrev;
        private Button _btnNext;
private Button _btnHome;
        private Label _lblTitle;
        private Panel _bottomPanel;
        
        private string _currentChapterId;
        private string _prevChapterId;
        private string _nextChapterId;
        private Manga _currentManga; // Need to pass this for history

        public event EventHandler BackClicked;
public event EventHandler HomeClicked;

        public ReaderView()
        {
            _service = new OtruyenService();
            _historyService = new HistoryService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.Black;

            // Top Panel (Title + Close)
            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 40,
                BackColor = Color.FromArgb(20, 20, 20)
            };

            _btnBack = new Button
            {
                Text = "X Đóng",
                Location = new Point(10, 5),
                Size = new Size(80, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(50, 50, 50),
                Cursor = Cursors.Hand
            };
            _btnBack.FlatAppearance.BorderSize = 0;
            _btnBack.Click += (s, e) => BackClicked?.Invoke(this, EventArgs.Empty);

            _lblTitle = new Label
            {
                Text = "Đang tải...",
                ForeColor = Color.White,
                AutoSize = true,
                Location = new Point(100, 10),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            topPanel.Controls.Add(_btnBack);
            topPanel.Controls.Add(_lblTitle);

            // Bottom Panel (Nav)
            _bottomPanel = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 50,
                BackColor = Color.FromArgb(20, 20, 20)
            };


        // Updated button initialization with Home button
        _btnPrev = new Button
        {
            Text = "< Trước",
            Size = new Size(100, 35),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(59, 130, 246),
            Enabled = false,
            Anchor = AnchorStyles.None
        };
        _btnPrev.Click += (s, e) => LoadChapter(_prevChapterId, _currentManga);

        _btnNext = new Button
        {
            Text = "Sau >",
            Size = new Size(100, 35),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(59, 130, 246),
            Enabled = false,
            Anchor = AnchorStyles.None
        };
        _btnNext.Click += (s, e) => LoadChapter(_nextChapterId, _currentManga);

        _btnHome = new Button
        {
            Text = "Home",
            Size = new Size(100, 35),
            FlatStyle = FlatStyle.Flat,
            ForeColor = Color.White,
            BackColor = Color.FromArgb(59, 130, 246),
            Cursor = Cursors.Hand
        };
        _btnHome.Click += (s, e) => HomeClicked?.Invoke(this, EventArgs.Empty);

        // Center buttons with Home in middle
        _bottomPanel.Resize += (s, e) =>
        {
            int center = _bottomPanel.Width / 2;
            _btnPrev.Location = new Point(center - 110 - 55, 7);
            _btnHome.Location = new Point(center - 55, 7);
            _btnNext.Location = new Point(center + 55, 7);
        };

        _bottomPanel.Controls.Add(_btnPrev);
        _bottomPanel.Controls.Add(_btnHome);
        _bottomPanel.Controls.Add(_btnNext);

            _pnlImages = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                FlowDirection = FlowDirection.TopDown,
                WrapContents = false,
                BackColor = Color.Black,
                Padding = new Padding(0, 0, 0, 50)
            };
            
            _pnlImages.Resize += (s, e) => 
            {
                foreach(Control c in _pnlImages.Controls)
                {
                    if (c is PictureBox pb)
                    {
                        pb.Width = _pnlImages.Width - 40;
                    }
                }
            };

            _loading = new LoadingControl
            {
                Location = new Point(400, 300),
                Visible = false
            };

            this.Controls.Add(_loading);
            this.Controls.Add(_pnlImages);
            this.Controls.Add(_bottomPanel);
            this.Controls.Add(topPanel);
        }

        public async void LoadChapter(string chapterId, Manga manga)
        {
            if (string.IsNullOrEmpty(chapterId)) return;

            _currentChapterId = chapterId;
            _currentManga = manga;
            
            _pnlImages.Controls.Clear();
            _loading.Start();
            _loading.BringToFront();
            _lblTitle.Text = "Đang tải...";
            _btnPrev.Enabled = false;
            _btnNext.Enabled = false;

            try
            {
                var data = await _service.GetChapterDetailAsync(chapterId);
                if (data?.Item != null)
                {
                    var item = data.Item;
                    _lblTitle.Text = $"{item.ChapterTitle} - {item.ChapterName}";
                    
                    // Save History
                    if (_currentManga != null)
                    {
                        _historyService.AddToHistory(_currentManga, item.ChapterName, chapterId);
                    }

                    // Setup Nav
                    if (item.PrevChapter != null) _prevChapterId = item.PrevChapter.Id;
                    else _prevChapterId = null;

                    if (item.NextChapter != null) _nextChapterId = item.NextChapter.Id;
                    else _nextChapterId = null;

                    _btnPrev.Enabled = !string.IsNullOrEmpty(_prevChapterId);
                    _btnNext.Enabled = !string.IsNullOrEmpty(_nextChapterId);

                    string domain = data.DomainCdn;
                    string path = item.ChapterPath;

                    if (item.ChapterImage != null)
                    {
                        foreach (var img in item.ChapterImage)
                        {
                            string imgUrl = $"{domain}/{path}/{img.ImageFile}";
                            AddImage(imgUrl);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chapter: " + ex.Message);
            }
            finally
            {
                _loading.Stop();
            }
        }

        private void AddImage(string url)
        {
            var pb = new PictureBox
            {
                Width = _pnlImages.Width - 40,
                Height = 800, 
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black,
                Margin = new Padding(20, 10, 0, 10)
            };
            
            pb.LoadCompleted += (s, e) =>
            {
                if (pb.Image != null)
                {
                    float ratio = (float)pb.Image.Height / pb.Image.Width;
                    pb.Height = (int)(pb.Width * ratio);
                }
            };
            pb.LoadAsync(url);

            _pnlImages.Controls.Add(pb);
        }
    }
}
