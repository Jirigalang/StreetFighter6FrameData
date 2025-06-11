using static StreetFighter6FrameData.Helpers;
var client = InitializeCrawlerClient();
FetchCharacterList(client, "ja-JP");
FetchCharacterFrameData(client);
ParseFrameData();
GenerateAliasFiles();