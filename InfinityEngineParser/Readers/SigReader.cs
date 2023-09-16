namespace InfinityEngineParser.Readers;

public class SigReader
{
	public static BaseHeader FromFile(string filePath)
	{
		using(BinaryReader reader = new(File.Open(filePath, FileMode.Open)))
		{
			return new(reader);
		}
	}
}
