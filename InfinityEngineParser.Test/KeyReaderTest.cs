namespace InfinityEngineParser.Test;

using InfinityEngineParser.Key;
using InfinityEngineParser.Readers;

[Collection("ReadsKeyFile")]
public class KeyReaderTest
{
	public static IEnumerable<object[]> BiffFileNames()
		=> new object[][]
		{
			new object[] { Games.BaldursGate1, "data\\CREAnim.bif" },
			new object[] { Games.BaldursGate1EnhancedEdition, "data/NPCANIM.BIF" },
			new object[] { Games.BaldursGate2, "data\\NPCAnim.bif" },
			new object[] { Games.BaldursGate2EnhancedEdition, "data/NPCAnim.bif" },
			new object[] { Games.IcewindDale1, "data\\CHRanim.bif" },
			new object[] { Games.IcewindDale1EnhancedEdition, "data/CREANIM6.bif" },
		};
	
	[Theory]
	[MemberData(nameof(BiffFileNames))]
    public void ReadKeyTest(Games game, string bifFileName)
    {
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var filePath = Path.Combine(installPath, InfinityEngineKey.FileName);
			Assert.NotNull(filePath);
			
			var result = KeyReader.FromFile(filePath);
			
			Assert.NotNull(result);
			
			Assert.NotNull(result.Header);
			Assert.Equal(InfinityEngineKey.Signature, result.Header.Signature);
			Assert.Equal(InfinityEngineKey.Version, result.Header.Version);
			
			Assert.NotEmpty(result.BifEntries);
			Assert.Equal((int)result.Header.BifCount, result.BifEntries.Count);
			result.BifEntries
				.ForEach(entry => {
					Assert.NotNull(entry.FileName);
					Assert.NotEmpty(entry.FileName);
				});
			
			Assert.Single(result.BifEntries.FindAll(be => be.FileName?.Equals(bifFileName) == true));
			
			Assert.NotEmpty(result.ResourceEntries);
			Assert.Equal((int)result.Header.ResourceCount, result.ResourceEntries.Count);
			result.ResourceEntries
				.ForEach(entry => {
					Assert.NotNull(entry.Name);
				});
		}
    }
}
