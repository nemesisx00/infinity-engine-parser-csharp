namespace InfinityEngineParser.Key;

/// <summary>
/// <para>Metadata defining the details of a resource file referenced in a given KEY V1 file.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/key_v1.htm</see>
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
/// 		<term>8</term>
/// 		<term>resref</term>
/// 		<description>Resource Name</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0008</term>
/// 		<term>2</term>
/// 		<term>word</term>
/// 		<description>Resource type</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000a</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>
/// 			Resource locator. The IE resource manager uses 32-bit values as
/// 			a 'resource index,' which codifies the source of the resources
/// 			as well as which source it refers to. The layout of this value is below:
/// 			<list type="bullet">
/// 				<item>
/// 					<term>Bits 31-20</term>
/// 					<description>Source index (the ordinal value giving the index of the corresponding BIF entry)</description>
/// 				</item>
/// 				<item>
/// 					<term>Bits 19-14</term>
/// 					<description>Tileset index</description>
/// 				</item>
/// 				<item>
/// 					<term>Bits 13-0</term>
/// 					<description>Non-tileset file index (any 12 bit value, so long as it matches the value used in the BIF file)</description>
/// 				</item>
/// 			</list>
/// 		</description>
/// 	</item>
/// </list>
/// </summary>
public class ResourceEntry : FillFromReader
{
	private const int LocatorMaskBitsFile = 14;
	private const int LocatorMaskBitsTileset = 6;
	private const int LocatorMaskBitsBif = 12;
	
	private uint locator;
	
	public string Name { get; set; } = String.Empty;
	public ushort Type { get; set; }
	public uint Locator
	{
		get { return locator; }
		
		set
		{
			locator = value;
			
			IndexFile = Bits.ReadValue(locator, LocatorMaskBitsFile);
			IndexTileset = Bits.ReadValue(locator, LocatorMaskBitsTileset, LocatorMaskBitsFile);
			IndexBifEntry = Bits.ReadValue(locator, LocatorMaskBitsBif, LocatorMaskBitsFile + LocatorMaskBitsTileset);
		}
	}
	
	public uint IndexFile { get; private set; }
	public uint IndexTileset { get; private set; }
	public uint IndexBifEntry { get; private set; }
	
	public ResourceEntry() {}
	public ResourceEntry(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		var nameBytes = reader.ReadBytes(MetadataLengths.RESREF);
		Name = Bytes.ToString(nameBytes) ?? String.Empty;
		Type = reader.ReadUInt16();
		Locator = reader.ReadUInt32();
	}
}
