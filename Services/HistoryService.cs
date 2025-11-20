using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using MangaReaderApp.Models;

namespace MangaReaderApp.Services
{
    public class HistoryService
    {
        private const string FILE_NAME = "history.json";
        private List<HistoryItem> _history;

        public HistoryService()
        {
            LoadHistory();
        }

        private void LoadHistory()
        {
            if (File.Exists(FILE_NAME))
            {
                try
                {
                    string json = File.ReadAllText(FILE_NAME);
                    _history = JsonConvert.DeserializeObject<List<HistoryItem>>(json) ?? new List<HistoryItem>();
                }
                catch
                {
                    _history = new List<HistoryItem>();
                }
            }
            else
            {
                _history = new List<HistoryItem>();
            }
        }

        public void AddToHistory(Manga manga, string chapterName, string chapterId)
        {
            var existing = _history.FirstOrDefault(h => h.MangaSlug == manga.Slug);
            if (existing != null)
            {
                _history.Remove(existing);
            }

            _history.Insert(0, new HistoryItem
            {
                MangaName = manga.Name,
                MangaSlug = manga.Slug,
                ThumbUrl = manga.ThumbUrl,
                LastChapterName = chapterName,
                LastChapterId = chapterId,
                LastReadTime = DateTime.Now
            });

            // Keep only last 50 items
            if (_history.Count > 50) _history.RemoveAt(_history.Count - 1);

            SaveHistory();
        }

        private void SaveHistory()
        {
            try
            {
                string json = JsonConvert.SerializeObject(_history, Formatting.Indented);
                File.WriteAllText(FILE_NAME, json);
            }
            catch { /* Ignore save errors */ }
        }

        public List<HistoryItem> GetHistory()
        {
            return _history;
        }
    }

    public class HistoryItem
    {
        public string MangaName { get; set; }
        public string MangaSlug { get; set; }
        public string ThumbUrl { get; set; }
        public string LastChapterName { get; set; }
        public string LastChapterId { get; set; }
        public DateTime LastReadTime { get; set; }
    }
}
