using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CT.Network.Serialization.Type;

namespace CT.Network.Serialization
{
	public class PacketWriter
	{
		private ArraySegment<byte> _buffer;

		public int Length;
		public int Position;

		public void Put(byte[] value)
		{

		}

		public void Put(byte value)
		{
		
		}

		public void Put(sbyte value)
		{

		}

		public void Put(short value)
		{

		}

		public void Put(ushort value)
		{

		}

		public void Put(int value)
		{

		}

		public void Put(uint value)
		{

		}

		public void Put(long value)
		{

		}

		public void Put(ulong value)
		{

		}

		public void Put(string value)
		{

		}

		public void Put(NetStringShort value)
		{

		}

		public void Put(float value)
		{

		}

		public void Put(double value)
		{

		}
	}
}
