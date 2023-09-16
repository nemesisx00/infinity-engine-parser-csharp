namespace InfinityEngineParser.Bif;

/// <summary>
/// <para>Metadata defining the details of a tileset included in a given BIFF V1 file.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bif_v1.htm</see>
///
/// <list type="table">
/// 	<listheader>
/// 		<term>Offset</term>
/// 		<term>Size</term>
/// 		<term>Data Type</term>
/// 		<description>Description</description>
/// 	</listheader>
/// 	<item>
/// 		<term>0x0000</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>
///				Resource locator. Only bits 14-19 are matched against the file
///				index in the "resource locator" field from the KEY file resource
///				entry.
///			</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0004</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Offset (from start of file) to resource data</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0008</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Count of tiles in this resource</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000c</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Size of each tile in this resource</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0010</term>
/// 		<term>2</term>
/// 		<term>word</term>
/// 		<description>Type of this resource (always 0x3eb - TIS)</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0012</term>
/// 		<term>2</term>
/// 		<term>word</term>
/// 		<description>Unknown</description>
/// 	</item>
/// </list>
/// </summary>
public class TilesetEntry : FillFromReader
{
	private const int LocatorIndexShift = 14;
	private const int LocatorIndexMaskBits = 6;
	
	private uint locator;
	
	public uint Locator
	{
		get { return locator; }
		set
		{
			locator = value;
			Index = Bits.ReadValue(locator, LocatorIndexMaskBits, LocatorIndexShift);
		}
	}
	
	public uint Offset { get; set; }
	public uint TileCount { get; set; }
	public uint TileSize { get; set; }
	public ushort Type { get; set; }
	public ushort Unknown { get; set; }
	
	public TilesetEntry() {}
	public TilesetEntry(BinaryReader reader) => Fill(reader);
	
	/// <summary>
	/// The tileset index used to match this TilesetEntry to a ResourceEntry.
	/// </summary>
	public uint Index { get; private set; }
	
	public void Fill(BinaryReader reader)
	{
		Locator = reader.ReadUInt32();
		Offset = reader.ReadUInt32();
		TileCount = reader.ReadUInt32();
		TileSize = reader.ReadUInt32();
		Type = reader.ReadUInt16();
		Unknown = reader.ReadUInt16();
	}
}
