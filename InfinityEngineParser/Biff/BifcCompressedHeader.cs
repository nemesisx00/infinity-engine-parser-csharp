namespace InfinityEngineParser.Bif;

/// <summary>
/// <para>Metadata defining the contents of a BIFC V1.0 (compressed) header.</para>
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
/// 		<description>Signature ('BIFC')</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0004</term>
/// 		<term>4</term>
/// 		<term>char array</term>
/// 		<description>Version ('V1.0')</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0008</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Uncompressed BIF size</description>
/// 	</item>
/// </list>
/// </summary>
public class BifcCompressedHeader : BaseHeader, IEquatable<BaseHeader>, IEquatable<BifcCompressedHeader>, FillFromReader
{
	public uint UncompressedBifSize { get; set; }
	
	public BifcCompressedHeader() {}
	public BifcCompressedHeader(BinaryReader reader) => Fill(reader);
	
	public bool Equals(BifcCompressedHeader? other)
	{
		return base.Equals(other)
			&& UncompressedBifSize == other?.UncompressedBifSize;
	}
	
	public new void Fill(BinaryReader reader)
	{
		base.Fill(reader);
		
		UncompressedBifSize = reader.ReadUInt32();
	}
}
