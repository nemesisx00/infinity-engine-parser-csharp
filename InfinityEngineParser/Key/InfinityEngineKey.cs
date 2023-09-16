namespace InfinityEngineParser.Key;

/// <summary>
/// <para>The fully parsed metadata contents of a KEY V1 file.</para>
/// 
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/ie_formats/key_v1.htm</see>
/// 
/// <para>
/// This file format acts as a central reference point to locate files required
/// by the game (in a BIFF file on a CD or in the override directory). The key
/// file also maintains a mapping from an 8 byte resource name (resref) to a 32
/// byte ID (using the lowest 12 bits to identify a resource). There is generally
/// only one key file with each game (chitin.key).
/// </para>
/// </summary>
public class InfinityEngineKey : FillFromReader
{
	public const string FileName = "chitin.key";
	
	public const string Signature = "KEY ";
	public const string Version = "V1  ";
	
	public InfinityEngineKey() {}
	public InfinityEngineKey(BinaryReader reader) => Fill(reader);
	
	public Header? Header { get; set; }
	public List<BifEntry> BifEntries { get; set; } = new();
	public List<ResourceEntry> ResourceEntries { get; set; } = new();
	
	public void Fill(BinaryReader reader)
	{
		Header = new(reader);
		
		for(int i = 0; i < Header.BifCount; i++)
		{
			BifEntries.Add(new(reader));
		}
		BifEntries.ForEach(be => be.FillName(reader));
		
		
		reader.BaseStream.Seek(Header.ResourceOffset, SeekOrigin.Begin);
		for(int i = 0; i < Header.ResourceCount; i++)
		{
			ResourceEntries.Add(new(reader));
		}
	}
}
