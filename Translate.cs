using System.Text.Json;

namespace StreetFighter6FrameData
{
    internal class Translate
    {
        /// <summary>
        /// 翻译一些固定的东西
        /// </summary>
        static public void Run()
        {
            string[] JsonFiles = Directory.GetFiles(
                                 AppDomain.CurrentDomain.BaseDirectory + "/json",
                                 "*.json"
                                );
            int count = 0;
            JsonSerializerOptions _jsonOptions = new()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            foreach (string file in JsonFiles)
            {
                string akaFile = File.ReadAllText(file);
                var commandList = JsonSerializer.Deserialize<CommandList>(akaFile);

                foreach (var 共通系统 in commandList!.共通系统)
                {
                    if (共通系统.别名 == null)
                    {
                        共通系统.别名 = [];
                    }
                    if (共通系统.经典指令 == "→→")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("前前");
                            count++;
                        }
                    }
                    if (共通系统.经典指令 == "←←")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("后后");
                            count++;
                        }
                    }
                    if (共通系统.经典指令 == "重拳重脚")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("迸发");
                            count++;
                        }
                    }
                    if (共通系统.经典指令 == "（ガードorドライブパリィ成立に）→+重拳重脚")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("防御斗反");
                            count++;
                        }
                    }
                    if (共通系统.经典指令 == "（起き上がり時に）→+重拳重脚")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("起身斗反");
                            count++;
                        }
                    }
                    if (共通系统.招式名 == "ドライブパリィ")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("蓝防");
                            count++;
                        }
                    }
                    if (共通系统.招式名 == "ジャストパリィ（打撃）")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("精防(打击)");
                            count++;
                        }
                    }
                    if (共通系统.招式名 == "ジャストパリィ（飛び道具）")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("精防(飞行道具)");
                            count++;
                        }
                    }
                    if (共通系统.招式名 == "パリィドライブラッシュ")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("蓝防绿冲");
                            count++;
                        }
                    }
                    if (共通系统.招式名 == "キャンセルドライブラッシュ")
                    {
                        if (共通系统.别名.Count == 0)
                        {
                            共通系统.别名.Add("取消绿冲");
                            count++;
                        }
                    }
                    if (共通系统.招式名.Contains("ドライブパリィに"))
                    {
                        共通系统.招式名.Replace("ドライブパリィに", "蓝防时");
                    }

                }
                File.WriteAllText(file, JsonSerializer.Serialize(commandList, _jsonOptions));
                Console.WriteLine($"添加了{count}个别名");
            }
        }
    }
}