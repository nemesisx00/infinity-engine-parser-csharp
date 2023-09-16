namespace InfinityEngineParser.Bmp;

public class BmpDecoder
{
	public static List<byte>? Decode1bit(byte[]? encoded, List<ColorValues> colorTable)
	{
		List<byte>? decoded = null;
		if(encoded != null && colorTable.Count > 0)
		{
			decoded = new();
			
			foreach(var bite in encoded)
			{
				//Encoded MSB first
				for(int i = 7; i >= 0; i--)
				{
					decoded.AddRange(colorTable[Bits.ReadBit(bite, i) ? 1 : 0].AsBytes());
				}
			}
			
			var padding = decoded.Count % 32;
			for(int i = 0; i < padding; i++)
			{
				decoded.Add((byte)0);
			}
		}
		
		return decoded;
	}
	
	public static List<byte>? Decode4bit(byte[]? encoded, List<ColorValues> colorTable)
	{
		List<byte>? decoded = null;
		if(encoded != null && colorTable.Count > 0)
		{
			decoded = new();
			
			foreach(var bite in encoded)
			{
				var color = colorTable[Bits.ReadValue(bite, 0, 4)];
				decoded.AddRange(color.AsBytes());
				
				color = colorTable[Bits.ReadValue(bite, 4)];
				decoded.AddRange(color.AsBytes());
			}
			
			var padding = decoded.Count % 8;
			for(int i = 0; i < padding; i++)
			{
				decoded.Add((byte)0);
			}
		}
		
		return decoded;
	}
	
	public static List<byte>? DecodeCompressed4bit(byte[] encoded, List<ColorValues> colorTable, long width)
	{
		List<byte>? decoded = null;
		if(encoded != null && colorTable.Count > 0)
		{
			decoded = new();
			using(BinaryReader reader = new(new MemoryStream(encoded)))
			{
				var lineWidthInBytes = width * 4;
				long currentY = 0;
				long currentX = 0;
				bool delta = false;
				var uncompressedBytes = 0;
				while(reader.BaseStream.Position < reader.BaseStream.Length)
				{
					if(uncompressedBytes > 0)
					{
						var color = new ColorValues(reader);
						decoded.AddRange(color.AsBytes());
						uncompressedBytes--;
						
						if(uncompressedBytes < 1 && decoded.Count % 2 == 1)
							decoded.Add((byte)0);
					}
					//Vector offset to next pixel
					else if(delta)
					{
						var x = reader.ReadByte();
						var y = reader.ReadByte();
						
						if(currentY < y)
						{
							//Fill in remaining line
							currentX = lineWidthInBytes - (decoded.Count % lineWidthInBytes);
							for(var i = currentX; i < lineWidthInBytes; i++)
							{
								decoded.Add((byte)0);
							}
							
							//Fill in intermediary lines
							for(var i = currentY; i < y - 1; ++i)
							{
								for(int j = 0; j < lineWidthInBytes; j++)
								{
									decoded.Add((byte)0);
								}
							}
							
							//Fill in line up to designated x coordinate
							currentY = y;
							for(var i = 0; i < x * 4; i++)
							{
								decoded.Add((byte)0);
							}
						}
						
						delta = false;
					}
					else
					{
						var bytes = reader.ReadBytes(2);
						
						var count = bytes[0];
						var indices = bytes[1];
						
						if(count > 0)
						{
							var left = colorTable[Bits.ReadValue(indices, 0, 4)];
							var right = colorTable[Bits.ReadValue(indices, 4)];
							
							for(int i = 0; i < count; i++)
							{
								var color = right;
								if(i % 2 == 0)
									color = left;
								
								decoded.AddRange(color.AsBytes());
							}
						}
						else
						{
							switch(indices)
							{
								case 0:
									currentY++;
									break;
								
								case 1:
									var zeroesToAdd = decoded.Count % 8;
									for(int i = 0; i < zeroesToAdd; i++)
									{
										decoded.Add((byte)0);
									}
									break;
								
								case 2:
									delta = true;
									break;
								
								default: // index > 2
									uncompressedBytes = indices;
									break;
							}
						}
					}
				}
			}
		}
		
		return decoded;
	}
	
