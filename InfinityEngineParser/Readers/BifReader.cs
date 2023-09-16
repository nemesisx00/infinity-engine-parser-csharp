namespace InfinityEngineParser.Readers;

using InfinityEngineParser.Bif;

public class BifReader
{
	public static Bifc BifcFromFile(string filePath)
	{
		using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
		{
			return new(reader);
		}
	}
	
	public static BifcCompressed BifcCompressedFromFile(string filePath)
	{
		using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
		{
			return new(reader);
		}
	}
	
	public static Biff BiffFromFile(string filePath)
	{
		using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
		{
			return new(reader);
		}
	}
	
	public static Biff BiffFromBytes(byte[] bytes)
	{
		using(BinaryReader reader = new(new MemoryStream(bytes)))
		{
			return new(reader);
		}
	}
}
