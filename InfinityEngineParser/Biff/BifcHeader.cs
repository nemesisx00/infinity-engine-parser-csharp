namespace InfinityEngineParser.Bif;

/// <summary>
/// <para>Metadata defining the contents of a BIFC V1 file.</para>
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
/// 		<description>Signature ('BIF ')</description>
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
/// 		<description>Length of filename</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000c</term>
/// 		<term>variable</term>
/// 		<term>ASCIIZ char array</term>
/// 		<description>Filename (length specified by previous field)</description>
/// 	</item>
/// 	<item>
/// 		<term>sizeof(filename) + 0x000c</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Uncompressed data length</description>
/// 	</item>
/// 	<item>
/// 		<term>sizeof(filename) + 0x0010</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Compressed data length</description>
/// 	</item>
/// 	<item>
/// 		<term>sizeof(filename) + 0x0014</term>
/// 		<term>variable</term>
/// 		<term>raw data</term>
/// 		<description>Compressed data</description>
/// 	</item>
/// </list>
/// </summary>
public class BifcHeader : BaseHeader, IEquatable<BaseHeader>, IEquatable<BifcHeader>, FillFromReader
{
	public uint FileNameLength { get; set; }
	public string FileName { get; set; } = String.Empty;
	public uint UncompressedDataLength { get; set; }
	public uint CompressedDataLength { get; set; }
	
	public BifcHeader() {}
	public BifcHeader(BinaryReader reader) => Fill(reader);
	
	public bool Equals(BifcHeader? other)
	{
		return base.Equals(other)
			&& FileNameLength == other?.FileNameLength
			&& FileName.Equals(other?.FileName)
			&& UncompressedDataLength == other?.UncompressedDataLength
			&& CompressedDataLength == other?.CompressedDataLength;
	}
	
	public new void Fill(BinaryReader reader)
	{
		base.Fill(reader);
		
		FileNameLength = reader.ReadUInt32();
		
		var fileNameBytes = reader.ReadBytes((int)FileNameLength);
		FileName = Bytes.ToString(fileNameBytes) ?? String.Empty;
		
		UncompressedDataLength = reader.ReadUInt32();
		CompressedDataLength = reader.ReadUInt32();
	}
}
