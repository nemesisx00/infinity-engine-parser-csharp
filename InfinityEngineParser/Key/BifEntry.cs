namespace InfinityEngineParser.Key;

/// <summary>
/// <para>Metadata defining the details of a BIF file referenced in a given KEY V1 file.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/key_v1.htm</see>
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
/// 		<description>Length of BIF file</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0004</term>
/// 		<term>4</term>
/// 		<term>dword</term>
/// 		<description>Offset from start of file to ASCIIZ BIF filename</description>
/// 	</item>
/// 	<item>
/// 		<term>0x0008</term>
/// 		<term>2</term>
/// 		<term>word</term>
/// 		<description>Length, including terminating NUL, of ASCIIZ BIF filename</description>
/// 	</item>
/// 	<item>
/// 		<term>0x000a</term>
/// 		<term>2</term>
/// 		<term>word</term>
/// 		<description>
/// 			The 16 bits of this field are used individually to mark the location
/// 			of the relevant file.
/// 			<para>(MSB) xxxx xxxx ABCD EFGH (LSB)</para>
/// 			<list type="bullet">
/// 				<item><description>Bits marked A to F determine on which CD the file is stored (A = CD6, F = CD1)</description></item>
/// 				<item><description>Bit G determines if the file is in the \cache directory</description></item>
/// 				<item><description>Bit H determines if the file is in the \data directory</description></item>
/// 			</list>
/// 		</description>
/// 	</item>
/// </list>
/// </summary>
public class BifEntry : FillFromReader
{
	private const int BitInDataDirectory = 0;
	private const int BitInCacheDirectory = 1;
	private const int BitOnCd1 = 2;
	private const int BitOnCd2 = 3;
	private const int BitOnCd3 = 4;
	private const int BitOnCd4 = 5;
	private const int BitOnCd5 = 6;
	private const int BitOnCd6 = 7;
	
	public string? FileName { get; set; }
	public uint FileLength { get; set; }
	public uint FileNameOffset { get; set; }
	public ushort FileNameLength { get; set; }
	public ushort Locator { get; set; }
	
	public bool InData { get { return Bits.ReadBit(Locator, BitInDataDirectory); } }
	public bool InCache { get { return Bits.ReadBit(Locator, BitInCacheDirectory); } }
	public bool OnCd1 { get { return Bits.ReadBit(Locator, BitOnCd1); } }
	public bool OnCd2 { get { return Bits.ReadBit(Locator, BitOnCd2); } }
	public bool OnCd3 { get { return Bits.ReadBit(Locator, BitOnCd3); } }
	public bool OnCd4 { get { return Bits.ReadBit(Locator, BitOnCd4); } }
	public bool OnCd5 { get { return Bits.ReadBit(Locator, BitOnCd5); } }
	public bool OnCd6 { get { return Bits.ReadBit(Locator, BitOnCd6); } }
	
	public BifEntry() {}
	public BifEntry(BinaryReader reader) => Fill(reader);
	
	public void Fill(BinaryReader reader)
	{
		FileLength = reader.ReadUInt32();
		FileNameOffset = reader.ReadUInt32();
		FileNameLength = reader.ReadUInt16();
		Locator = reader.ReadUInt16();
	}
	
	/// <summary>
	/// Read this entry's file name as a string from <paramref name="reader" />.
	/// </summary>
	/// <param name="reader">The data source being read.</param>
	/// <remarks>
	/// Alters the current position of the <paramref name="reader" /> based on
	/// the <c>FileNameOffset</c> and <c>FileNameLength</c> properties before
	/// reading.
	/// </remarks>
	public void FillName(BinaryReader reader)
	{
		FileName = ReadUtility.ReadStringAt(reader, FileNameOffset, FileNameLength);
	}
}
