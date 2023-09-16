namespace InfinityEngineParser.Bif;

/// <summary>
/// <para>Metadata defining the details of a file included in a given BIFF V1 file.</para>
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
///				Resource locator. Only bits 0-13 are matched against the file
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
/// 		<description>Size of this resource</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000c</term>
/// 		<term>2</term>
/// 		<term>word</term>
/// 		<description>Type of this resource</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000e</term>
/// 		<term>2</term>
/// 		<term>word</term>
/// 		<description>Unknown</description>
/// 	</item>
/// </list>
/// </summary>
public class FileEntry : FillFromReader
{
	private const int LocatorIndexMaskBits = 14;
	
	private uint locator;
	
	public uint Locator
	{
		get { return locator; }
		set
		{
			locator = value;
			Index = Bits.ReadValue(locator, LocatorIndexMaskBits);
		}
	}
	
	public uint Offset { get; set; }
	public uint Size { get; set; }
	public ushort Type { get; set; }
	public ushort Unknown { get; set; }
	
	public FileEntry() {}
	public FileEntry(BinaryReader reader) => Fill(reader);
	
	/// <summary>
	/// The non-tileset file index used to match this FileEntry to a ResourceEntry.
	/// </summary>
	public uint Index { get; private set; }
	
	public void Fill(BinaryReader reader)
	{
		Locator = reader.ReadUInt32();
		Offset = reader.ReadUInt32();
		Size = reader.ReadUInt32();
		Type = reader.ReadUInt16();
		Unknown = reader.ReadUInt16();
	}
}
