namespace InfinityEngineParser;

public sealed class MetadataLengths
{
	public const int DWORD = 4;
	public const int RESREF = 8;
	public const int Signature = 4;
	public const int Version = 4;
	public const int WORD = 2;
}

/// <summary>
/// See <see>https://gibberlings3.github.io/iesdp/file_formats/general.html</see>
/// </summary>
public sealed class ResourceTypes
{
	/// <summary>0x0001</summary>
	public const ushort BMP = 1;
	
	/// <summary>0x0002</summary>
	public const ushort MVE = 2;
	
	/// <summary>0x0004</summary>
	public const ushort WAV = 4;
	
	/// <summary>0x0004</summary>
	public const ushort WAVC = 4;
	
	/// <summary>0x0005</summary>
	public const ushort WFX = 5;
	
	/// <summary>0x0006</summary>
	public const ushort PLT = 6;
	
	/// <summary>0x03e8</summary>
	public const ushort BAM = 1000;
	
	/// <summary>0x03e8</summary>
	public const ushort BAMC = 1000;
	
	/// <summary>0x03e9</summary>
	public const ushort WED = 1001;
	
	/// <summary>0x03ea</summary>
	public const ushort CHU = 1002;
	
	/// <summary>0x03eb</summary>
	public const ushort TIS = 1003;
	
	/// <summary>0x03ec</summary>
	public const ushort MOS = 1004;
	
	/// <summary>0x03ec</summary>
	public const ushort MOSC = 1004;
	
	/// <summary>0x03ed</summary>
	public const ushort ITM = 1005;
	
	/// <summary>0x03ee</summary>
	public const ushort SPL = 1006;
	
	/// <summary>0x03ef</summary>
	public const ushort BCS = 1007;
	
	/// <summary>0x03f0</summary>
	public const ushort IDS = 1008;
	
	/// <summary>0x03f1</summary>
	public const ushort CRE = 1009;
	
	/// <summary>0x03f2</summary>
	public const ushort ARE = 1010;
	
	/// <summary>0x03f3</summary>
	public const ushort DLG = 1011;
	
	/// <summary>0x03f4</summary>
	public const ushort TwoDA = 1012;
	
	/// <summary>0x03f5</summary>
	public const ushort GAM = 1013;
	
	/// <summary>0x03f6</summary>
	public const ushort STO = 1014;
	
	/// <summary>0x03f7</summary>
	public const ushort WMP = 1015;
	
	/// <summary>0x03f8</summary>
	public const ushort CHR = 1016;
	
	/// <summary>0x03f8</summary>
	public const ushort EFF = 1016;
	
	/// <summary>0x03f9</summary>
	public const ushort BS = 1017;
	
	/// <summary>0x03fa</summary>
	public const ushort CHR2 = 1018;
	
	/// <summary>0x03fb</summary>
	public const ushort VVC = 1019;
	
	/// <summary>0x03fc</summary>
	public const ushort VEF = 1020;
	
	/// <summary>0x03fd</summary>
	public const ushort PRO = 1021;
	
	/// <summary>0x03fe</summary>
	public const ushort BIO = 1022;
	
	/// <summary>0x03ff</summary>
	public const ushort WBM = 1023;
	
	/// <summary>0x0400</summary>
	public const ushort FNT = 1024;
	
	/// <summary>0x0402</summary>
	public const ushort GUI = 1026;
	
	/// <summary>0x0403</summary>
	public const ushort SQL = 1027;
	
	/// <summary>0x0404</summary>
	public const ushort PVRZ = 1028;
	
	/// <summary>0x0405</summary>
	public const ushort GLSL = 1029;
	
	/// <summary>0x0408</summary>
	public const ushort MENU = 1032;
	
	/// <summary>0x0409</summary>
	public const ushort MENU2 = 1033;
	
	/// <summary>0x040a</summary>
	public const ushort TTF = 1034;
	
	/// <summary>0x040b</summary>
	public const ushort PNG = 1035;
	
	/// <summary>0x044c</summary>
	public const ushort BAH = 1100;
	
	/// <summary>0x0802</summary>
	public const ushort INI = 2050;
	
	/// <summary>0x0803</summary>
	public const ushort SRC = 2051;
}
