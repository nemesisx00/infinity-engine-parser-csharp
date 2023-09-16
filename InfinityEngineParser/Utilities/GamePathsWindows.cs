namespace InfinityEngineParser.PlatformSpecific;

using System.Runtime.Versioning;
using Microsoft.Win32;

[SupportedOSPlatform("windows")]
public class GamePathsWindows
{
	//Non-Galaxy / old GOG installations
	private static Dictionary<Games, string> OldGogUninstallKeys = new()
	{
		{ Games.BaldursGate1, "GOGPACKBALDURSGATE1_is1" },
		{ Games.BaldursGate2, "GOGPACKBALDURSGATE2_is1" },
		{ Games.IcewindDale1, "GOGPACKICEWINDDALE1_is1" },
	};
	
	private static List<string> WindowsRegistrySearchKeys = new()
	{
		"Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
		"Software\\WOW6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall",
		"Software\\WOW6432Node\\GOG.com\\"
	};
	
	private const string WindowsRegistryValueName_DisplayName = "DisplayName";
	private const string WindowsRegistryValueName_InstallLocation = "InstallLocation";
	private const string WindowsRegistryValueName_NewGog_GameId = "gameID";
	private const string WindowsRegistryValueName_NewGog_Path = "path";
	
	private static Dictionary<Games, string[]> GameDisplayNames = new()
	{
		{ Games.BaldursGate1, new string[] { "Baldur's Gate -  The Original Saga" } },
		{ Games.BaldursGate1EnhancedEdition, new string[] { "Baldur's Gate: Enhanced Edition" } },
		{ Games.BaldursGate2, new string[] { "Baldur's Gate 2 Complete" } },
		{ Games.BaldursGate2EnhancedEdition, new string[] { "Baldur's Gate II: Enhanced Edition" } },
		{ Games.IcewindDale1, new string[] { "Icewind Dale Complete" } },
		{ Games.IcewindDale1EnhancedEdition, new string[] { "Icewind Dale: Enhanced Edition" } },
		{ Games.IcewindDale2, new string[] { "Icewind Dale 2" } },
		{ Games.PlanescapeTorment, new string[] { "Planescape: Torment" } },
		{ Games.PlanescapeTormentEnhancedEdition, new string[] { "Planescape: Torment - Enhanced Edition" } },
	};
	
	public static string? FindInstallationPath(Games game = Games.None)
	{
		string? path = null;
		
		if(!game.Equals(Games.None))
		{
			var gogGameId = GamePaths.GogGameIds[game];
			string? oldGogUninstallKey = null;
			if(OldGogUninstallKeys.ContainsKey(game))
				oldGogUninstallKey = OldGogUninstallKeys[game];
			int? steamAppId = null;
			if(GamePaths.SteamAppIds.ContainsKey(game))
				steamAppId = GamePaths.SteamAppIds[game];
			
			WindowsRegistrySearchKeys.ForEach(regKey => {
				using(var key = Registry.LocalMachine.OpenSubKey(regKey))
				{
					if(key != null)
					{
						var query = key.GetSubKeyNames().Where(name => name.Equals($"{gogGameId}_is1")
							|| name.Equals(gogGameId.ToString())
							|| (steamAppId != null && name.Equals($"Steam App {steamAppId}"))
							|| (!String.IsNullOrEmpty(oldGogUninstallKey) && name.Equals(oldGogUninstallKey)));
						
						if(query.Any())
						{
							(var gameKey, var newGog) = query
								.Select(name => (name, name.Equals(gogGameId.ToString())))
								.First();
							
							if(!String.IsNullOrEmpty(gameKey))
							{
								if(newGog)
									path = CheckNewGog(key, gameKey, game);
								else
									path = CheckDisplayName(key, gameKey, game);
							}
						}
					}
				}
			});
		}
		
		return path;
	}
	
	private static string? CheckDisplayName(RegistryKey key, string keyName, Games game)
	{
		string? path = null;
		using(var subKey = key.OpenSubKey(keyName))
		{
			if(subKey != null && GameDisplayNames[game].Contains(subKey.GetValue(WindowsRegistryValueName_DisplayName)))
				path = subKey.GetValue(WindowsRegistryValueName_InstallLocation) as string;
		}
		return path;
	}
	
	private static string? CheckNewGog(RegistryKey key, string keyName, Games game)
	{
		string? path = null;
		using(var subKey = key.OpenSubKey(keyName))
		{
			if(subKey != null && GameDisplayNames[game].Contains(subKey.GetValue(WindowsRegistryValueName_NewGog_GameId)))
				path = subKey.GetValue(WindowsRegistryValueName_NewGog_Path) as string;
		}
		return path;
	}
}
