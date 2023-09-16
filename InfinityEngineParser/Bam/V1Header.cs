namespace InfinityEngineParser.Bam;

/// <summary>
/// <para>Metadata defining the contents of a BAM V1 header.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bif_v1.htm</see>
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
/// 		<term>4</term>
/// 		<description>Signature ('BAM ')</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0004</term>
/// 		<term>4</term>
/// 		<description>Version ('V1  ')</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0008</term>
/// 		<term>2</term>
/// 		<description>Count of frame entries</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000a</term>
/// 		<term>1</term>
/// 		<description>Count of cycles</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000b</term>
/// 		<term>1</term>
/// 		<description>The compressed color index for RLE encoded bams</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000c</term>
/// 		<term>4</term>
/// 		<description>Offset to frame entries</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0010</term>
/// 		<term>4</term>
/// 		<description>Offset to palette</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0014</term>
/// 		<term>4</term>
/// 		<description>Offset to frame lookup table</description>
/// 	</item>
/// </list>
/// </remarks>
public class V1Header : BaseHeader, IEquatable<BaseHeader>, IEquatable<V1Header>, FillFromReader
{
	public ushort FrameEntryCount { get; set; }
	public byte CycleCount { get; set; }
	public byte CompressedColorIndex { get; set; }
	public uint FrameEntryOffset { get; set; }
	public uint PaletteOffset { get; set; }
	public uint LookupTableOffset { get; set; }
	
	public V1Header() {}
	public V1Header(BinaryReader reader) => Fill(reader);
	
	public bool Equals(V1Header? other)
	{
		return base.Equals(other)
			&& FrameEntryCount == other?.FrameEntryCount
			&& CycleCount == other?.CycleCount
			&& CompressedColorIndex == other?.CompressedColorIndex
			&& FrameEntryOffset == other?.FrameEntryOffset
			&& PaletteOffset == other?.PaletteOffset
			&& LookupTableOffset == other?.LookupTableOffset;
	}
	
	public new void Fill(BinaryReader reader)
	{
		base.Fill(reader);
		
		FrameEntryCount = reader.ReadUInt16();
		CycleCount = reader.ReadByte();
		CompressedColorIndex = reader.ReadByte();
		FrameEntryOffset = reader.ReadUInt32();
		PaletteOffset = reader.ReadUInt32();
		LookupTableOffset = reader.ReadUInt32();
	}
}
