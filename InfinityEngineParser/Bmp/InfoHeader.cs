namespace InfinityEngineParser.Bmp;

/// <summary>
/// <para>The contents of a BMP file's InfoHeader.</para>
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
/// 		<term>0x0e</term>
/// 		<term>Size</term>
/// 		<term>4</term>
/// 		<description>Size of InfoHeader - 40</description>
/// 	</item>
/// 	<item>
/// 		<term>0x12</term>
/// 		<term>Width</term>
/// 		<term>4</term>
/// 		<description>Bitmap Width</description>
/// 	</item>
/// 	<item>
/// 		<term>0x16</term>
/// 		<term>Height</term>
/// 		<term>4</term>
/// 		<description>Bitmap Height</description>
/// 	</item>
/// 	<item>
/// 		<term>0x1a</term>
/// 		<term>Planes</term>
/// 		<term>2</term>
/// 		<description>Number of Planes</description>
/// 	</item>
/// 	<item>
/// 		<term>0x1c</term>
/// 		<term>BitCount</term>
/// 		<term>2</term>
/// 		<description>Bits per Pixel</description>
/// 	</item>
/// 	<item>
/// 		<term>0x1e</term>
/// 		<term>Compression</term>
/// 		<term>4</term>
/// 		<description>Type of Compression</description>
/// 	</item>
/// 	<item>
/// 		<term>0x22</term>
/// 		<term>ImageSize</term>
/// 		<term>4</term>
/// 		<description>Size of the image</description>
/// 	</item>
/// 	<item>
/// 		<term>0x26</term>
/// 		<term>XpixelsPerM</term>
/// 		<term>4</term>
/// 		<description>Horizontal resolution: pixels/meter</description>
/// 	</item>
/// 	<item>
/// 		<term>0x2a</term>
/// 		<term>YpixelsPerM</term>
/// 		<term>4</term>
/// 		<description>Vertical resolution: pixels/meter</description>
/// 	</item>
/// 	<item>
/// 		<term>0x2e</term>
/// 		<term>ColorsUsed</term>
/// 		<term>4</term>
/// 		<description>Number of actually used colors</description>
/// 	</item>
/// 	<item>
/// 		<term>0x32</term>
/// 		<term>ColorsImportant</term>
/// 		<term>4</term>
/// 		<description>Number of important colors (0 = all)</description>
/// 	</item>
/// 	<item>
/// 		<term>0x36</term>
/// 		<term>ColorTable</term>
/// 		<term>variable</term>
/// 		<description>4 bytes * ColorsUsed value</description>
/// 	</item>
/// </list>
/// </summary>
public class InfoHeader : AsBytes, FillFromReader
{
	/// <summary>The size of the BitmapInfoHeader in bytes.</summary>
	public uint Size { get; set; }
	
	/// <summary>The width of the bitmap in pixels.</summary>
	public int Width { get; set; }
	
	/// <summary>The height of the bitmap in pixels.</summary>
	public int Height { get; set; }
	
	/// <summary>The number of planes represented in this bitmap.</summary>
	/// <remarks>Expected to always be 1.</remarks>
	public ushort Planes { get; set; }
	
	/// <summary>The number of bits used to define a single pixel.</summary>
	/// <remarks>
	/// This is used to calculate the number of possible colors that can be used
	/// in the bitmap, which subsequently defines whether or not there is a
	/// color table to be parsed.
	/// </remarks>
	public ushort BitsPerPixel { get; set; }
	
	/// <summary>The type of compression used when the data was encoded.</summary>
	/// <remarks>
	/// It seems that only two of the potential compression methods were used
	/// for Infinity Engine BMP files, specifically for 4-bit and 8-bit bitmaps.
	/// </remarks>
	public uint Compression { get; set; }
	
	/// <summary>The size of the bitmap, in bytes, after compression.</summary>
	/// <remarks>
	/// This value will still be present even if there is no compression
	/// (Compression == 0). In that case, the value is expected to be 0.
	/// </remarks>
	public uint CompressedSize { get; set; }
	
	/// <summary>The number of pixels per meter along the horizontal axis.</summary>
	public int ResolutionHorizontal { get; set; }
	
	/// <summary>The number of pixels per meter along the vertical axis.</summary>
	public int ResolutionVertical { get; set; }
	
	/// <summary>
	/// The number of color indices in the color table which are actually used
	/// by the bitmap.
	/// </summary>
	/// <remarks>
	/// This value defines the size of the color table. If this value is 0,
	/// the table's size is the maximum number of colors based on BitsPerPixel.
	/// </remarks>
	public uint ColorsUsed { get; set; }
	
	/// <summary>
	/// The number of color indices in the color table which are considered
	/// important for displaying the bitmap.
	/// </summary>
	/// <remarks>
	/// If set to 0, all colors are important.
	/// </remarks>
	public uint ColorsImportant { get; set; }
	
	public InfoHeader() {}
	public InfoHeader(BinaryReader reader) => Fill(reader);
	
	public byte[] AsBytes()
	{
		List<byte> bytes = new();
		
		bytes.AddRange(BitConverter.GetBytes(Size));
		bytes.AddRange(BitConverter.GetBytes(Width));
		bytes.AddRange(BitConverter.GetBytes(Height));
		bytes.AddRange(BitConverter.GetBytes(Planes));
		bytes.AddRange(BitConverter.GetBytes(BitsPerPixel));
		bytes.AddRange(BitConverter.GetBytes(Compression));
		bytes.AddRange(BitConverter.GetBytes(CompressedSize));
		bytes.AddRange(BitConverter.GetBytes(ResolutionHorizontal));
		bytes.AddRange(BitConverter.GetBytes(ResolutionVertical));
		bytes.AddRange(BitConverter.GetBytes(ColorsUsed));
		bytes.AddRange(BitConverter.GetBytes(ColorsImportant));
		
		return bytes.ToArray();
	}
	
	public void Fill(BinaryReader reader)
	{
		Size = reader.ReadUInt32();
		Width = reader.ReadInt32();
		Height = reader.ReadInt32();
		Planes = reader.ReadUInt16();
		BitsPerPixel = reader.ReadUInt16();
		Compression = reader.ReadUInt32();
		CompressedSize = reader.ReadUInt32();
		ResolutionHorizontal = reader.ReadInt32();
		ResolutionVertical = reader.ReadInt32();
		ColorsUsed = reader.ReadUInt32();
		ColorsImportant = reader.ReadUInt32();
	}
}
