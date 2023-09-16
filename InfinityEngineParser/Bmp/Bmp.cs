namespace InfinityEngineParser.Bmp;

/// <summary>
/// <para>The fully parsed contents of a BMP file.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bmp.htm</see>
/// 
/// <para>
/// This file format is the MS-Windows standard format. It holds black & white,
/// 16-color, and 256-color images which may be compressed via run length encoding.
/// Notice there is also an OS/2-BMP format.
/// </para>
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
/// 		<term>FileHeader</term>
/// 		<term>14</term>
/// 		<description>Windows Structure: BITMAPFILEHEADER</description>
/// 	</item>
/// 	<item>
/// 		<term>FileHeader size</term>
/// 		<term>InfoHeader</term>
/// 		<term>40</term>
/// 		<description>Windows Structure: BITMAPINFOHEADER</description>
/// 	</item>
/// 	<item>
/// 		<term>FileHeader size + InfoHeader size</term>
/// 		<term>RasterData</term>
/// 		<term>variable</term>
/// 		<description>The pixel data</description>
/// 	</item>
/// </list>
/// </summary>
public class Bmp : AsBytes, FillFromReader
{
	public const string Type = "BM";
	
	public FileHeader? File { get; set; }
	public InfoHeader? Info { get; set; }
	public List<ColorValues>? ColorTable { get; set; }
	public List<byte>? Data { get; set; }
	
	public int DecodedSize
	{
		get
		{
			var fileSize = File?.Size ?? 0;
			var bitCount = Info?.BitsPerPixel ?? 0;
			
			switch(bitCount)
			{
				case (ushort)BPP.Monochrome:
					return (int)fileSize * 32;
				case (ushort)BPP.Palletized4bit:
					if(Info?.Compression == (uint)CompressionType.BI_RGB)
						return (int)fileSize * 8;
					else
						return 0;
				case (ushort)BPP.Palletized8bit:
					if(Info?.Compression == (uint)CompressionType.BI_RGB)
						return (int)fileSize * 4;
					else
						return 0;
				default:
					return (int)fileSize;
			}
		}
	}
	
	public Bmp() {}
	public Bmp(BinaryReader reader) => Fill(reader);
	
	public byte[] AsBytes()
	{
		List<byte> bytes = new();
		if(File != null && Info != null && Data != null)
		{
			bytes.AddRange(File.AsBytes());
			bytes.AddRange(Info.AsBytes());
			if(ColorTable?.Count > 0)
				ColorTable.ForEach(color => bytes.AddRange(color.AsBytes()));
			bytes.AddRange(Data.ToArray());
		}
		return bytes.ToArray();
	}
	
	public void Fill(BinaryReader reader)
	{
		var entryOffset = reader.BaseStream.Position;
		
		File = new(reader);
		Info = new(reader);
		
		BuildColorTable(reader);
		DecodeRasterData(reader, entryOffset);
	}
	
	private void BuildColorTable(BinaryReader reader)
	{
		if(Info != null)
		{
			switch(Info.BitsPerPixel)
			{
				case (ushort)BPP.Monochrome:
				case (ushort)BPP.Palletized4bit:
				case (ushort)BPP.Palletized8bit:
					var colorCount = (int)Info.ColorsUsed;
					if(Info.ColorsUsed == 0)
						colorCount = 1 << Info.BitsPerPixel;
					
					ColorTable = new();
					for(int i = 0; i < colorCount; i++)
					{
						ColorTable.Add(new(reader));
					}
					break;
			}
		}
	}
	
	private void DecodeRasterData(BinaryReader reader, long entryOffset)
	{
		if(File != null && Info != null)
		{
			var fileSize = File.Size;
			if(Info.Compression != (uint)CompressionType.BI_RGB && Info.CompressedSize > 0)
				fileSize = Info.CompressedSize;
			
			// It should always be equal but just in case
			if(reader.BaseStream.Position != entryOffset + File.DataOffset)
				reader.BaseStream.Seek(entryOffset + File.DataOffset, SeekOrigin.Begin);
			
			var rasterData = reader.ReadBytes((int)fileSize);
			
			switch(Info.BitsPerPixel)
			{
				case (ushort)BPP.Monochrome:
					if(ColorTable != null)
						Data = BmpDecoder.Decode1bit(rasterData, ColorTable);
					break;
				case (ushort)BPP.Palletized4bit:
					if(ColorTable != null)
					{
						if(CompressionType.BI_RLE4.Equals(Info.Compression))
							Data = BmpDecoder.DecodeCompressed4bit(rasterData, ColorTable, Info.Width);
						else
							Data = BmpDecoder.Decode4bit(rasterData, ColorTable);
					}
					break;
				case (ushort)BPP.Palletized8bit:
					if(ColorTable != null)
					{
						if(CompressionType.BI_RLE8.Equals(Info.Compression))
							Data = BmpDecoder.DecodeCompressed8bit(rasterData, ColorTable, Info.Width);
						else
							Data = BmpDecoder.Decode8bit(rasterData, ColorTable);
					}
					break;
				case (ushort)BPP.Rgb16bit:
					Data = BmpDecoder.Decode16bit(rasterData);
					break;
				case (ushort)BPP.Rgb24bit:
					Data = BmpDecoder.Decode24bit(rasterData);
					break;
			}
		}
	}
}
