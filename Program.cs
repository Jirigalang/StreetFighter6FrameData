using StreetFighter6FrameData;
using static StreetFighter6FrameData.Helpers;
var client = InitializeCrawlerClient();
FetchCharacterList(client, "ja-JP");
FetchCharacterFrameData(client);
ParseFrameData();
GenerateAliasFiles();
//只能翻译日语
Translate.Run();
