namespace InfinityEngineParser.Bif;

using InfinityEngineParser.Readers;

/// <summary>
/// <para>The parsed metadata and decompressed data of a BIFC V1.0 (compressed) file.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bif_v1.htm</see>
/// 
/// <para>
/// This file format is comprised of a header section, containing metadata about the BIF
/// file, and a set of blocks containing compressed data which, when decompressed and combined,
/// amount to a BIFF V1 file.
/// </para>
/// </summary>
public class BifcCompressed : FillFromReader
{
	public const string Signature = "BIFC";
	public const string Version = "V1.0";
	
	public BifcCompressedHeader? Header { get; set; }
	public Biff? Data { get; set; }
	
	public BifcCompressed() {}
	public BifcCompressed(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		Header = new(reader);
		
		List<BifcCompressedBlock> blocks = new();
		while(reader.BaseStream.Length > reader.BaseStream.Position)
		{
			blocks.Add(new(reader));
		}
		
		var bytes = new byte[Header.UncompressedBifSize];
		int actualSize = 0;
		blocks.ForEach(b => {
			if(b.CompressedData != null)
			{
				var decompressed = Bytes.DecompressBytes(b.CompressedData);
				Buffer.BlockCopy(decompressed, 0, bytes, actualSize, decompressed.Length);
				actualSize += decompressed.Length;
			}
		});
		
		Data = BifReader.BiffFromBytes(bytes);
	}
}
