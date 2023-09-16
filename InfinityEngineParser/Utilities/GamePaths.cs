namespace InfinityEngineParser;

using InfinityEngineParser.PlatformSpecific;

public class GamePaths
{
	public readonly static Dictionary<Games, int> SteamAppIds = new()
	{
		{ Games.BaldursGate1, 24431 },
		{ Games.BaldursGate1EnhancedEdition, 228280 },
		{ Games.BaldursGate2, 99140 },
		{ Games.BaldursGate2EnhancedEdition, 257350 },
		{ Games.IcewindDale1, 206940 },
		{ Games.IcewindDale1EnhancedEdition, 321800 },
		{ Games.IcewindDale2, 206950 },
		{ Games.PlanescapeTorment, 205180 },
		{ Games.PlanescapeTormentEnhancedEdition, 466300},
	};
	
	// GOG Galaxy / modern installations
	public readonly static Dictionary<Games, int> GogGameIds = new()
	{
		{ Games.BaldursGate1, 1207658886 },
		{ Games.BaldursGate1EnhancedEdition, 1207666353 },
		{ Games.BaldursGate2, 1207658893 },
		{ Games.BaldursGate2EnhancedEdition, 1207666373 },
		{ Games.IcewindDale1, 1207658888 },
		{ Games.IcewindDale1EnhancedEdition, 1207666683 },
		{ Games.IcewindDale2, 1207658891 },
		{ Games.PlanescapeTorment, 1207658887 },
		{ Games.PlanescapeTormentEnhancedEdition, 1203613131 },
	};
	
	public readonly static Dictionary<Games, string> KeyFileNames = new()
	{
		{ Games.BaldursGate1, "Chitin.key" },
		{ Games.BaldursGate1EnhancedEdition, "chitin.key" },
		{ Games.BaldursGate2, "CHITIN.KEY" },
		{ Games.BaldursGate2EnhancedEdition, "chitin.key" },
		{ Games.IcewindDale1, "CHITIN.KEY" },
		{ Games.IcewindDale1EnhancedEdition, "chitin.key" },
		{ Games.IcewindDale2, "CHITIN.KEY" },
		{ Games.PlanescapeTorment, "CHITIN.KEY" },
		{ Games.PlanescapeTormentEnhancedEdition, "chitin.key" },
	};
	
	public static string? FindInstallationPath(Games game = Games.None)
	{
		string? path = null;
		
		//TODO: Implement functionality for other platforms
		if(OperatingSystem.IsLinux()) {}
		if(OperatingSystem.IsMacOS()) {}
		if(OperatingSystem.IsWindows())
			path = GamePathsWindows.FindInstallationPath(game);
		
		return path;
	}
}
