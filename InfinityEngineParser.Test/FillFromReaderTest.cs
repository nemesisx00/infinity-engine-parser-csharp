namespace InfinityEngineParser.Test;

using InfinityEngineParser.Bif;
using InfinityEngineParser.Key;

#pragma warning disable xUnit1026

[Collection("ReadsKeyFile")]
public class FillFromReaderTest
{
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
	public void FillTest(Games game)
	{
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var filePath = Path.Combine(installPath, InfinityEngineKey.FileName);
			using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
			{
				BaseHeader header = new(reader);
				Assert.True(reader.BaseStream.Position > 0);
				Assert.Equal(InfinityEngineKey.Signature, header.Signature);
				Assert.Equal(InfinityEngineKey.Version, header.Version);
			}
		}
	}
	
	[Theory]
	[MemberData(nameof(GamesData))]
	public void FillWithOffsetTest(Games game)
	{
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var filePath = Path.Combine(installPath, InfinityEngineKey.FileName);
			using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
			{
				BaseHeader header1 = new(reader);
				Assert.True(reader.BaseStream.Position > 0);
				
				BaseHeader header2 = new();
				Assert.NotEqual(header1, header2);
				
				header2.Fill(reader, 0);
				Assert.Equal(header1, header2);
			}
		}
	}
	
	[Theory]
	[MemberData(nameof(GamesData))]
	public void CrossHeaderComparisonTest(Games game)
	{
		var installPath = GamePaths.FindInstallationPath(game);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			var filePath = Path.Combine(installPath, InfinityEngineKey.FileName);
			using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
			{
				BaseHeader header1 = new(reader);
				Assert.True(reader.BaseStream.Position > 0);
				
				Key.Header header2 = new();
				header2.Fill(reader, 0);
				
				BiffHeader header3 = new();
				header3.Fill(reader, 0);
				
				Assert.Equal(header1, header2);
				Assert.Equal(header1, header3);
				Assert.True(header2.Equals(header3));
			}
		}
	}
}

#pragma warning restore xUnit1026
