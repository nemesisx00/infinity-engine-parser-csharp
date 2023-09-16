namespace InfinityEngineParser.Bam;

using InfinityEngineParser.Readers;

/// <summary>
/// <para>The parsed metadata and decompressed data of a BAM V1 file.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bam_v1.htm</see>
/// 
/// <para>
/// This file format describes animated graphics. Such files are used for
/// animations (creature, item, and spell animations) and interactive GUI
/// elements (i.e. buttons), and for logical collections of images (i.e.
/// fonts). BAM files can contain multiple sequences of animations, up to
/// a limit of 255.
/// </para>
/// 
/// <remarks>
/// While the BAM format allows the dimensions of a frame to be very large, the
/// engine will only show frames up to a certain size. This maximum size varies
/// with the version of the engine:
/// <list type="table">
/// 	<item>
/// 		<term>Baldur's Gate 1</term>
/// 		<description>Unknown</description>
/// 	</item>
/// 	<item>
/// 		<term>Baldur's Gate 2</term>
/// 		<description>256*256</description>
/// 	</item>
/// 	<item>
/// 		<term>Planescape: Torment</term>
/// 		<description>Unknown (greater than 256*256)</description>
/// 	</item>
/// 	<item>
/// 		<term>Icewind Dale</term>
/// 		<description>Unknown</description>
/// 	</item>
/// 	<item>
/// 		<term>Icewind Dale 2</term>
/// 		<description>Unknown</description>
/// 	</item>
/// 	<item>
/// 		<term>Baldur's Gate 2: Enhanced Edition</term>
/// 		<description>Unknown (1024*1024 or greater)</description>
/// 	</item>
/// </list>
/// </remarks>
/// </summary>
public class BamV1 : FillFromReader
{
	public const string Signature = "Bam ";
	public const string Version = "V1  ";
	
	public V1Header? Header { get; set; }
	public List<V1FrameEntry> FrameEntries { get; set; } = new();
	public List<V1CycleEntry> CycleEntries { get; set; } = new();
	public List<ushort> FrameLookupTable { get; set; } = new();
	
	public BamV1() {}
	public BamV1(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		Header = new(reader);
		
		reader.BaseStream.Seek(Header.FrameEntryOffset, SeekOrigin.Begin);
		for(var i = 0; i < Header.FrameEntryCount; i++)
		{
			FrameEntries.Add(new(reader));
		}
		// Cycle entries follow immediately after frame entries
		for(var i = 0; i < Header.CycleCount; i++)
		{
			CycleEntries.Add(new(reader));
		}
		
		FillLookupTable(reader);
		FrameEntries.ForEach(fe => fe.FillData(reader));
	}
	
	private void FillLookupTable(BinaryReader reader)
	{
		if(Header != null && CycleEntries.Count > 0)
		{
			CycleEntries.Sort();
			var lastCycle = CycleEntries.Last();
			var lookupTableLength = lastCycle.Count + lastCycle.LookupIndex;
			
			reader.BaseStream.Seek(Header.LookupTableOffset, SeekOrigin.Begin);
			while(reader.BaseStream.Position <= lookupTableLength + Header.LookupTableOffset)
			{
				FrameLookupTable.Add(reader.ReadUInt16());
			}
		}
	}
}
