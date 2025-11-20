using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using MangaReaderApp.Services;
using MangaReaderApp.Models;
using MangaReaderApp.Controls;
using System.Linq;

namespace MangaReaderApp.Views
{
    public class DetailView : UserControl
    {
        private OtruyenService _service;
        private PictureBox _pbCover;
        private Label _lblName;
        private Label _lblAuthor;
        private Label _lblStatus;
        private RichTextBox _txtDesc;
        private FlowLayoutPanel _pnlChapters;
        private LoadingControl _loading;
        private Button _btnBack;
        
        private Manga _currentManga; // Store for passing to Reader

        public event EventHandler BackClicked;
        public event EventHandler<(string ChapterId, Manga Manga)> ChapterClicked;

        public DetailView()
        {
            _service = new OtruyenService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(16, 20, 30);

            _btnBack = new Button
            {
                Text = "← Quay lại",
                Location = new Point(20, 10),
                Size = new Size(100, 30),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(28, 35, 51),
                Cursor = Cursors.Hand
            };
            _btnBack.FlatAppearance.BorderSize = 0;
            _btnBack.Click += (s, e) => BackClicked?.Invoke(this, EventArgs.Empty);

            _pbCover = new PictureBox
            {
                Location = new Point(20, 50),
                Size = new Size(200, 300),
                SizeMode = PictureBoxSizeMode.StretchImage,
                BackColor = Color.FromArgb(28, 35, 51)
            };

            _lblName = new Label
            {
                Location = new Point(240, 50),
                Size = new Size(500, 40),
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoEllipsis = true
            };

            _lblAuthor = new Label
            {
                Location = new Point(240, 100),
                Size = new Size(500, 20),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(176, 184, 196)
            };

            _lblStatus = new Label
            {
                Location = new Point(240, 125),
                Size = new Size(500, 20),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.FromArgb(59, 130, 246)
            };

            _txtDesc = new RichTextBox
            {
                Location = new Point(240, 160),
                Size = new Size(500, 190),
                BackColor = Color.FromArgb(16, 20, 30),
                ForeColor = Color.FromArgb(200, 200, 200),
                BorderStyle = BorderStyle.None,
                ReadOnly = true,
                Font = new Font("Segoe UI", 9)
            };

            var lblChaptersHeader = new Label
            {
                Text = "Danh sách chương",
                Location = new Point(20, 370),
                Size = new Size(200, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White
            };

            _pnlChapters = new FlowLayoutPanel
            {
                Location = new Point(20, 410),
                Size = new Size(740, 300),
                Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
                AutoScroll = true,
                BackColor = Color.FromArgb(28, 35, 51)
            };

            _loading = new LoadingControl
            {
                Location = new Point(350, 250),
                Visible = false
            };

            this.Controls.Add(_btnBack);
            this.Controls.Add(_pbCover);
            this.Controls.Add(_lblName);
            this.Controls.Add(_lblAuthor);
            this.Controls.Add(_lblStatus);
            this.Controls.Add(_txtDesc);
            this.Controls.Add(lblChaptersHeader);
            this.Controls.Add(_pnlChapters);
            this.Controls.Add(_loading);
        }

        public async void LoadManga(string slug)
        {
            _loading.Start();
            _loading.BringToFront();
            _pnlChapters.Controls.Clear();
            _pbCover.Image = null;

            try
            {
                var data = await _service.GetMangaDetailAsync(slug);
                if (data?.Item != null)
                {
                    _currentManga = data.Item;
                    var item = data.Item;
                    _lblName.Text = item.Name;
                    _lblAuthor.Text = "Tác giả: " + (item.Author != null ? string.Join(", ", item.Author) : "Chưa rõ");
                    _lblStatus.Text = "Trạng thái: " + item.Status;
                    _txtDesc.Text = item.Content?.Replace("<p>", "").Replace("</p>", "") ?? "Không có mô tả.";
                    
                    string thumbUrl = _service.GetThumbUrl(item.ThumbUrl);
                    if (!string.IsNullOrEmpty(thumbUrl)) _pbCover.LoadAsync(thumbUrl);

                    if (item.Chapters != null)
                    {
                        foreach (var server in item.Chapters)
                        {
                            foreach (var chap in server.ServerData)
                            {
                                var btn = new Button
                                {
                                    Text = $"Chương {chap.ChapterName}",
                                    Size = new Size(100, 40),
                                    Margin = new Padding(5),
                                    FlatStyle = FlatStyle.Flat,
                                    BackColor = Color.FromArgb(40, 48, 66),
                                    ForeColor = Color.White,
                                    Cursor = Cursors.Hand,
                                    Tag = chap.ChapterApiData
                                };
                                btn.FlatAppearance.BorderSize = 0;
                                
                                string chapId = GetIdFromUrl(chap.ChapterApiData);
                                btn.Click += (s, e) => ChapterClicked?.Invoke(this, (chapId, _currentManga));
                                _pnlChapters.Controls.Add(btn);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải chi tiết: " + ex.Message);
            }
            finally
            {
                _loading.Stop();
            }
        }

        private string GetIdFromUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return "";
            var parts = url.Split('/');
            return parts[parts.Length - 1];
        }
    }
}
