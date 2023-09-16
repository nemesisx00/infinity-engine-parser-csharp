namespace InfinityEngineParser.Bam;

/// <summary>
/// <para>Metadata defining the contents of a BAM V1 cycle entry.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bif_v1.htm</see>
/// 
/// <para>
/// These entries refer to a range of indices in the rame lookup table, which
/// in turn points to the actual frames. Note that entries in the frame lookup
/// table can also be shared between cycles.
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
/// 		<description>Count of frame indices in this cycle</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0002</term>
/// 		<term>2</term>
/// 		<description>Index of the first frame in this cycle, found in the frame lookup table</description>
/// 	</item>
/// </list>
/// </remarks>
public class V1CycleEntry : FillFromReader, IComparable<V1CycleEntry>
{
	public ushort Count { get; set; }
	public ushort LookupIndex { get; set; }
	
	public V1CycleEntry() {}
	public V1CycleEntry(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		Count = reader.ReadUInt16();
		LookupIndex = reader.ReadUInt16();
	}

	public int CompareTo(V1CycleEntry? other)
	{
		return (Count + LookupIndex).CompareTo(other?.Count + other?.LookupIndex);
	}
}
