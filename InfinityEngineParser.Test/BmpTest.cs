namespace InfinityEngineParser.Test;

using InfinityEngineParser;
using InfinityEngineParser.Bmp;
using InfinityEngineParser.Key;
using InfinityEngineParser.Readers;

[Collection("ReadsKeyFile")]
public class BmpTest
{
	private const ushort imageType = ResourceTypes.BMP;
	private const string testKeyName = "name";
	private const string testKeyBitCount = "bitCount";
	private const string testKeyHeight = "height";
	private const string testKeyWidth = "width";
	
	public static IEnumerable<object[]> ImageData()
		=> new object[][]
		{
			new object[] { "AR0002SR", 56, 54, 4 },
			new object[] { "AJANTISS", 38, 60, 8 },
			new object[] { "AJANTISG", 210, 330, 24 }
		};
	
	[Theory]
	[MemberData(nameof(ImageData))]
	public void FindImageResource(string imageName, int expectedWidth, int expectedHeight, int expectedBitsPerPixel)
	{
		var installPath = GamePaths.FindInstallationPath(Games.BaldursGate1);
		//If the game is installed
		if(!String.IsNullOrEmpty(installPath))
		{
			#nullable disable
			var keyPath = Path.Combine(installPath, InfinityEngineKey.FileName);
			var key = KeyReader.FromFile(keyPath);
			var resourceEntry = key.ResourceEntries.Find(re => imageName.Equals(re.Name));
			var bifEntry = key.BifEntries[(int)resourceEntry.IndexBifEntry];
			var bifPath = Path.Combine(installPath, bifEntry.FileName);
			var biff = BifReader.BiffFromFile(bifPath);
			#nullable enable
			
			Assert.NotNull(resourceEntry);
			Assert.NotNull(biff);
			Assert.NotEmpty(bifPath);
			
			var fileEntries = biff.FileEntries.FindAll(fe => resourceEntry.IndexFile == fe.Index && fe.Type == imageType);
			Assert.Single(fileEntries);
			
			var bmp = BmpReader.FromFile(bifPath, fileEntries[0]);
			Assert.NotNull(bmp);
			Assert.NotNull(bmp.File);
			Assert.Equal(Bmp.Type, bmp.File.Type);
			Assert.Equal(FileHeader.ExpectedSize, bmp.File.AsBytes().Length);
			Assert.NotNull(bmp.Info);
			Assert.Equal((int)bmp.Info.Size, bmp.Info.AsBytes().Length);
			Assert.NotNull(bmp.Data);
			Assert.NotEmpty(bmp.Data);
			
			if(bmp.Info.Compression == (uint)CompressionType.BI_RGB)
			{
				Assert.NotNull(bmp.File);
				Assert.Equal(bmp.DecodedSize, bmp.Data.Count);
			}
			
			Assert.Equal(expectedBitsPerPixel, bmp.Info.BitsPerPixel);
			Assert.Equal(expectedHeight, bmp.Info.Height);
			Assert.Equal(expectedWidth, bmp.Info.Width);
			
			switch(bmp.Info.BitsPerPixel)
			{
				case (ushort)BPP.Monochrome:
					Assert.NotNull(bmp.ColorTable);
					Assert.Equal((int)ColorTableSizes.Monochrome, bmp.ColorTable.Count);
					break;
				
				case (ushort)BPP.Palletized4bit:
					Assert.NotNull(bmp.ColorTable);
					Assert.Equal((int)ColorTableSizes.Palletized4bit, bmp.ColorTable.Count);
					break;
				
				case (ushort)BPP.Palletized8bit:
					Assert.NotNull(bmp.ColorTable);
					Assert.Equal((int)ColorTableSizes.Palletized8bit, bmp.ColorTable.Count);
					break;
				
				case (ushort)BPP.Rgb16bit:
				case (ushort)BPP.Rgb24bit:
				default:
					Assert.Null(bmp.ColorTable);
					break;
			}
			
			//Verify with eyes
			//File.WriteAllBytes($"./{imageName}.bmp", bmp.AsBytes());
		}
	}
}
