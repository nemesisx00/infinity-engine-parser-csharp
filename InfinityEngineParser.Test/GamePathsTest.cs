namespace InfinityEngineParser.Test;

using InfinityEngineParser;

public class GamePathsTest
{
	public static IEnumerable<object[]> GamesData()
		=> new object[][]
		{
			new object[] { Games.BaldursGate1 },
			new object[] { Games.BaldursGate1EnhancedEdition },
			new object[] { Games.BaldursGate2 },
			new object[] { Games.BaldursGate2EnhancedEdition },
			new object[] { Games.IcewindDale1 },
			new object[] { Games.IcewindDale2 },
		};
	
	[Theory]
	[MemberData(nameof(GamesData))]
	public void FindInstallationPathTest(Games game)
	{
		var result = GamePaths.FindInstallationPath(game);
		Assert.NotNull(result);
		Assert.NotEmpty(result);
	}
}