	public static List<byte>? Decode8bit(byte[]? encoded, List<ColorValues> colorTable)
	{
		List<byte>? decoded = null;
		if(encoded != null && colorTable.Count > 0)
		{
			decoded = new();
			foreach(var bite in encoded)
			{
				var color = colorTable[bite];
				decoded.AddRange(color.AsBytes());
			}
			
			var padding = decoded.Count % 4;
			for(int i = 0; i < padding; i++)
			{
				decoded.Add((byte)0);
			}
		}
		
		return decoded;
	}
	
	public static List<byte>? DecodeCompressed8bit(byte[]? encoded, List<ColorValues> colorTable, long width)
	{
		List<byte>? decoded = null;
		if(encoded != null && colorTable.Count > 0)
		{
			decoded = new();
			using(BinaryReader reader = new(new MemoryStream(encoded)))
			{
				long currentY = 0;
				long currentX = 0;
				var lineWidthInBytes = width * 4;
				bool delta = false;
				var uncompressedBytes = 0;
				
				while(reader.BaseStream.Position < reader.BaseStream.Length)
				{
					if(uncompressedBytes > 0)
					{
						var b = reader.ReadByte();
						var color = colorTable[b];
						decoded.AddRange(color.AsBytes());
						uncompressedBytes--;
					}
					else if(delta)
					{
						var x = reader.ReadByte();
						var y = reader.ReadByte();
						
						if(currentY < y)
						{
							//Fill in remaining line
							currentX = lineWidthInBytes - (decoded.Count % lineWidthInBytes);
							for(var i = currentX; i < lineWidthInBytes; i++)
							{
								decoded.Add((byte)0);
							}
							
							//Fill in intermediary lines
							for(var i = currentY; i < y - 1; ++i)
							{
								for(int j = 0; j < lineWidthInBytes; j++)
								{
									decoded.Add((byte)0);
								}
							}
							
							//Fill in line up to designated x coordinate
							currentY = y;
							for(var i = 0; i < x * 4; i++)
							{
								decoded.Add((byte)0);
							}
						}
						
						delta = false;
					}
					else
					{
						var count = reader.ReadByte();
						var index = reader.ReadByte();
						
						if(count > 0)
						{
							var color = colorTable[index];
							for(int i = 0; i < count; i++)
							{
								decoded.AddRange(color.AsBytes());
							}
						}
						else
						{
							switch(index)
							{
								case 0:
									var padding0 = decoded.Count % lineWidthInBytes;
									for(int i = 0; i < padding0; i++)
									{
										decoded.Add((byte)0);
									}
									
									currentY++;
									break;
								
								case 1:
									var padding1 = decoded.Count % 4;
									for(int i = 0; i < padding1; i++)
									{
										decoded.Add((byte)0);
									}
									break;
								
								case 2:
									delta = true;
									break;
								
								default: // index > 2
									uncompressedBytes = index;
									break;
							}
						}
					}
				}
			}
		}
		
		return decoded;
	}
	
	public static List<byte>? Decode16bit(byte[]? encoded)
	{
		List<byte>? decoded = null;
		if(encoded != null)
		{
			decoded = new();
			foreach(var bite in encoded)
			{
				decoded.Add(Bits.ReadValue(bite, 0, 4));
				decoded.Add(Bits.ReadValue(bite, 4));
			}
		}
		return decoded;
	}
	
	public static List<byte>? Decode24bit(byte[]? encoded)
	{
		/*
		We could jump through the hoop of building ColorValues instances and
		then .AsBytes()'ing them out but, because every 4 bytes defines a single
		pixel and a ColorValues instance writes out as 4 bytes, the input and
		output bytes have a 1-to-1 relationship.
		
		So just convert from an array to a List and be done with it.
		*/
		List<byte>? decoded = null;
		if(encoded != null)
			decoded = new(encoded);
		return decoded;
	}
}
