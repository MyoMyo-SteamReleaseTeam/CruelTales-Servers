using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CT.Network.Serialization
{
	public class PacketSegment
	{
		private ArraySegment<byte> _buffer;
		public ArraySegment<byte> Buffer => _buffer;

		public PacketSegment(ArraySegment<byte> buffer)
		{
			_buffer = buffer;
		}
	}
}
