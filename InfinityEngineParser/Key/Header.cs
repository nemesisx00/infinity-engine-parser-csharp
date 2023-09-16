namespace InfinityEngineParser.Key;

/// <summary>
/// <para>Metadata defining the contents of a KEY V1 file.</para>
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
/// 		<term>4</term>
/// 		<term>char array</term>
/// 		<description>Signature ('KEY ')</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0004</term>
/// 		<term>4</term>
/// 		<term>char array</term>
/// 		<description>Version ('V1  ')</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0008</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Count of BIF entries</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000c</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Count of resource entries</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0010</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Offset (from start of file) to BIF entries</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0014</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Offset (from start of file) to resource entries</description>
/// 	</item>
/// </list>
/// </summary>
public class Header : BaseHeader, IEquatable<BaseHeader>, IEquatable<Header>, FillFromReader
{
	public uint BifCount { get; set; }
	public uint ResourceCount { get; set; }
	public uint BifOffset { get; set; }
	public uint ResourceOffset { get; set; }
	
	public Header() {}
	public Header(BinaryReader reader) => Fill(reader);

	public bool Equals(Header? other)
	{
		return base.Equals(other)
			&& BifCount == other?.BifCount
			&& ResourceCount == other?.ResourceCount
			&& BifOffset == other?.BifOffset
			&& ResourceOffset == other?.ResourceOffset;
	}
	
	public new void Fill(BinaryReader reader)
	{
		base.Fill(reader);
		
		BifCount = reader.ReadUInt32();
		ResourceCount = reader.ReadUInt32();
		BifOffset = reader.ReadUInt32();
		ResourceOffset = reader.ReadUInt32();
	}
}
