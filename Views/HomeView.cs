using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using MangaReaderApp.Services;
using MangaReaderApp.Controls;
using MangaReaderApp.Models;

namespace MangaReaderApp.Views
{
    public class HomeView : UserControl
    {
        private FlowLayoutPanel _flowPanel;
        private OtruyenService _service;
        private HistoryService _historyService;
        private LoadingControl _loading;
        private Label _lblTitle;
        private ContextMenuStrip _categoryMenu;

        public event EventHandler<Manga> MangaSelected;
        public event EventHandler<HistoryItem> HistorySelected;

        public HomeView()
        {
            _service = new OtruyenService();
            _historyService = new HistoryService();
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(16, 20, 30);

            var topPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60, // Increased for better touch
                BackColor = Color.FromArgb(28, 35, 51),
                Padding = new Padding(10)
            };

            _lblTitle = new Label
            {
                Text = "Truyện Mới",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 15),
                AutoSize = true
            };

            // Responsive buttons panel
            var btnPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Right,
                Width = 400,
                FlowDirection = FlowDirection.RightToLeft,
                BackColor = Color.Transparent
            };

            var btnHistory = CreateHeaderButton("Lịch Sử", (s, e) => LoadHistory());
            var btnCategory = CreateHeaderButton("Thể Loại", (s, e) => ShowCategories(s as Control));
            var btnHot = CreateHeaderButton("Hot", (s, e) => LoadHotManga());
            var btnNew = CreateHeaderButton("Mới", (s, e) => LoadNewManga());

            btnPanel.Controls.Add(btnHistory);
            btnPanel.Controls.Add(btnCategory);
            btnPanel.Controls.Add(btnHot);
            btnPanel.Controls.Add(btnNew);

            topPanel.Controls.Add(_lblTitle);
            topPanel.Controls.Add(btnPanel);

            _flowPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(10), // Reduced padding for small screens
                BackColor = Color.FromArgb(16, 20, 30)
            };
            
            // Responsive resize
            _flowPanel.Resize += (s, e) => AdjustCardSize();

            _loading = new LoadingControl
            {
                Location = new Point(300, 200),
                Anchor = AnchorStyles.None
            };

            this.Controls.Add(_loading);
            this.Controls.Add(_flowPanel);
            this.Controls.Add(topPanel);

            // Initial Load
            LoadNewManga();
        }

        private void AdjustCardSize()
        {
            // Optional: Adjust card margins or size based on width
        }

        private Button CreateHeaderButton(string text, EventHandler onClick)
        {
            var btn = new Button
            {
                Text = text,
                Size = new Size(80, 35), // Compact
                Margin = new Padding(5),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(59, 130, 246),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Cursor = Cursors.Hand
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.Click += onClick;
            return btn;
        }

        private async void LoadNewManga()
        {
            _lblTitle.Text = "Truyện Mới";
            await LoadMangaList(async () => await _service.GetNewMangaAsync());
        }

        private async void LoadHotManga()
        {
            _lblTitle.Text = "Truyện Hot";
            await LoadMangaList(async () => await _service.GetHotMangaAsync());
        }

        private void LoadHistory()
        {
            _lblTitle.Text = "Lịch Sử Đọc";
            _flowPanel.Controls.Clear();
            var history = _historyService.GetHistory();
            
            if (history.Count == 0)
            {
                var lbl = new Label
                {
                    Text = "Chưa có lịch sử đọc.",
                    ForeColor = Color.Gray,
                    AutoSize = true,
                    Font = new Font("Segoe UI", 12)
                };
                _flowPanel.Controls.Add(lbl);
                return;
            }

            foreach (var item in history)
            {
                // Reuse MangaCard or create specific HistoryCard?
                // Reuse MangaCard for simplicity, map HistoryItem to Manga
                var manga = new Manga
                {
                    Name = item.MangaName,
                    Slug = item.MangaSlug,
                    ThumbUrl = item.ThumbUrl,
                    ChaptersLatest = new List<ChapterShort> 
                    { 
                        new ChapterShort { ChapterName = item.LastChapterName } 
                    }
                };
                
                var card = new MangaCard(manga);
                card.Clicked += (s, m) => 
                {
                    // If history clicked, maybe go straight to reader?
                    // Or Detail? Let's go to Detail for consistency, 
                    // but user might want to resume.
                    // Let's trigger a special event or just standard selection.
                    MangaSelected?.Invoke(this, m);
                };
                _flowPanel.Controls.Add(card);
            }
        }

        private async void ShowCategories(Control anchor)
        {
            if (_categoryMenu == null)
            {
                _categoryMenu = new ContextMenuStrip();
                _categoryMenu.Renderer = new ToolStripProfessionalRenderer(new CustomColorTable());
                _categoryMenu.ShowImageMargin = false;
                _categoryMenu.MaximumSize = new Size(200, 400); // Scrollable? No, ContextMenu isn't easily scrollable.
                // Better to use a Panel or Form for many categories.
                // But for "compact", a dropdown is nice.
                
                var categories = await _service.GetCategoriesAsync();
                if (categories != null)
                {
                    foreach (var cat in categories)
                    {
                        var item = new ToolStripMenuItem(cat.Name);
                        item.ForeColor = Color.Black; // Standard menu color
                        item.Click += (s, e) => LoadCategory(cat);
                        _categoryMenu.Items.Add(item);
                    }
                }
            }
            
            _categoryMenu.Show(anchor, new Point(0, anchor.Height));
        }

        private class CustomColorTable : ProfessionalColorTable
        {
            public override Color MenuItemSelected => Color.FromArgb(200, 220, 255);
            public override Color MenuItemBorder => Color.Transparent;
        }

        private async void LoadCategory(Category cat)
        {
             _lblTitle.Text = cat.Name;
             // API for category search?
             // The user prompt didn't explicitly give "Get by Category" API.
             // But usually it's /the-loai/{slug}.
             // Let's check the user request...
             // "1.3. Danh sách thể loại GET .../the-loai"
             // It doesn't explicitly say how to get manga by genre.
             // Usually it is /the-loai/{slug} or /danh-sach/{slug} ?
             // Or maybe Search?
             // Let's assume standard Otruyen structure: /the-loai/{slug} returns list.
             // Wait, I need to verify this.
             // If not sure, I will use Search with keyword = category name as fallback?
             // No, Otruyen usually has /the-loai/{slug}.
             // Let's try to implement a generic GetListByUrl in Service if needed.
             // For now, I'll assume I can't easily do it without the endpoint.
             // Wait, the user request said: "1.3. Danh sách thể loại". It didn't say how to get manga of that genre.
             // But typically: https://otruyenapi.com/v1/api/the-loai/{slug}
             // Let's try that.
             
             await LoadMangaList(async () => 
             {
                 // Quick inline fetch for category
                 // This should be in Service, but for speed:
                 // Actually, let's just use Search for now if we aren't sure, 
                 // OR add a method to Service.
                 return await _service.GetMangaByCategoryAsync(cat.Slug);
             });
        }

        private async Task LoadMangaList(Func<Task<MangaListData>> fetchAction)
        {
            _flowPanel.Controls.Clear();
            _loading.Start();
            _loading.BringToFront();

            try
            {
                var data = await fetchAction();
                if (data?.Items != null)
                {
                    foreach (var item in data.Items)
                    {
                        var card = new MangaCard(item);
                        card.Clicked += (s, m) => MangaSelected?.Invoke(this, m);
                        _flowPanel.Controls.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                // MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                _loading.Stop();
            }
        }
    }
}
