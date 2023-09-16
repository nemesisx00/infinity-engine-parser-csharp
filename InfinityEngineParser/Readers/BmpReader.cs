namespace InfinityEngineParser.Readers;

using InfinityEngineParser.Bif;
using InfinityEngineParser.Bmp;

public class BmpReader
{
	public static Bmp? FromFile(string filePath, FileEntry entry)
	{
		Bmp? instance = null;
		
		using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
		{
			var signature = ReadUtility.ReadStringAt(reader, entry.Offset, Bmp.Type.Length);
			
			if(Bmp.Type.Equals(signature))
			{
				instance = new();
				instance.Fill(reader, entry.Offset);
			}
		}
		
		return instance;
	}
	
	public static Bmp? FromBytes(byte[] bytes, FileEntry entry)
	{
		Bmp? instance = null;
		
		using(BinaryReader reader = new(new MemoryStream(bytes)))
		{
			var signature = ReadUtility.ReadStringAt(reader, entry.Offset, Bmp.Type.Length);
			
			if(Bmp.Type.Equals(signature))
			{
				instance = new();
				instance.Fill(reader, entry.Offset);
			}
		}
		
		return instance;
	}
}
