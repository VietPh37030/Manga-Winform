# Manga Reader App - Ứng Dụng Đọc Truyện Tranh

## 📖 Giới Thiệu

**Manga Reader App** là một ứng dụng desktop được xây dựng bằng Windows Forms (.NET) cho phép người dùng đọc truyện tranh manga trực tuyến. Ứng dụng có giao diện thân thiện, hiện đại với theme tối và hỗ trợ đầy đủ các tính năng cần thiết để trải nghiệm đọc truyện mượt mà.

## ✨ Tính Năng Chính

- **Trang Chủ (Home View)**: Hiển thị danh sách các bộ truyện manga có sẵn
- **Trang Chi Tiết (Detail View)**: Xem thông tin chi tiết về từng bộ truyện và danh sách chương
- **Trang Đọc Truyện (Reader View)**: Đọc truyện với giao diện tối ưu
- **Lịch Sử Đọc**: Lưu trữ và quản lý lịch sử đọc truyện của người dùng
- **Giao Diện Tối**: Theme màu tối (Dark Theme) dễ chịu cho mắt
- **Điều Hướng Linh Hoạt**: Chuyển đổi giữa các màn hình dễ dàng

## 🏗️ Kiến Trúc Dự Án

Dự án được tổ chức theo mô hình phân tầng rõ ràng:

```
Manga-Winform/
├── Controls/          # Các custom controls tái sử dụng
├── Models/            # Các lớp dữ liệu (Data Models)
│   └── Manga.cs      # Model cho thông tin manga
├── Services/          # Các dịch vụ xử lý logic nghiệp vụ
├── Views/            # Các màn hình giao diện
│   ├── HomeView      # Màn hình trang chủ
│   ├── DetailView    # Màn hình chi tiết truyện
│   └── ReaderView    # Màn hình đọc truyện
├── MainForm.cs       # Form chính điều phối các View
├── Program.cs        # Entry point của ứng dụng
└── history.json      # File lưu lịch sử đọc truyện
```

## 🔧 Công Nghệ Sử Dụng

- **Framework**: .NET Windows Forms
- **Ngôn ngữ**: C# 
- **UI Framework**: Windows Forms với custom controls
- **Data Storage**: JSON (lưu trữ lịch sử)

## 💻 Cách Hoạt Động

### 1. Entry Point (Program.cs)
```csharp
Application.Run(new MainForm());
```
Ứng dụng khởi động từ `MainForm` - form chính làm container cho tất cả các view.

### 2. Main Form (MainForm.cs)
- **Kích thước**: 1024x768 pixels
- **Màu nền**: #101420 (Dark Blue)
- **Vị trí**: Căn giữa màn hình

MainForm quản lý 3 view chính:
- `HomeView`: Danh sách manga
- `DetailView`: Chi tiết manga và danh sách chapter
- `ReaderView`: Màn hình đọc truyện

### 3. Luồng Điều Hướng

```
HomeView (Chọn manga)
    ↓
DetailView (Chọn chapter) → Back → HomeView
    ↓
ReaderView → Back → DetailView
    ↓ Home
HomeView
```

**Events Flow:**
- `MangaSelected`: Khi người dùng chọn một manga từ HomeView
- `ChapterClicked`: Khi người dùng chọn một chapter từ DetailView  
- `BackClicked`: Quay lại màn hình trước
- `HomeClicked`: Quay về trang chủ

## 🚀 Cài Đặt và Chạy

### Yêu Cầu Hệ Thống
- Windows OS (Windows 7 trở lên)
- .NET Framework hoặc .NET 5.0+
- Visual Studio 2019+ (để build từ source)

### Các Bước Chạy

1. **Clone repository:**
```bash
git clone https://github.com/VietPh37030/Manga-Winform.git
cd Manga-Winform
```

2. **Mở project:**
- Mở file `MangaReaderApp.csproj` bằng Visual Studio

3. **Build và chạy:**
- Nhấn F5 hoặc click nút Start trong Visual Studio
- Hoặc build Release và chạy file .exe trong thư mục `bin/Release/`

## 📦 Cấu Trúc Components

### Models
Chứa các class định nghĩa cấu trúc dữ liệu:
- `Manga`: Thông tin manga (title, slug, cover, chapters...)

### Views
Các UserControl đại diện cho từng màn hình:
- `HomeView`: Hiển thị grid/list các manga
- `DetailView`: Hiển thị thông tin chi tiết và chapters
- `ReaderView`: Hiển thị nội dung chapter

### Services  
Chứa các service xử lý business logic:
- API calls
- Data processing
- History management

### Controls
Custom controls có thể tái sử dụng trong nhiều view

## 📝 File Dữ Liệu

### history.json
Lưu trữ lịch sử đọc truyện của người dùng với format JSON, bao gồm:
- Manga đã đọc
- Chapter cuối cùng đã đọc
- Timestamp

## 🎨 Giao Diện

Ứng dụng sử dụng color scheme tối chủ đạo:
- Background: `#101420` (Dark Blue)
- Theme: Modern, minimalist
- Responsive layout với Dock style

## 🔮 Tương Lai Phát Triển

- [ ] Thêm tính năng tìm kiếm manga
- [ ] Bookmark/Favorite manga
- [ ] Download manga offline
- [ ] Nhiều theme color hơn
- [ ] Settings/Preferences
- [ ] Đồng bộ lịch sử giữa các thiết bị

## 👨‍💻 Tác Giả

**VietPh37030**
- GitHub: [@VietPh37030](https://github.com/VietPh37030)
- Repository: [Manga-Winform](https://github.com/VietPh37030/Manga-Winform)

## 📄 License

Project này được tạo ra cho mục đích học tập và nghiên cứu.

---

**Lưu ý**: Đây là ứng dụng demo/học tập. Hãy đảm bảo tuân thủ bản quyền khi sử dụng nội dung manga từ các nguồn khác.
