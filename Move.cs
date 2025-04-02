using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace StreetFighter6FrameData
{

    public class Move
    {
        public required string 招式名 { get; set; }
        public List<string>? 别名 { get; set; }
        public required string 类型 { get; set; }
        public required string 经典指令 { get; set; }
        public string? 现代指令 { get; set; }
        public string? 发生 { get; set; }
        public string? 持续帧 { get; set; }
        public required string 后摇 { get; set; }
        public string? 命中 { get; set; }
        public string? 被防 { get; set; }
        public string? 取消类型 { get; set; }
        public string? 伤害 { get; set; }
        public string? 连招修正 { get; set; }
        public string? 斗气槽增加量 { get; set; }
        public string? 被防斗气槽减少量 { get; set; }
        public string? 反击康斗气槽减少量 { get; set; }
        public string? 大招增加量 { get; set; }
        public string? 属性 { get; set; }
        public string? 备注 { get; set; }
        public static Move Pares(HtmlNode link, string 类型)
        {
            var 招式名 = link.SelectSingleNode(".//span[@class='frame_arts__ZU5YI']");
            var 经典指令 = link.SelectSingleNode(".//p[@class='frame_classic___gpLR']");
            var 发生 = link.SelectSingleNode(".//td[@class='frame_startup_frame__Dc2Ph']");
            var 持续帧 = link.SelectSingleNode(".//td[@class='frame_active_frame__6Sovc']");
            var 部分持续帧 = 持续帧.SelectSingleNode(".//div[@class='frame_inner__Qf7xV']");
            var 总持续帧 = 持续帧.SelectSingleNode(".//label");
            bool 有无部分持续帧 = 部分持续帧 is null;
            var 后摇 = link.SelectSingleNode(".//td[@class='frame_recovery_frame__CznJj']");
            var 命中 = link.SelectSingleNode(".//td[@class='frame_hit_frame__K7xOz undefined']");
            var 被防 = link.SelectSingleNode(".//td[@class='frame_block_frame__SfHiW undefined']");
            var 取消类型 = link.SelectSingleNode(".//td[@class='frame_cancel__hT_hr']");
            var 伤害 = link.SelectSingleNode(".//td[@class='frame_damage__HWaQm']");
            var 连招修正 = link.SelectSingleNode(".//td[@class='frame_combo_correct__hCDUB']");
            var 斗气槽增加量 = link.SelectSingleNode(".//td[@class='frame_drive_gauge_gain_hit___Jg7j']");
            var 被防斗气槽减少量 = link.SelectSingleNode(".//td[@class='frame_drive_gauge_lose_dguard__4uQOc']");
            var 反击康斗气槽减少量 = link.SelectSingleNode(".//td[@class='frame_drive_gauge_lose_punish__mFrmM']");
            var 大招增加量 = link.SelectSingleNode(".//td[@class='frame_sa_gauge_gain__oGcqw']");
            var 属性 = link.SelectSingleNode(".//td[@class='frame_attribute__1vABD']");
            var 备注td = link.SelectSingleNode(".//td[@class='frame_note__hfwBr']");
            string 备注 = "";
            if (备注td.SelectNodes(".//li") != null)
                foreach (var 备注li in 备注td.SelectNodes(".//li"))
                {
                    备注 += 备注li.InnerText.Replace("\r\n", "").Replace(" ", "") + "\n";
                }
            else
            {
                备注 = 备注td.InnerText.Replace("\r\n", "").Replace(" ", "");
            }
            return new Move()
            {
                招式名 = (招式名?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                别名 = [],
                经典指令 = ReplaceImagePathsInHtml(经典指令!.InnerHtml.Replace("弱", "").Replace("中", "").Replace("強", "")).Replace(" ", "").Replace("\n", " ").Replace("\r", ""),
                类型 = 类型,
                发生 = (发生?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                持续帧 = 有无部分持续帧 ? 持续帧.InnerText.Replace(" ", "") : $"{总持续帧.InnerText}({部分持续帧!.InnerText})",
                后摇 = (后摇?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                命中 = (命中?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                被防 = (被防?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                取消类型 = (取消类型?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                伤害 = (伤害?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                连招修正 = (连招修正?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                斗气槽增加量 = (斗气槽增加量?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                被防斗气槽减少量 = (被防斗气槽减少量?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                反击康斗气槽减少量 = (反击康斗气槽减少量?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                大招增加量 = (大招增加量?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                属性 = (属性?.InnerText ?? "").Replace(" ", "").Replace("\r\n", ""),
                备注 = 备注
            };
        }
        static public string ReplaceImagePathsInHtml(string html)
        {
            foreach (var kvp in Command.NameMap)
            {
                string pattern = $@"(<img\s+[^>]*src\s*=\s*[""']){Regex.Escape(kvp.Key)}([""'][^>]*>)";
                html = Regex.Replace(html, pattern, kvp.Value);
            }
            return html;
        }
    }
    class CommandList
    {
        public CommandList()
        {
            通常技 = [];
            特殊技 = [];
            必杀技 = [];
            超必杀 = [];
            通常投 = [];
            共通系统 = [];
        }

        public List<Move> 通常技 { get; set; }
        public List<Move> 特殊技 { get; set; }
        public List<Move> 必杀技 { get; set; }
        public List<Move> 超必杀 { get; set; }
        public List<Move> 通常投 { get; set; }
        public List<Move> 共通系统 { get; set; }
    }
}
