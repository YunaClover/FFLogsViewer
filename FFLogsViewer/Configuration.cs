using System;
using System.Collections.Generic;
using Dalamud.Configuration;
using FFLogsViewer.Model;
using Newtonsoft.Json;

namespace FFLogsViewer;

[Serializable]
public class Configuration : IPluginConfiguration
{
    [JsonIgnore]
    public const int CurrentConfigVersion = 1;
    public int Version { get; set; } = CurrentConfigVersion;
    public string ClientId { get; set; } = string.Empty;
    public string ClientSecret { get; set; } = string.Empty;
    public bool ContextMenu { get; set; } = true;
    public bool ContextMenuStreamer { get; set; }
    public bool ContextMenuPartyView { get; set; } = true;
    public bool ContextMenuAlwaysPartyView { get; set; }
    public bool OpenInBrowser { get; set; }
    public bool ShowTomestoneOption { get; set; } = false;
    public string ContextMenuButtonName { get; set; } = "Search FF Logs";
    public bool IsDefaultViewParty { get; set; }
    public bool HideInCombat { get; set; }
    public bool IsDefaultLayout { get; set; } = true;
    public bool IsHistoricalDefault { get; set; } = true;
    public bool IsEncounterLayout { get; set; } = true;
    public bool IsCachingEnabled { get; set; } = true;
    public bool IsAllJobsDefault { get; set; } = true;
    public int NbOfDecimalDigits { get; set; }
    public StatType? DefaultStatTypePartyView { get; set; }
    public LayoutEntry? DefaultEncounterPartyView { get; set; }
    public List<LayoutEntry> Layout { get; set; } = [];
    public List<Stat> Stats { get; set; } = [];
    public Metric Metric { get; set; } = new() { Name = "rDPS", InternalName = "rdps" };
    public Style Style { get; set; } = new();
    public OpenWith OpenWith { get; set; } = new();
    public bool IsUpdateDismissed2213 { get; set; }

    public void Save()
    {
        Service.Interface.SavePluginConfig(this);
    }

    public void Initialize()
    {
        if (this.IsDefaultLayout || this.Layout.Count == 0)
        {
            this.SetDefaultLayout();
        }

        if (this.Stats.Count == 0)
        {
            this.Stats.AddRange(GetDefaultStats());
        }

        this.Upgrade();
    }

    public void Upgrade()
    {
        // all stars stats
        if (this.Version == 0)
        {
            var defaultStats = GetDefaultStats();
            if (this.Stats.Count < defaultStats.Count)
            {
                for (var i = this.Stats.Count; i < defaultStats.Count; i++)
                {
                    this.Stats.Add(defaultStats[i]);
                }
            }

            this.Version++;
            this.Save();
        }
    }

    public void SetDefaultLayout()
    {
        this.Layout = GetDefaultLayout();
        this.IsDefaultLayout = true;
    }

    private static List<LayoutEntry> GetDefaultLayout()
    {
        return
        [
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "阿卡迪亚重量级", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-", SwapId = "7.4", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "致命美人", Expansion = "Dawntrail", Zone = "AAC Heavyweight", ZoneId = 73, Encounter = "Vamp Fatale", EncounterId = 101, Difficulty = "Savage", DifficultyId = 101, SwapId = "7.4", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "极限兄弟", Expansion = "Dawntrail", Zone = "ACC Heavyweight", ZoneId = 73, Encounter = "Red Hot and Deep Blue", EncounterId = 102, Difficulty = "Savage", DifficultyId = 101, SwapId = "7.4", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "霸王", Expansion = "Dawntrail", Zone = "ACC Heavyweight", ZoneId = 73, Encounter = "The Tyrant", EncounterId = 103, Difficulty = "Savage", DifficultyId = 101, SwapId = "7.4", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "林德布鲁姆", Expansion = "Dawntrail", Zone = "ACC Heavyweight", ZoneId = 73, Encounter = "The Lindwurm", EncounterId = 104, Difficulty = "Savage", DifficultyId = 101, SwapId = "7.4", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "林德布鲁姆 II", Expansion = "Dawntrail", Zone = "ACC Heavyweight", ZoneId = 73, Encounter = "The Lindwurm II", EncounterId = 105, Difficulty = "Savage", DifficultyId = 101, SwapId = "7.4", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "绝境战", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-", SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "巴哈姆特绝境战", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "The Unending Coil of Bahamut", EncounterId = 1073, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "究极神兵绝境战", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "The Weapon's Refrain", EncounterId = 1074, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "亚历山大绝境战", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "The Epic of Alexander", EncounterId = 1075, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "幻象龙诗绝境战", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "Dragonsong's Reprise", EncounterId = 1076, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "欧米茄绝境验证战", Expansion = "Dawntrail", Zone = "Ultimates (Legacy)", ZoneId = 59, Encounter = "The Omega Protocol", EncounterId = 1077, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "光暗未来绝境战", Expansion = "Dawntrail", Zone = "Futures Rewritten", ZoneId = 65, Encounter = "Futures Rewritten", EncounterId = 1079, Difficulty = "Normal", DifficultyId = 100, SwapId = "DT ult", SwapNumber = 0 },
            new LayoutEntry { Type = LayoutEntryType.Header, Alias = "讨伐歼灭战III", Expansion = "-", Zone = "-", Encounter = "-", Difficulty = "-" },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "泽莲尼娅", Expansion = "Dawntrail", Zone = "Trials II (Extreme)", ZoneId = 67, Encounter = "Zelenia", EncounterId = 1080, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "永远之暗", Expansion = "Dawntrail", Zone = "Trials II (Extreme)", ZoneId = 67, Encounter = "Necron", EncounterId = 1081, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "护锁刃龙", Expansion = "Dawntrail", Zone = "Trials II (Extreme)", ZoneId = 67, Encounter = "Arkveld", EncounterId = 1082, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "格莱杨拉波尔", Expansion = "Dawntrail", Zone = "Trials III (Extreme)", ZoneId = 72, Encounter = "Doomtrain", EncounterId = 1083, Difficulty = "Normal", DifficultyId = 100 },
            new LayoutEntry { Type = LayoutEntryType.Encounter, Alias = "恩欧", Expansion = "Dawntrail", Zone = "Trials III (Extreme)", ZoneId = 72, Encounter = "Enuo", EncounterId = 1084, Difficulty = "Normal", DifficultyId = 100 },
        ];
    }

    private static List<Stat> GetDefaultStats()
    {
        return
        [
            new Stat { Name = "Best", Type = StatType.Best, IsEnabled = true },
            new Stat { Alias = "Med.", Name = "Median", Type = StatType.Median, IsEnabled = true },
            new Stat { Name = "Kills", Type = StatType.Kills, IsEnabled = true },
            new Stat { Name = "Fastest", Type = StatType.Fastest, IsEnabled = false },
            new Stat { Alias = "/metric/", Name = "Best Metric", Type = StatType.BestAmount, IsEnabled = false },
            new Stat { Name = "Job", Type = StatType.Job, IsEnabled = true },
            new Stat { Name = "Best Job", Type = StatType.BestJob, IsEnabled = false },
            new Stat { Alias = "ASP", Name = "All Stars Points", Type = StatType.AllStarsPoints, IsEnabled = false },
            new Stat { Alias = "ASP R", Name = "All Stars Rank", Type = StatType.AllStarsRank, IsEnabled = false },
            new Stat { Alias = "ASP R%", Name = "All Stars Rank %", Type = StatType.AllStarsRankPercent, IsEnabled = false },
        ];
    }
}
