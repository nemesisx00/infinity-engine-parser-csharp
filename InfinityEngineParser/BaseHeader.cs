namespace InfinityEngineParser;

public class BaseHeader : IEquatable<BaseHeader>, FillFromReader
{
	public string Signature { get; set; } = String.Empty;
	public string Version { get; set; } = String.Empty;
	
	public BaseHeader() {}
	public BaseHeader(BinaryReader reader) => Fill(reader);

	public bool Equals(BaseHeader? other)
	{
		return Signature.Equals(other?.Signature)
			&& Version.Equals(other?.Version);
	}
	
	public void Fill(BinaryReader reader)
	{
		var sigBytes = reader.ReadBytes(MetadataLengths.Signature);
		Signature = Bytes.ToString(sigBytes) ?? String.Empty;
		
		var verBytes = reader.ReadBytes(MetadataLengths.Version);
		Version = Bytes.ToString(verBytes) ?? String.Empty;
	}
}
