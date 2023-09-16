namespace InfinityEngineParser.Bif;

using InfinityEngineParser.Readers;

/// <summary>
/// <para>The parsed metadata and decompressed data of a BIFC V1 file.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bif_v1.htm</see>
/// 
/// <para>
/// This file format is comprised of a header section, containing metadata about the BIF
/// file, and the compressed data of a standard BIFF V1 file.
/// </para>
/// </summary>
public class Bifc : FillFromReader
{
	public const string Signature = "BIF ";
	public const string Version = "V1.0";
	
	public BifcHeader? Header { get; set; }
	public Biff? Data { get; set; }
	
	public Bifc() {}
	public Bifc(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		Header = new(reader);
		
		var compressed = reader.ReadBytes((int)Header.CompressedDataLength);
		byte[] bytes = Bytes.DecompressBytes(compressed);
		Data = BifReader.BiffFromBytes(bytes);
	}
}
