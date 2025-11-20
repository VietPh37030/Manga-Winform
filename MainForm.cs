using System;
using System.Drawing;
using System.Windows.Forms;
using MangaReaderApp.Views;
using MangaReaderApp.Models;

namespace MangaReaderApp
{
    public partial class MainForm : Form
    {
        private Panel _contentPanel;
        private HomeView _homeView;
        private DetailView _detailView;
        private ReaderView _readerView;
        // Search view to be added if requested, for now Home has lists.

        public MainForm()
        {
            InitializeComponent();
            SetupViews();
        }

        private void InitializeComponent()
        {
            this.Text = "Ứng dụng Đọc Truyện";
            this.Size = new Size(1024, 768);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(16, 20, 30);
            
            _contentPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(16, 20, 30)
            };

            this.Controls.Add(_contentPanel);
        }

        private void SetupViews()
        {
            // Init Home
            _homeView = new HomeView();
            _homeView.MangaSelected += OnMangaSelected;
            
            // Init Detail
            _detailView = new DetailView();
            _detailView.BackClicked += (s, e) => ShowView(_homeView);
            _detailView.ChapterClicked += OnChapterClicked;

            // Init Reader
            _readerView = new ReaderView();
            _readerView.BackClicked += (s, e) => ShowView(_detailView);
            _readerView.HomeClicked += (s, e) => ShowView(_homeView);

            // Start with Home
            ShowView(_homeView);
        }

        private void ShowView(UserControl view)
        {
            _contentPanel.Controls.Clear();
            view.Dock = DockStyle.Fill;
            _contentPanel.Controls.Add(view);
        }

        private void OnMangaSelected(object sender, Manga manga)
        {
            _detailView.LoadManga(manga.Slug);
            ShowView(_detailView);
        }

        private void OnChapterClicked(object sender, (string ChapterId, Manga Manga) args)
        {
            _readerView.LoadChapter(args.ChapterId, args.Manga);
            ShowView(_readerView);
        }
    }
}
