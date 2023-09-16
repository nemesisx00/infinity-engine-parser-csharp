namespace InfinityEngineParser;

using System.Text;

public sealed class ReadUtility
{
	private const char NUL = '\0';
	
	public static string? ReadStringAt(BinaryReader reader, uint offset, int length, Encoding? encoding = null)
		=> ReadStringAt(reader, (int)offset, length, encoding);
	
	public static string? ReadStringAt(BinaryReader reader, int offset, int length, Encoding? encoding = null)
	{
		var enc = Encoding.ASCII;
		if(encoding != null)
			enc = encoding;
		
		string? text = null;
		if(reader.BaseStream.Length >= offset + length)
		{
			reader.BaseStream.Seek(offset, SeekOrigin.Begin);
			var bytes = reader.ReadBytes(length);
			if(bytes != null && bytes.Length > 0)
				text = enc.GetString(bytes);
		}
		
		return text?.TrimEnd(NUL); //Trim off any NUL bytes
	}
}
