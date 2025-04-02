namespace StreetFighter6FrameData
{
    static public class Command
    {
        public static Dictionary<string, string> NameMap { get; } =
        new Dictionary<string, string>
        {
            { @"/6/assets/images/common/controller/key-u.png", "↑" },
            { @"/6/assets/images/common/controller/key-d.png", "↓" },
            { @"/6/assets/images/common/controller/key-dc.png", "↓蓄力" },
            { @"/6/assets/images/common/controller/key-l.png", "←" },
            { @"/6/assets/images/common/controller/key-lc.png", "←蓄力" },
            { @"/6/assets/images/common/controller/key-r.png", "→" },
            { @"/6/assets/images/common/controller/key-ul.png", "↖" },
            { @"/6/assets/images/common/controller/key-ur.png", "↗" },
            { @"/6/assets/images/common/controller/key-dl.png", "↙" },
            { @"/6/assets/images/common/controller/key-dr.png", "↘" },
            { @"/6/assets/images/common/controller/key-circle.png", "摇杆一周" },
            { @"/6/assets/images/common/controller/key-plus.png", " + " },
            { @"/6/assets/images/common/controller/icon_punch.png", "拳" },
            { @"/6/assets/images/common/controller/icon_punch_l.png", "轻拳" },
            { @"/6/assets/images/common/controller/icon_punch_m.png", "中拳" },
            { @"/6/assets/images/common/controller/icon_punch_h.png", "重拳" },
            { @"/6/assets/images/common/controller/icon_kick.png", "脚" },
            { @"/6/assets/images/common/controller/icon_kick_l.png", "轻脚" },
            { @"/6/assets/images/common/controller/icon_kick_m.png", "中脚" },
            { @"/6/assets/images/common/controller/icon_kick_h.png", "重脚" },
            { @"/6/assets/images/common/controller/arrow_3.png", "▷" },
            { @"/6/assets/images/common/controller/key-or.png", "或" },
            { @"/6/assets/images/common/controller/key-nutral.png", "摇杆回中" },
        };

        public static Dictionary<char, string> numToDirection = new()
                        {
                            {'6', "→"}, {'2', "↓"}, {'3', "↘"}, {'4', "←"}, {'1', "↙"}, {'9', "↗"}, {'7', "↖"}, {'8', "↑"}
                        };

        public static Dictionary<string, string> buttonAbbr = new()
                        {
                            {"lp", "轻拳"}, {"mp", "中拳"}, {"hp", "重拳"},
                            {"lk", "轻脚"}, {"mk", "中脚"}, {"hk", "重脚"},
                            {"p", "拳"}, {"k", "脚"},{"pp", "拳拳"}, {"kk", "脚脚"}
                        };
    }
}
