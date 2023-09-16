namespace InfinityEngineParser.Bmp;

/// <summary>
/// <para>The contents of a BMP file's FileHeader.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bmp.htm</see>
///
/// <list type="table">
/// 	<listheader>
/// 		<term>Offset</term>
/// 		<term>Name</term>
/// 		<term>Size</term>
/// 		<description>Description</description>
/// 	</listheader>
/// 	<item>
/// 		<term>0x00</term>
/// 		<term>Signature</term>
/// 		<term>2</term>
/// 		<description>Signature ('BM')</description>
/// 	</item>
/// 	<item>
/// 		<term>0x02</term>
/// 		<term>FileSize</term>
/// 		<term>4</term>
/// 		<description>File size in bytes</description>
/// 	</item>
/// 	<item>
/// 		<term>0x06</term>
/// 		<term>Reserved</term>
/// 		<term>4</term>
/// 		<description>Reserved space - unused</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0a</term>
/// 		<term>DataOffset</term>
/// 		<term>4</term>
/// 		<description>File offset to Raster Data</description>
/// 	</item>
/// </list>
/// </summary>
public class FileHeader : AsBytes, FillFromReader
{
	/// <summary>
	/// The expected size, in bytes, of a BitmapFileHeader when rendered as a
	/// byte array.
	/// </summary>
	public const int ExpectedSize = 14;
	
	/// <summary>The file type signature.</summary>
	/// <remarks>Expected to be "BM"</remarks>
	public string Type { get; set; } = String.Empty;
	
	/// <summary>The size of the file in bytes</summary>
	public uint Size { get; set; }
	
	/// <summary>
	/// Reserved space but unused in Infinity Engine BMP files.
	/// </summary>
	/// <remarks>
	/// Technically, the specification denotes two WORDs that are reserved
	/// (bfReserved1, bfReserved2). But since they are both unused and
	/// contiguous, it makes sense to read them as a single DWORD.
	/// </remarks>
	public uint Reserved { get; set; }
	
	/// <summary>The offset to the beginning of the raster data.</summary>
	/// <remarks>
	/// This is indexed to the beginning of the BMP file entry, not the
	/// containing BIF file.
	/// </remarks>
	public uint DataOffset { get; set; }
	
	public FileHeader() {}
	public FileHeader(BinaryReader reader) => Fill(reader);
	
	public byte[] AsBytes()
	{
		List<byte> bytes = new();
		
		foreach(var c in Type.ToCharArray())
		{
			bytes.Add(Convert.ToByte(c));
		}
		
		bytes.AddRange(BitConverter.GetBytes(Size));
		bytes.AddRange(BitConverter.GetBytes(Reserved));
		bytes.AddRange(BitConverter.GetBytes(DataOffset));
		
		return bytes.ToArray();
	}
	
	public void Fill(BinaryReader reader)
	{
		var sigBytes = reader.ReadBytes(Bmp.Type.Length);
		Type = Bytes.ToString(sigBytes) ?? String.Empty;
		
		Size = reader.ReadUInt32();
		Reserved = reader.ReadUInt32();
		DataOffset = reader.ReadUInt32();
	}
}
