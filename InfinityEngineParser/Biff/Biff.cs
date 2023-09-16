namespace InfinityEngineParser.Bif;

/// <summary>
/// <para>The fully parsed metadata contents of a BIFF V1 file.</para>
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bif_v1.htm</see>
/// </summary> 
/// 
/// <remarks>
/// <para>
/// This file format is a simple archive format, used mainly both to simplify
/// organization of the files by grouping logically related files together
/// (especially for areas). There is also a gain from having few large files
/// rather than many small files, due to the wastage in the FAT and NTFS file
/// systems. BIF files containing areas typically contain:
/// </para>
///
/// <list type="bullet">
/// 	<item><description>One ore more WED files, detailing tiles and wallgroups</description></item>
/// 	<item><description>One or more TIS files, containing the tileset itself</description></item>
/// 	<item><description>One or more MOS files, containing the minimap graphic</description></item>
/// 	<item><description>Three or four bitmap files which contain one pixel for each tile needed to cover the region</description></item>
/// </list>
///
/// <para>
/// The bitmaps are named xxxxxxHT.BMP, xxxxxxLM.BMP, xxxxxxSR.BMP, and
/// optionally xxxxxxLN.BMP
/// </para>
///
/// <list type="bullet">
/// 	<item>
/// 		<term>xxxxxxHT.BMP</term>
/// 		<description>Height map, detailing altitude of each tile cell in the associated WED file</description>
/// 	</item>
/// 	<item>
/// 		<term>xxxxxxLM.BMP</term>
/// 		<description>Light map, detailing the level and color of illumination for each tile cell on the map. Used during daytime</description>
/// 	</item>
/// 	<item>
/// 		<term>xxxxxxLN.BMP</term>
/// 		<description>Light map, detailing the level and color of illumination for each tile cell on the map. Used during nighttime</description>
/// 	</item>
/// 	<item>
/// 		<term>xxxxxxSR.BMP</term>
/// 		<description>Search Map, detailing where characters cannot walk and the footstep sounds</description>
/// 	</item>
/// </list>
/// </remarks>
public class Biff : FillFromReader
{
	public const string Signature = "BIFF";
	public const string Version = "V1  ";
	
	public BiffHeader? Header { get; set; }
	public List<FileEntry> FileEntries { get; set; } = new();
	public List<TilesetEntry> TilesetEntries { get; set; } = new();
	
	public Biff() {}
	public Biff(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		Header = new(reader);
		
		for(uint i = 0; i < Header.FileCount; i++)
		{
			FileEntries.Add(new(reader));
		}
		
		for(uint i = 0; i < Header.TilesetCount; i++)
		{
			TilesetEntries.Add(new(reader));
		}
	}
}
