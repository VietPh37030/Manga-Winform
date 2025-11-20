using Newtonsoft.Json;
using System.Collections.Generic;

namespace MangaReaderApp.Models
{
    // --- Common / List Models ---

    public class ApiResponse<T>
    {
        [JsonProperty("status")]
        public string Status { get; set; }
        
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class MangaListData
    {
        [JsonProperty("items")]
        public List<Manga> Items { get; set; }

        [JsonProperty("params")]
        public PaginationParams Params { get; set; }
        
        [JsonProperty("type_list")]
        public string TypeList { get; set; }
    }

    public class PaginationParams
    {
        [JsonProperty("pagination")]
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        [JsonProperty("totalItems")]
        public int TotalItems { get; set; }
        
        [JsonProperty("totalItemsPerPage")]
        public int TotalItemsPerPage { get; set; }
        
        [JsonProperty("currentPage")]
        public int CurrentPage { get; set; }
        
        [JsonProperty("pageRanges")]
        public int PageRanges { get; set; }
    }

    public class Manga
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("origin_name")]
        public List<string> OriginName { get; set; }

        [JsonProperty("thumb_url")]
        public string ThumbUrl { get; set; }

        [JsonProperty("sub_docquyen")]
        public bool SubDocQuyen { get; set; }

        [JsonProperty("category")]
        public List<Category> Category { get; set; }

        [JsonProperty("updatedAt")]
        public string UpdatedAt { get; set; }

        [JsonProperty("chaptersLatest")]
        public List<ChapterShort> ChaptersLatest { get; set; }
    }

    public class Category
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("slug")]
        public string Slug { get; set; }
    }

    public class ChapterShort
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }
        
        [JsonProperty("chapter_name")]
        public string ChapterName { get; set; }
        
        [JsonProperty("chapter_title")]
        public string ChapterTitle { get; set; }
        
        [JsonProperty("chapter_api_data")]
        public string ChapterApiData { get; set; }
    }

    public class CategoryListData
    {
        [JsonProperty("items")]
        public List<Category> Items { get; set; }
    }

    // --- Detail Models ---

    public class MangaDetailData
    {
        [JsonProperty("item")]
        public MangaDetail Item { get; set; }
    }

    public class MangaDetail : Manga
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("author")]
        public List<string> Author { get; set; }

        [JsonProperty("chapters")]
        public List<ServerVolume> Chapters { get; set; }
    }

    public class ServerVolume
    {
        [JsonProperty("server_name")]
        public string ServerName { get; set; }

        [JsonProperty("server_data")]
        public List<ChapterItem> ServerData { get; set; }
    }

    public class ChapterItem
    {
        [JsonProperty("filename")]
        public string Filename { get; set; }

        [JsonProperty("chapter_name")]
        public string ChapterName { get; set; }

        [JsonProperty("chapter_title")]
        public string ChapterTitle { get; set; }

        [JsonProperty("chapter_api_data")]
        public string ChapterApiData { get; set; }
    }

    // --- Chapter Image Models ---

    public class ChapterDetailResponse
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("data")]
        public ChapterDetailData Data { get; set; }
    }

    public class ChapterDetailData
    {
        [JsonProperty("domain_cdn")]
        public string DomainCdn { get; set; }

        [JsonProperty("item")]
        public ChapterImagesItem Item { get; set; }
    }

    public class ChapterImagesItem
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("chapter_name")]
        public string ChapterName { get; set; }

        [JsonProperty("chapter_title")]
        public string ChapterTitle { get; set; }

        [JsonProperty("chapter_path")]
        public string ChapterPath { get; set; }

        [JsonProperty("chapter_image")]
        public List<ChapterImageFile> ChapterImage { get; set; }

        [JsonProperty("prev_chapter")]
        public ChapterLink PrevChapter { get; set; }

        [JsonProperty("next_chapter")]
        public ChapterLink NextChapter { get; set; }
    }

    public class ChapterImageFile
    {
        [JsonProperty("image_page")]
        public int ImagePage { get; set; }

        [JsonProperty("image_file")]
        public string ImageFile { get; set; }
    }

    public class ChapterLink
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("chapter_api_data")]
        public string ChapterApiData { get; set; }
    }
}
