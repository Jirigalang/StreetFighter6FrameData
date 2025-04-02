using HtmlAgilityPack;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace StreetFighter6FrameData
{
    public static class Helpers
    {
        /// <summary>
        /// 爬虫准备
        /// </summary>
        /// <returns></returns>
        static public HttpClient InitializeCrawlerClient()
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.124 Safari/537.36");
            client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8");
            client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
            client.DefaultRequestHeaders.Add("Referer", "https://www.streetfighter.com");
            client.DefaultRequestHeaders.Add("DNT", "1");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
            client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            client.BaseAddress = new Uri("https://www.streetfighter.com");
            return client;
        }
        /// <summary>
        /// 爬取角色列表
        /// </summary>
        /// <param name="lang"></param>
        static public void FetchCharacterList(HttpClient client, string lang)
        {
            lang = "/" + lang;
            if (lang == "en-US")
                lang = string.Empty;
            var html = client.GetStringAsync(@$"https://www.streetfighter.com/6{lang}/character").Result;
            File.WriteAllText("characterlist.html", html);
        }
        /// <summary>
        /// 根据列表爬取对应的角色帧数表
        /// </summary>
        static public void FetchCharacterFrameData(HttpClient client)
        {
            HtmlDocument doc = new();

            doc.Load("characterlist.html");
            var targetDiv = doc.DocumentNode.SelectSingleNode("//div[@class='select_character__select__list__PP9yQ']");
            var links = targetDiv?.SelectNodes(".//li/a[@href]");
            if (links != null)
            {
                foreach (var link in links)
                {
                    string href = link.GetAttributeValue("href", "");
                    string characterName = href.Split('/').Last();
                    var framedata = client.GetAsync(client.BaseAddress + href + @"/frame").Result;
                    if (!Path.Exists("html"))
                    {
                        Directory.CreateDirectory("html");
                    }
                    File.WriteAllText(@"html/" + characterName + ".html", framedata.Content.ReadAsStringAsync().Result);
                    Console.WriteLine("已保存 " + characterName + ".html");
                }

            }
            else
            {
                Console.WriteLine("未找到目标 div");
            }
            File.Delete("character.html");
        }
        /// <summary>
        /// 将帧数表保存到各个角色的 json 文件
        /// </summary>
        static public void ParseFrameData()
        {
            JsonSerializerOptions _jsonOptions = new()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string[] htmlFiles = Directory.GetFiles(
                                 AppDomain.CurrentDomain.BaseDirectory + "/html",
                                 "*.html"
                                );

            foreach (var filename in htmlFiles)
            {
                CommandList commandList = new();
                HtmlDocument doc = new();
                doc.Load(filename);
                var tableRows = doc.DocumentNode.SelectNodes("//tr");

                var headingNodes = tableRows
                    .Select((node, index) => new { Node = node, Index = index })
                    .Where(x => x.Node.GetAttributeValue("class", "") == "frame_heading__hh7Ah")
                    .ToList();

                var groups = headingNodes.Select((x, i) => new
                {
                    Category = x.Node.SelectSingleNode(".//span").InnerText,
                    StartIndex = x.Index,
                    EndIndex = (i < headingNodes.Count - 1)
                        ? headingNodes[i + 1].Index
                        : tableRows.Count
                });

                foreach (var group in groups)
                {
                    if (group.Category == "通常技")
                    {
                        for (int i = group.StartIndex + 1; i < group.EndIndex; i++)
                        {
                            commandList.通常技.Add(Move.Pares(tableRows[i], group.Category));
                        }
                    }
                    if (group.Category == "特殊技")
                    {
                        for (int i = group.StartIndex + 1; i < group.EndIndex; i++)
                        {
                            commandList.特殊技.Add(Move.Pares(tableRows[i], group.Category));
                        }
                    }
                    if (group.Category == "必殺技")
                    {
                        for (int i = group.StartIndex + 1; i < group.EndIndex; i++)
                        {
                            commandList.必杀技.Add(Move.Pares(tableRows[i], group.Category));
                        }
                    }
                    if (group.Category == "スーパーアーツ")
                    {
                        for (int i = group.StartIndex + 1; i < group.EndIndex; i++)
                        {
                            commandList.超必杀.Add(Move.Pares(tableRows[i], "超必杀技"));
                        }
                    }
                    if (group.Category == "通常投げ")
                    {
                        for (int i = group.StartIndex + 1; i < group.EndIndex; i++)
                        {
                            commandList.通常投.Add(Move.Pares(tableRows[i], "通常投"));
                        }
                    }
                    if (group.Category == "共通システム")
                    {
                        for (int i = group.StartIndex + 1; i < group.EndIndex; i++)
                        {
                            commandList.共通系统.Add(Move.Pares(tableRows[i], "共通系统"));
                        }
                    }
                }
                if (!Directory.Exists("json"))
                {
                    Directory.CreateDirectory("json");
                }
                File.WriteAllText("json/" + Path.GetFileNameWithoutExtension(filename) + ".json", JsonSerializer.Serialize(commandList, _jsonOptions));
            }
        }
        /// <summary>
        /// 将八方向键的html标签替换为字符
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        static public string ReplaceImagePathsInHtml(string html)
        {
            foreach (var kvp in Command.NameMap)
            {
                string pattern = $@"(<img\s+[^>]*src\s*=\s*[""']){Regex.Escape(kvp.Key)}([""'][^>]*>)";
                html = Regex.Replace(html, pattern, kvp.Value);
            }
            return html;
        }
        /// <summary>
        /// 生成别名文件
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="names"></param>
        static public void GenerateAliasFiles()
        {
            List<Name> names = [];

            var config = new JsonSerializerOptions()
            {
                WriteIndented = true,
                Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            string[] htmlFiles = Directory.GetFiles(
                                 Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "json"),
                                 "*.json"
                                );
            foreach (var filename in htmlFiles)
            {
                string text = File.ReadAllText(filename);
                var a = JsonSerializer.Deserialize<CommandList>(text);

                names.Add(new Name { BaseName = Path.GetFileNameWithoutExtension(filename), 别名 = [] });
                if (!Directory.Exists("Alias"))
                {
                    Directory.CreateDirectory("Alias");
                }
                File.WriteAllText(Path.Combine("Alias", "Alias.json"), JsonSerializer.Serialize(names, config));

            }
        }

    }
    public class Name
    {
        public required string BaseName { get; set; }
        public List<string>? 别名 { get; set; }
    }
}
