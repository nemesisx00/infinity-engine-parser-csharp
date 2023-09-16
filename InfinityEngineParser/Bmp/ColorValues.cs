namespace InfinityEngineParser.Bmp;

/// <summary>
/// <para>The contents of one of a BMP file's Color values.</para>
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
/// 		<term>Red</term>
/// 		<term>1</term>
/// 		<description>Red intensity</description>
/// 	</item>
/// 	<item>
/// 		<term>0x01</term>
/// 		<term>Green</term>
/// 		<term>1</term>
/// 		<description>Green intensity</description>
/// 	</item>
/// 	<item>
/// 		<term>0x02</term>
/// 		<term>Blue</term>
/// 		<term>1</term>
/// 		<description>Blue intensity</description>
/// 	</item>
/// 	<item>
/// 		<term>0x04</term>
/// 		<term>Reserved</term>
/// 		<term>1</term>
/// 		<description>Unused</description>
/// 	</item>
/// </list>
/// </summary>
public class ColorValues : AsBytes, FillFromReader
{
	public byte Red { get; set; }
	public byte Green { get; set; }
	public byte Blue { get; set; }
	public byte Reserved { get; set; }
	
	public ColorValues() {}
	public ColorValues(BinaryReader reader) => Fill(reader);
	
	public byte[] AsBytes()
	{
		return new byte[] { Red, Green, Blue, Reserved };
	}

	public void Fill(BinaryReader reader)
	{
		Red = reader.ReadByte();
		Green = reader.ReadByte();
		Blue = reader.ReadByte();
		Reserved = reader.ReadByte();
	}
}
