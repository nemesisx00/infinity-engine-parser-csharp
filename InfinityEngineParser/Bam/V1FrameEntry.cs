namespace InfinityEngineParser.Bam;

/// <summary>
/// <para>Metadata defining the contents of a BAM V1 frame entry.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bif_v1.htm</see>
/// 
/// <para>
/// Cycles may share frames, which is accomplished by using a layer of indrection.
/// Instead of specifying which frames belong to a given cycle, each cycle has
/// a list of frame indices.
/// </para>
/// </summary>
///
/// <remarks>
/// <list type="table">
/// 	<listheader>
/// 		<term>Offset</term>
/// 		<term>Size</term>
/// 		<description>Description</description>
/// 	</listheader>
/// 	<item>
/// 		<term>0x0000</term>
/// 		<term>2</term>
/// 		<description>Frame width</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0002</term>
/// 		<term>2</term>
/// 		<description>Frame height</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0004</term>
/// 		<term>2</term>
/// 		<description>Frame center X coordinate</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0006</term>
/// 		<term>2</term>
/// 		<description>Frame center Y coordinate</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0008</term>
/// 		<term>4</term>
/// 		<description><list type="bullet">
/// 		<item><term>Bits 30-0</term><description>Offset to frame data</description></item>
/// 		<item><term>Bit 31</term><description>0 = Compressed (RLE), 1 = Uncompressed</description></item>
/// 		</list></description>
/// 	</item>
/// </list>
/// </remarks>
public class V1FrameEntry : FillFromReader
{
	private const int CompressionBit = 31;
	
	private uint offset;
	
	public ushort Width { get; set; }
	public ushort Height { get; set; }
	public short X { get; set; }
	public short Y { get; set; }
	
	public uint Offset
	{
		get { return offset; }
		
		set
		{
			offset = Bits.ReadValue(value, 30);
			Compressed = Bits.ReadBit(value, 31);
		}
	}
	
	public byte[]? Data { get; set; }
	
	public bool Compressed { get; private set; }
	
	public V1FrameEntry() {}
	public V1FrameEntry(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		Width = reader.ReadUInt16();
		Height = reader.ReadUInt16();
		X = reader.ReadInt16();
		Y = reader.ReadInt16();
		Offset = reader.ReadUInt32();
	}
	
	public void Fill(BinaryReader reader, bool data)
	{
		Fill(reader);
		if(data)
			FillData(reader);
	}
	
	public void FillData(BinaryReader reader)
	{
		reader.BaseStream.Seek(Offset, SeekOrigin.Begin);
		
		if(!Compressed)
			Data = reader.ReadBytes(Height * Width);
		else
		{
			//TODO: Implement decoding read operation
		}
	}
}
