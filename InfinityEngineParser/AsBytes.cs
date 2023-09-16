namespace InfinityEngineParser;

/// <summary>
/// An object capable of converting its data into an array of bytes.
/// </summary>
public interface AsBytes
{
	/// <summary>
	/// Convert this object's properties into a contiguous array of bytes.
	/// </summary>
	/// <returns>
	/// A byte array comprised of the object's data.
	/// </returns>
	public byte[] AsBytes();
}
