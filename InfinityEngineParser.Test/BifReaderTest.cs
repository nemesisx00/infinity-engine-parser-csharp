namespace InfinityEngineParser.Test;

using InfinityEngineParser;
using InfinityEngineParser.Bif;
using InfinityEngineParser.Key;
using InfinityEngineParser.Readers;

[Collection("ReadsKeyFile")]
public class BifReaderTest
{
	public static IEnumerable<object[]> BifcFileNameData
		=> new object[][]
		{
			new object[] { Games.IcewindDale1, "CD2/Data/AR100A.cbf" },
		};
	
	public static IEnumerable<object[]> BifcCompressedFileNameData
		=> new object[][]
		{
			new object[] { Games.BaldursGate2, "data/Data/AREA000A.bif" },
		};
	
	public static IEnumerable<object[]> BiffFileNameData
		=> new object[][]
		{
			new object[] { Games.BaldursGate1, "data/AREA000A.BIF" },
			new object[] { Games.BaldursGate1EnhancedEdition, "data/AR0002.BIF" },
			new object[] { Games.BaldursGate2, "data/NPCAnim.bif" },
			new object[] { Games.BaldursGate2EnhancedEdition, "data/BlackPits.bif" },
			new object[] { Games.IcewindDale1, "data/AR1000.bif" },
			new object[] { Games.IcewindDale1EnhancedEdition, "data/AR100A.bif" },
		};
	
	private static Dictionary<Games, (string, ushort)> ResourceNames = new()
	{
		{ Games.BaldursGate1, ("NSLVLG1", ResourceTypes.BAM) },
		{ Games.BaldursGate1EnhancedEdition, ("NSLVLG1", ResourceTypes.BAM) },
		{ Games.BaldursGate2, ("NSLVLG1", ResourceTypes.BAM) },
		{ Games.BaldursGate2EnhancedEdition, ("NSLVLG1", ResourceTypes.BAM) },
		{ Games.IcewindDale1, ("NSLVLG1", ResourceTypes.BAM) },
		{ Games.IcewindDale1EnhancedEdition, ("NSLVLG1", ResourceTypes.BAM) },
	};
	
