namespace InfinityEngineParser.Bif;

/// <summary>
/// <para>Metadata defining the contents of a BIFC V1.0 (compressed) compressed data block.</para>
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
/// 		<description>Decompressed size</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0004</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Compressed size</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0008</term>
/// 		<term>variable</term>
/// 		<term>byte array</term>
/// 		<description>Compressed data</description>
/// 	</item>
/// </list>
/// </summary>
public class BifcCompressedBlock : FillFromReader
{
	public uint DecompressedSize { get; set; }
	public uint CompressedSize { get; set; }
	public byte[]? CompressedData { get; set; }
	
	public BifcCompressedBlock() {}
	public BifcCompressedBlock(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		DecompressedSize = reader.ReadUInt32();
		CompressedSize = reader.ReadUInt32();
		CompressedData = reader.ReadBytes((int)CompressedSize);
	}
}
