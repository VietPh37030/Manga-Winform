using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using MangaReaderApp.Models;
using System.Collections.Generic;

namespace MangaReaderApp.Services
{
    public class OtruyenService
    {
        private readonly HttpClient _httpClient;
        private const string BASE_URL = "https://otruyenapi.com/v1/api";
        private const string CHAPTER_BASE_URL = "https://sv1.otruyencdn.com/v1/api";
        public const string IMAGE_CDN_URL = "https://otruyenapi.com/uploads/comics/";

        public OtruyenService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<MangaListData> GetNewMangaAsync(int page = 1)
        {
            string url = $"{BASE_URL}/danh-sach/truyen-moi?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaListData> GetHotMangaAsync(int page = 1)
        {
            string url = $"{BASE_URL}/danh-sach/truyen-hot?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaListData> SearchMangaAsync(string keyword, int page = 1)
        {
            string url = $"{BASE_URL}/tim-kiem?keyword={Uri.EscapeDataString(keyword)}&page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public async Task<MangaDetailData> GetMangaDetailAsync(string slug)
        {
            string url = $"{BASE_URL}/truyen-tranh/{slug}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaDetailData>>(response);
            return apiResponse?.Data;
        }

        public async Task<ChapterDetailData> GetChapterDetailAsync(string chapterId)
        {
            string url = $"{CHAPTER_BASE_URL}/chapter/{chapterId}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ChapterDetailResponse>(response);
            return apiResponse?.Data;
        }
        
        public async Task<List<Category>> GetCategoriesAsync()
        {
            string url = $"{BASE_URL}/the-loai";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<CategoryListData>>(response);
            return apiResponse?.Data?.Items;
        }

        public async Task<MangaListData> GetMangaByCategoryAsync(string slug, int page = 1)
        {
            string url = $"{BASE_URL}/the-loai/{slug}?page={page}";
            var response = await _httpClient.GetStringAsync(url);
            var apiResponse = JsonConvert.DeserializeObject<ApiResponse<MangaListData>>(response);
            return apiResponse?.Data;
        }

        public string GetThumbUrl(string thumbPath)
        {
             if (string.IsNullOrEmpty(thumbPath)) return "";
             if (thumbPath.StartsWith("http")) return thumbPath;
             return $"{IMAGE_CDN_URL}{thumbPath}";
        }
    }
}
