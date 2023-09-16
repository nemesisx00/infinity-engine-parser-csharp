namespace InfinityEngineParser.Readers;

using InfinityEngineParser.Key;

public class KeyReader
{
	public static InfinityEngineKey FromFile(string filePath)
	{
		using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
		{
			return new(reader);
		}
	}
}
