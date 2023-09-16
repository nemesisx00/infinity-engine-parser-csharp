namespace InfinityEngineParser;

using System.IO.Compression;
using System.Text;

public sealed class Bytes
{
	private const char NUL = '\0';
	
	public static byte[] DecompressBytes(byte[] compressedData)
	{
		int outputSize = 0;
		var decompressedBytes = new byte[compressedData.Length * 2];
		using(var memoryStream = new MemoryStream(compressedData, false))
		{
			// Discard the zlib header bytes
			memoryStream.Read(decompressedBytes, 0, 2);
			
			using(var deflateStream = new DeflateStream(memoryStream, CompressionMode.Decompress))
			{
				outputSize = deflateStream.Read(decompressedBytes, 0, compressedData.Length);
			}
		}
		
		var bytes = new byte[outputSize];
		Buffer.BlockCopy(decompressedBytes, 0, bytes, 0, outputSize);
		
		return bytes;
	}
	
	public static string? ToString(byte[] bytes, Encoding? encoding = null)
	{
		var enc = Encoding.ASCII;
		if(encoding != null)
			enc = encoding;
		
		string? text = null;
		if(bytes != null && bytes.Length > 0)
			text = enc.GetString(bytes);
		
		return text?.TrimEnd(NUL); //Trim off any NUL bytes
	}
}
