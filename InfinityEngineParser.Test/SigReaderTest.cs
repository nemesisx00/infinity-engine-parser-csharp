namespace InfinityEngineParser.Test;

using InfinityEngineParser;
using InfinityEngineParser.Bif;
using InfinityEngineParser.Key;
using InfinityEngineParser.Readers;

[Collection("ReadsKeyFile")]
public class SigReaderTest
{
	private static Dictionary<Games, string> BifcCompressedFileName = new()
	{
		{ Games.BaldursGate2, "data/Data/AREA000A.bif"},
	};
	
	private static Dictionary<Games, string> BifcFileName = new()
	{
		{ Games.IcewindDale1, "CD2/Data/AR100A.cbf" },
	};
	
	private static Dictionary<Games, string> BiffFileName = new()
	{
		{ Games.BaldursGate1, "data/AREA000A.BIF" },
		{ Games.BaldursGate1EnhancedEdition, "data/AR0002.BIF" },
		{ Games.BaldursGate2, "data/25Areas.bif" },
		{ Games.BaldursGate2EnhancedEdition, "data/AREA000A.bif" },
		{ Games.IcewindDale1, "data/AR1000.bif" },
		{ Games.IcewindDale1EnhancedEdition, "data/AR100A.bif" },
	};
	
	public static IEnumerable<object[]> GamesData()
		=> new object[][]
		{
			new object[] { Games.BaldursGate1 },
			new object[] { Games.BaldursGate1EnhancedEdition },
			new object[] { Games.BaldursGate2 },
			new object[] { Games.BaldursGate2EnhancedEdition },
			new object[] { Games.IcewindDale1 },
			new object[] { Games.IcewindDale1EnhancedEdition },
		};
	
	[Theory]
	[MemberData(nameof(GamesData))]
	public void DetectFileFormatTest(Games game)
	{
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var key = KeyReader.FromFile(Path.Combine(installPath, InfinityEngineKey.FileName));
			Assert.NotNull(key);
			
			Bifc? bifc = null;
			BifcCompressed? bifcCompressed = null;
			Biff? biff = null;
			
			key.BifEntries.ForEach(entry => {
				Assert.NotNull(entry.FileName);
				
				if(!(entry.FileName.Contains("AREA") || entry.FileName.Contains("Anim") || entry.FileName.Contains("AR")))
					return;
				
				var filePath = installPath;
				
				filePath = Path.Combine(filePath, entry.FileName);
				
				switch(game)
				{
					case Games.BaldursGate2:
						if((entry.FileName.StartsWith("data\\AREA") || entry.FileName.StartsWith("movies")))
							filePath = Path.Combine(installPath, "data", entry.FileName);
						break;
					case Games.IcewindDale1:
						if(!File.Exists(Path.Combine(installPath, entry.FileName)))
						{
							var cbf = entry.FileName.Substring(0, entry.FileName.Length - 4) + ".cbf";
							if(File.Exists(Path.Combine(installPath, "CD2", cbf)))
								filePath = Path.Combine(installPath, "CD2", cbf);
							else if(File.Exists(Path.Combine(installPath, "CD3", cbf)))
								filePath = Path.Combine(installPath, "CD3", cbf);
						}
						break;
				}
				
				var header = SigReader.FromFile(filePath);
				
				if(bifc == null && Bifc.Signature.Equals(header.Signature) && Bifc.Version.Equals(header.Version))
				{
					bifc = BifReader.BifcFromFile(filePath);
					Assert.NotNull(bifc);
				}
				else if(bifcCompressed == null && BifcCompressed.Signature.Equals(header.Signature) && BifcCompressed.Version.Equals(header.Version))
				{
					bifcCompressed = BifReader.BifcCompressedFromFile(filePath);
					Assert.NotNull(bifcCompressed);
				}
				else if(biff == null && Biff.Signature.Equals(header.Signature) && Biff.Version.Equals(header.Version))
				{
					biff = BifReader.BiffFromFile(filePath);
					Assert.NotNull(biff);
				}
				
				if(bifc != null && bifcCompressed != null && biff != null)
					return;
			});
			
			Assert.NotNull(biff);
			
			switch(game)
			{
				case Games.BaldursGate2:
					Assert.Null(bifc);
					Assert.NotNull(bifcCompressed);
					break;
				case Games.IcewindDale1:
					Assert.NotNull(bifc);
					Assert.Null(bifcCompressed);
					break;
				default:
					Assert.Null(bifc);
					Assert.Null(bifcCompressed);
					break;
			}
		}
	}
	
	[Theory]
	[MemberData(nameof(GamesData))]
	public void ReadBaseHeaderTest(Games game)
	{
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			BaseHeader? header = null;
			
			if(BifcFileName.ContainsKey(game))
			{
				header = SigReader.FromFile(Path.Combine(installPath, BifcFileName[game]));
				
				Assert.NotNull(header);
				Assert.Equal(Bifc.Signature, header.Signature);
				Assert.Equal(Bifc.Version, header.Version);
				
				header = null;
			}
			
			if(BifcCompressedFileName.ContainsKey(game))
			{
				header = SigReader.FromFile(Path.Combine(installPath, BifcCompressedFileName[game]));
				
				Assert.NotNull(header);
				Assert.Equal(BifcCompressed.Signature, header.Signature);
				Assert.Equal(BifcCompressed.Version, header.Version);
				
				header = null;
			}
			
			if(BiffFileName.ContainsKey(game))
			{
				header = SigReader.FromFile(Path.Combine(installPath, BiffFileName[game]));
				
				Assert.NotNull(header);
				Assert.Equal(Biff.Signature, header.Signature);
				Assert.Equal(Biff.Version, header.Version);
				
				header = null;
			}
			
			header = SigReader.FromFile(Path.Combine(installPath, InfinityEngineKey.FileName));
				
			Assert.NotNull(header);
			Assert.Equal(InfinityEngineKey.Signature, header.Signature);
			Assert.Equal(InfinityEngineKey.Version, header.Version);
		}
	}
}
