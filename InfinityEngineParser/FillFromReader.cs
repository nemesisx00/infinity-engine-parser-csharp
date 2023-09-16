namespace InfinityEngineParser;

/// <summary>
/// An object capable of reading directly from a <c>BinaryReader</c> instance,
/// ensuring the correct amount of data is read in the correct order.
/// </summary>
public interface FillFromReader
{
	/// <summary>
	/// Fill this object's properties with data read from <paramref name="reader" />
	/// </summary>
	/// <param name="reader">The data source being read.</param>
	/// <remarks>
	/// Does not alter the <paramref name="reader" />'s position before reading.
	/// </remarks>
	public void Fill(BinaryReader reader);
}
