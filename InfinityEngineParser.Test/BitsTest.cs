namespace InfinityEngineParser.Test;

using InfinityEngineParser;

public class BitsTest
{
	public static IEnumerable<object[]> BitTestData()
	{
		List<object[]> list = new();
		for(int i = -1; i < 65; i++)
		{
			list.Add(new object[] { i });
		}
		
		return list;
	}
	
	public static IEnumerable<object[]> ReadBitRangeData()
		=> new object[][]
		{
			new object[] { 8, 4, 0, 8 },
			new object[] { 16, 2, 3, 2 },
		};
	
	[Theory]
	[MemberData(nameof(BitTestData))]
	public void BitTest(int bit)
	{
		long next = 1 << bit;
		long max = next - 1;
		
		if(bit < 0)
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((byte)next, bit));
		else if(bit < 8)
		{
			Assert.True(Bits.ReadBit((byte)next, (byte)bit));
			Assert.True(Bits.ReadBit((byte)next, (short)bit));
			Assert.True(Bits.ReadBit((byte)next, (ushort)bit));
			Assert.True(Bits.ReadBit((byte)next, bit));
			Assert.True(Bits.ReadBit((byte)next, (uint)bit));
			Assert.True(Bits.ReadBit((byte)next, (long)bit));
			
			Assert.False(Bits.ReadBit((byte)max, (byte)bit));
			Assert.False(Bits.ReadBit((byte)max, (short)bit));
			Assert.False(Bits.ReadBit((byte)max, (ushort)bit));
			Assert.False(Bits.ReadBit((byte)max, bit));
			Assert.False(Bits.ReadBit((byte)max, (uint)bit));
			Assert.False(Bits.ReadBit((byte)max, (long)bit));
		}
		else if(bit == 8)
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((byte)next, bit));
		
		if(bit < 0)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((short)next, bit));
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((ushort)next, bit));
		}
		else if(bit < 16)
		{
			Assert.True(Bits.ReadBit((short)next, (byte)bit));
			Assert.True(Bits.ReadBit((short)next, (short)bit));
			Assert.True(Bits.ReadBit((short)next, (ushort)bit));
			Assert.True(Bits.ReadBit((short)next, bit));
			Assert.True(Bits.ReadBit((short)next, (uint)bit));
			Assert.True(Bits.ReadBit((short)next, (long)bit));
			
			Assert.False(Bits.ReadBit((short)max, (byte)bit));
			Assert.False(Bits.ReadBit((short)max, (short)bit));
			Assert.False(Bits.ReadBit((short)max, (ushort)bit));
			Assert.False(Bits.ReadBit((short)max, bit));
			Assert.False(Bits.ReadBit((short)max, (uint)bit));
			Assert.False(Bits.ReadBit((short)max, (long)bit));
			
			// On 15, next's value is beyond a ushort's memory capacity and so
			// technically this becomes a test of Bits.ReadBit(uint, int) but
			// whatever.
			Assert.True(Bits.ReadBit((ushort)next, (byte)bit));
			Assert.True(Bits.ReadBit((ushort)next, (short)bit));
			Assert.True(Bits.ReadBit((ushort)next, (ushort)bit));
			Assert.True(Bits.ReadBit((ushort)next, bit));
			Assert.True(Bits.ReadBit((ushort)next, (uint)bit));
			Assert.True(Bits.ReadBit((ushort)next, (long)bit));
			
			Assert.False(Bits.ReadBit((ushort)max, (byte)bit));
			Assert.False(Bits.ReadBit((ushort)max, (short)bit));
			Assert.False(Bits.ReadBit((ushort)max, (ushort)bit));
			Assert.False(Bits.ReadBit((ushort)max, bit));
			Assert.False(Bits.ReadBit((ushort)max, (uint)bit));
			Assert.False(Bits.ReadBit((ushort)max, (long)bit));
		}
		else if(bit == 16)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((short)next, bit));
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((ushort)next, bit));
		}
		
		if(bit < 0)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((int)next, bit));
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((uint)next, bit));
		}
		else if(bit < 32)
		{
			Assert.True(Bits.ReadBit((int)next, (byte)bit));
			Assert.True(Bits.ReadBit((int)next, (short)bit));
			Assert.True(Bits.ReadBit((int)next, (ushort)bit));
			Assert.True(Bits.ReadBit((int)next, bit));
			Assert.True(Bits.ReadBit((int)next, (uint)bit));
			Assert.True(Bits.ReadBit((int)next, (long)bit));
			
			Assert.False(Bits.ReadBit((int)max, (byte)bit));
			Assert.False(Bits.ReadBit((int)max, (short)bit));
			Assert.False(Bits.ReadBit((int)max, (ushort)bit));
			Assert.False(Bits.ReadBit((int)max, bit));
			Assert.False(Bits.ReadBit((int)max, (uint)bit));
			Assert.False(Bits.ReadBit((int)max, (long)bit));
			
			
			// On 31, next's value is beyond a uint's memory capacity and there
			// is no Bits.ReadBit(ulong, int) overload
			if(bit < 31)
			{
				Assert.True(Bits.ReadBit((uint)next, (byte)bit));
				Assert.True(Bits.ReadBit((uint)next, (short)bit));
				Assert.True(Bits.ReadBit((uint)next, (ushort)bit));
				Assert.True(Bits.ReadBit((uint)next, bit));
				Assert.True(Bits.ReadBit((uint)next, (uint)bit));
				Assert.True(Bits.ReadBit((uint)next, (long)bit));
			}
			
			Assert.False(Bits.ReadBit((uint)max, (byte)bit));
			Assert.False(Bits.ReadBit((uint)max, (short)bit));
			Assert.False(Bits.ReadBit((uint)max, (ushort)bit));
			Assert.False(Bits.ReadBit((uint)max, bit));
			Assert.False(Bits.ReadBit((uint)max, (uint)bit));
			Assert.False(Bits.ReadBit((uint)max, (long)bit));
		}
		else if(bit == 32)
		{
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((int)next, bit));
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit((uint)next, bit));
		}
		
		if(bit < 0)
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit(next, bit));
		else if(bit < 64)
		{
			Assert.True(Bits.ReadBit(next, (byte)bit));
			Assert.True(Bits.ReadBit(next, (short)bit));
			Assert.True(Bits.ReadBit(next, (ushort)bit));
			Assert.True(Bits.ReadBit(next, bit));
			Assert.True(Bits.ReadBit(next, (uint)bit));
			Assert.True(Bits.ReadBit(next, (long)bit));
			
			Assert.False(Bits.ReadBit(max, (byte)bit));
			Assert.False(Bits.ReadBit(max, (short)bit));
			Assert.False(Bits.ReadBit(max, (ushort)bit));
			Assert.False(Bits.ReadBit(max, bit));
			Assert.False(Bits.ReadBit(max, (uint)bit));
			Assert.False(Bits.ReadBit(max, (long)bit));
		}
		else
			Assert.Throws<ArgumentOutOfRangeException>(() => Bits.ReadBit(next, bit));
	}
	
	[Theory]
	[MemberData(nameof(ReadBitRangeData))]
	public void ReadBitRangeTest(long value, long bit, long shift, long expected)
	{
		Assert.Equal(expected, Bits.ReadValue(value, bit, shift));
		Assert.Equal(expected, Bits.ReadValue(value, (uint)bit, shift));
		Assert.Equal(expected, Bits.ReadValue(value, (uint)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (uint)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (uint)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (uint)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (uint)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (uint)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (int)bit, shift));
		Assert.Equal(expected, Bits.ReadValue(value, (int)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (int)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (int)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (int)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (int)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (int)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (ushort)bit, shift));
		Assert.Equal(expected, Bits.ReadValue(value, (ushort)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (ushort)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (ushort)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (ushort)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (ushort)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (ushort)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (short)bit, shift));
		Assert.Equal(expected, Bits.ReadValue(value, (short)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (short)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (short)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (short)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (short)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (short)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (byte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue(value, (byte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (byte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (byte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (byte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (byte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (byte)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (sbyte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue(value, (sbyte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (sbyte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (sbyte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (sbyte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (sbyte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue(value, (sbyte)bit, (sbyte)shift));
		
		Assert.Equal(expected, Bits.ReadValue((uint)value, bit, shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (uint)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (uint)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (uint)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (uint)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (uint)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (uint)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (uint)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (int)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (int)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (int)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (int)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (int)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (int)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (int)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (ushort)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (ushort)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (ushort)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (ushort)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (ushort)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (ushort)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (ushort)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (short)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (short)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (short)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (short)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (short)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (short)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (short)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (byte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (byte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (byte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (byte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (byte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (byte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (byte)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (sbyte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (sbyte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (sbyte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (sbyte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (sbyte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (sbyte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((uint)value, (sbyte)bit, (sbyte)shift));
		
		Assert.Equal(expected, Bits.ReadValue((int)value, bit, shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (uint)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (uint)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (uint)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (uint)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (uint)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (uint)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (uint)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (int)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (int)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (int)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (int)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (int)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (int)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (int)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (ushort)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (ushort)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (ushort)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (ushort)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (ushort)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (ushort)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (ushort)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (short)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (short)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (short)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (short)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (short)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (short)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (short)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (byte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (byte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (byte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (byte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (byte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (byte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (byte)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (sbyte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (sbyte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (sbyte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (sbyte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (sbyte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (sbyte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((int)value, (sbyte)bit, (sbyte)shift));
		
		Assert.Equal(expected, Bits.ReadValue((ushort)value, bit, shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (uint)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (uint)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (uint)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (uint)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (uint)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (uint)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (uint)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (int)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (int)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (int)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (int)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (int)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (int)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (int)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (ushort)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (ushort)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (ushort)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (ushort)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (ushort)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (ushort)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (ushort)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (short)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (short)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (short)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (short)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (short)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (short)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (short)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (byte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (byte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (byte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (byte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (byte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (byte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (byte)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (sbyte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (sbyte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (sbyte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (sbyte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (sbyte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (sbyte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((ushort)value, (sbyte)bit, (sbyte)shift));
		
		Assert.Equal(expected, Bits.ReadValue((short)value, bit, shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (uint)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (uint)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (uint)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (uint)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (uint)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (uint)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (uint)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (int)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (int)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (int)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (int)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (int)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (int)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (int)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (ushort)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (ushort)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (ushort)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (ushort)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (ushort)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (ushort)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (ushort)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (short)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (short)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (short)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (short)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (short)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (short)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (short)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (byte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (byte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (byte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (byte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (byte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (byte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (byte)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (sbyte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (sbyte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (sbyte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (sbyte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (sbyte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (sbyte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((short)value, (sbyte)bit, (sbyte)shift));
		
		Assert.Equal(expected, Bits.ReadValue((byte)value, bit, shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (uint)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (uint)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (uint)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (uint)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (uint)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (uint)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (uint)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (int)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (int)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (int)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (int)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (int)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (int)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (int)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (ushort)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (ushort)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (ushort)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (ushort)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (ushort)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (ushort)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (ushort)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (short)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (short)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (short)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (short)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (short)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (short)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (short)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (byte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (byte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (byte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (byte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (byte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (byte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (byte)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (sbyte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (sbyte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (sbyte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (sbyte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (sbyte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (sbyte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((byte)value, (sbyte)bit, (sbyte)shift));
		
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, bit, shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (uint)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (uint)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (uint)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (uint)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (uint)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (uint)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (uint)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (int)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (int)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (int)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (int)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (int)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (int)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (int)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (ushort)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (ushort)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (ushort)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (ushort)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (ushort)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (ushort)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (ushort)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (short)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (short)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (short)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (short)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (short)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (short)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (short)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (byte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (byte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (byte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (byte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (byte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (byte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (byte)bit, (sbyte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (sbyte)bit, shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (sbyte)bit, (uint)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (sbyte)bit, (int)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (sbyte)bit, (ushort)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (sbyte)bit, (short)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (sbyte)bit, (byte)shift));
		Assert.Equal(expected, Bits.ReadValue((sbyte)value, (sbyte)bit, (sbyte)shift));
	}
	
	[Fact]
	public void SpecificBitTest()
	{
		var value = 0b01101001;
		Assert.False(Bits.ReadBit(value, 7));
		Assert.True(Bits.ReadBit(value, 6));
		Assert.True(Bits.ReadBit(value, 5));
		Assert.False(Bits.ReadBit(value, 4));
		Assert.True(Bits.ReadBit(value, 3));
		Assert.False(Bits.ReadBit(value, 2));
		Assert.False(Bits.ReadBit(value, 1));
		Assert.True(Bits.ReadBit(value, 0));
		
		Assert.Equal(value, Bits.ReadValue(value, 7));
		
		var expected = 0b00101001;
		Assert.Equal(expected, Bits.ReadValue(value, 6));
		
		expected = 0b00001001;
		Assert.Equal(expected, Bits.ReadValue(value, 5));
		Assert.Equal(expected, Bits.ReadValue(value, 4));
		
		expected = 0b00000001;
		Assert.Equal(expected, Bits.ReadValue(value, 3));
		Assert.Equal(expected, Bits.ReadValue(value, 2));
		Assert.Equal(expected, Bits.ReadValue(value, 1));
		
		expected = 0b00000000;
		Assert.Equal(expected, Bits.ReadValue(value, 0));
	}
}
