namespace InfinityEngineParser.Bif;

/// <summary>
/// <para>Metadata defining the contents of a BIFF V1 file.</para>
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
/// 		<term>char array</term>
/// 		<description>Signature ('BIFF')</description>
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
/// 		<description>Count of file entries</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000c</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Count of tileset entries</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0010</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Offset (from start of file) to file entries</description>
/// 	</item>
/// </list>
/// </summary>
public class BiffHeader : BaseHeader, IEquatable<BaseHeader>, IEquatable<BiffHeader>, FillFromReader
{
	public uint FileCount { get; set; }
	public uint TilesetCount { get; set; }
	public uint Offset { get; set; }
	
	public BiffHeader() {}
	public BiffHeader(BinaryReader reader) => Fill(reader);
	
	public bool Equals(BiffHeader? other)
	{
		return base.Equals(other)
			&& FileCount == other?.FileCount
			&& TilesetCount == other?.TilesetCount
			&& Offset == other?.Offset;
	}
	
	public new void Fill(BinaryReader reader)
	{
		base.Fill(reader);
		
		FileCount = reader.ReadUInt32();
		TilesetCount = reader.ReadUInt32();
		Offset = reader.ReadUInt32();
	}
}
