namespace InfinityEngineParser.Bmp;

/// <summary>
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/bmp.htm</see>
/// </summary>
public enum BPP : ushort
{
	Monochrome = 1,
	Palletized4bit = 4,
	Palletized8bit = 8,
	Rgb16bit = 16,
	Rgb24bit = 24,
}
