using System;
using System.Collections.Generic;
using System.IO;

namespace GDDownloader
{
    public class Constants
    {
        public const string Title = "GD Downloader";
        public const string NgDownloadsBaseUrl = "https://audio-download.ngfiles.com/";
        public const int MinimunId = 469775;  // First ID with the known name pattern.
        public const int MaxAudioNameLength = 26;
        public static string LocalAppData { get; } = Environment.GetEnvironmentVariable("LOCALAPPDATA");
        public static string DefaultSavePath { get; } = LocalAppData != null ? Path.Combine(LocalAppData, "GeometryDash") : null;
        public static Dictionary<string, string> Filter { get; } = new Dictionary<string, string>
        {
            {" ", "-"},
            {"&", "amp"},
            {"<", "lt"},
            {">", "gt"},
            {"\"", "quot"}
        };
    }
}