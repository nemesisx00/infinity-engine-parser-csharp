namespace InfinityEngineParser;

public static class Extensions
{
	/// <summary>
	/// Fill this object's properties with data read from <paramref name="reader" />.
	/// </summary>
	/// <param name="reader">The data source being read.</param>
	/// <param name="offset">
	/// The position to which the <paramref name="reader" /> will seek before
	/// any data is read.
	/// </param>
	/// <remarks>
	/// Alters the <paramref name="reader" />'s position before reading.
	/// </remarks>
	public static void Fill<T>(this T instance, BinaryReader reader, uint offset)
		where T: FillFromReader
	{
		reader.BaseStream.Seek(offset, SeekOrigin.Begin);
		instance.Fill(reader);
	}
	
	/// <summary>
	/// Fill this object's properties with data read from <paramref name="reader" />.
	/// </summary>
	/// <param name="reader">The data source being read.</param>
	/// <param name="offset">
	/// The position to which the <paramref name="reader" /> will seek before
	/// any data is read.
	/// </param>
	/// <remarks>
	/// Alters the <paramref name="reader" />'s position before reading.
	/// </remarks>
	public static void Fill<T>(this T instance, BinaryReader reader, int offset)
			where T: FillFromReader
		=> Fill<T>(instance, reader, (uint)offset);
}