	public static IEnumerable<object[]> ResourceSearchFileNameData
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
	[MemberData(nameof(ResourceSearchFileNameData))]
    public void FindResourceFilesTest(Games game, string expectedFileName)
    {
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var key = KeyReader.FromFile(Path.Combine(installPath, InfinityEngineKey.FileName));
			Assert.NotNull(key);
			
			(var resourceName, var resourceType) = ResourceNames[game];
			
			var resourceEntries = key.ResourceEntries.FindAll(re => re.Type == resourceType && re.Name.Equals(resourceName));
			Assert.Single(resourceEntries);
			
			var resourceEntry = resourceEntries.First();
			
			var bifEntries = key.BifEntries
				.Where((be, index) => index == resourceEntry.IndexBifEntry)
				.Distinct()
				.ToList();
			
			Assert.Single(bifEntries);
			
			var bifEntry = bifEntries.First();
			Assert.NotNull(bifEntry);
			Assert.NotNull(bifEntry.FileName);
			
			Assert.Equal(expectedFileName, bifEntry.FileName);
			
			var biff = BifReader.BiffFromFile(Path.Combine(installPath, bifEntry.FileName));
			Assert.NotNull(biff);
			
			Assert.NotNull(biff.Header);
			Assert.Equal(Biff.Signature, biff.Header.Signature);
			Assert.Equal(Biff.Version, biff.Header.Version);
		}
	}
	
	[Theory]
	[MemberData(nameof(BifcFileNameData))]
	public void ReadBifcTest(Games game, string fileName)
	{
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var bifc = BifReader.BifcFromFile(Path.Combine(installPath, fileName));
			Assert.NotNull(bifc);
			
			Assert.NotNull(bifc.Header);
			Assert.Equal(Bifc.Signature, bifc.Header.Signature);
			Assert.Equal(Bifc.Version, bifc.Header.Version);
			Assert.NotNull(bifc.Header.FileName);
			Assert.True(bifc.Header.CompressedDataLength > 0 && bifc.Header.UncompressedDataLength > bifc.Header.CompressedDataLength);
			
			Assert.NotNull(bifc.Data);
			
			Assert.NotNull(bifc.Data.Header);
			Assert.Equal(Biff.Signature, bifc.Data.Header.Signature);
			Assert.Equal(Biff.Version, bifc.Data.Header.Version);
			
			Assert.NotEmpty(bifc.Data.TilesetEntries);
			Assert.Equal((int)bifc.Data.Header.TilesetCount, bifc.Data.TilesetEntries.Count);
			bifc.Data.TilesetEntries.ForEach(tilesetEntry => {
				Assert.True(tilesetEntry.Locator > 0);
				Assert.True(tilesetEntry.TileCount > 0);
				Assert.True(tilesetEntry.TileSize > 0);
			});
			
			Assert.NotEmpty(bifc.Data.FileEntries);
			Assert.Equal((int)bifc.Data.Header.FileCount, bifc.Data.FileEntries.Count);
			bifc.Data.FileEntries.ForEach(fileEntry => {
				Assert.True(fileEntry.Locator > 0);
				Assert.True(fileEntry.Size > 0);
			});
		}
	}
	
	[Theory]
	[MemberData(nameof(BifcCompressedFileNameData))]
	public void ReadBifcCompressedTest(Games game, string fileName)
	{
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var result = BifReader.BifcCompressedFromFile(Path.Combine(installPath, fileName));
			Assert.NotNull(result);
			
			Assert.NotNull(result.Header);
			Assert.Equal(BifcCompressed.Signature, result.Header.Signature);
			Assert.Equal(BifcCompressed.Version, result.Header.Version);
			Assert.True(result.Header.UncompressedBifSize > 0);
			
			Assert.NotNull(result.Data);
			
			Assert.NotNull(result.Data.Header);
			Assert.Equal(Biff.Signature, result.Data.Header.Signature);
			Assert.Equal(Biff.Version, result.Data.Header.Version);
			
			Assert.NotEmpty(result.Data.TilesetEntries);
			result.Data.TilesetEntries.ForEach(tilesetEntry => {
				Assert.True(tilesetEntry.Locator > 0);
				Assert.True(tilesetEntry.TileCount > 0);
				Assert.True(tilesetEntry.TileSize > 0);
			});
			
			Assert.NotEmpty(result.Data.FileEntries);
			result.Data.FileEntries.ForEach(fileEntry => {
				Assert.True(fileEntry.Locator > 0);
				Assert.True(fileEntry.Size > 0);
			});
		}
	}
	
	[Theory]
	[MemberData(nameof(BiffFileNameData))]
	public void ReadBiffTest(Games game, string fileName)
	{
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var biff = BifReader.BiffFromFile(Path.Combine(installPath, fileName));
			Assert.NotNull(biff);
			
			Assert.NotNull(biff.Header);
			Assert.Equal(Biff.Signature, biff.Header.Signature);
			Assert.Equal(Biff.Version, biff.Header.Version);
			
			Assert.NotEmpty(biff.FileEntries);
			Assert.Equal((int)biff.Header.FileCount, biff.FileEntries.Count);
			biff.FileEntries.ForEach(fileEntry => {
				if(Games.BaldursGate2EnhancedEdition != game)
					Assert.True(fileEntry.Size > 0);
			});
			
			if(Games.BaldursGate2 == game)
				Assert.Empty(biff.TilesetEntries);
			else
				Assert.NotEmpty(biff.TilesetEntries);
			
			Assert.Equal((int)biff.Header.TilesetCount, biff.TilesetEntries.Count);
			biff.TilesetEntries.ForEach(tilesetEntry => {
				Assert.True(tilesetEntry.Locator > 0);
				Assert.True(tilesetEntry.TileCount > 0);
				Assert.True(tilesetEntry.TileSize > 0);
			});
		}
	}
}
