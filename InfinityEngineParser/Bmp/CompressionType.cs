namespace InfinityEngineParser.Bmp;

/// <summary>
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bmp.htm</see>
/// </summary>
public enum CompressionType : uint
{
	BI_RGB = 0,
	BI_RLE8 = 1,
	BI_RLE4 = 2,
}
