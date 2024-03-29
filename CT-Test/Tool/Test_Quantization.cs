﻿using System;
using System.Numerics;
using CT.Common.Quantization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CT.Test.Tool
{
	[TestClass]
	public class Test_Quantization
	{
		[TestMethod]
		public void Quantization_Angle()
		{
			check(new Vector2(1, 0));
			check(new Vector2(1, 1));
			check(new Vector2(0, 1));
			check(new Vector2(-1, 1));
			check(new Vector2(-1, 0));
			check(new Vector2(-1, -1));
			check(new Vector2(0, -1));
			check(new Vector2(1, -1));

			//for (float x = -1; x <= 1; x += 0.1f)
			//{
			//	for (float y = -1; y <= 1; y += 0.1f)
			//	{
			//		check(new Vector2(x, y));
			//	}
			//}

			void check(Vector2 value)
			{
				var vec = Vector2.Normalize(value);
				var a = Quantizer.Vector2ToByte(vec);
				var qVec = Quantizer.RadByteToVec2(a);
				Console.WriteLine($"Expected:{vec} / Actual:{qVec}");

				int checkPrecision = 0;

				Assert.AreEqual(MathF.Round(vec.X, checkPrecision),
								MathF.Round(qVec.X, checkPrecision));
				Assert.AreEqual(MathF.Round(vec.Y, checkPrecision),
								MathF.Round(qVec.Y, checkPrecision));
			}
		}

		//[TestMethod]
		//public void Quantization_Print()
		//{
		//	printQuantizeAngle(new Vector2(1, 0));
		//	printQuantizeAngle(new Vector2(1, 1));
		//	printQuantizeAngle(new Vector2(0, 1));
		//	printQuantizeAngle(new Vector2(-1, 1));
		//	printQuantizeAngle(new Vector2(-1, 0));
		//	printQuantizeAngle(new Vector2(-1, -1));
		//	printQuantizeAngle(new Vector2(0, -1));
		//	printQuantizeAngle(new Vector2(1, -1));
		//	printQuantizeAngle(new Vector2());
		//}

		private void printQuantizeAngle(Vector2 direction, string name = "vec")
		{
			Vector2 vec2 = Vector2.Normalize(direction);
			byte angle = Quantizer.Vector2ToByte(vec2);
			Console.WriteLine($"{name} : " + vec2);
			Console.WriteLine($"{name} : " + angle);
		}

		[TestMethod]
		public void BoundaryTest()
		{
			var maxbytes = new byte[2];
			var minbytes = new byte[2];

			short maxPos = short.MaxValue;
			short minPos = short.MinValue;

			maxPos = 1;
			minPos = -1;

			maxbytes = BitConverter.GetBytes(maxPos);
			minbytes = BitConverter.GetBytes(minPos);

			maxPos /= 2;
			minPos /= 2;
			maxbytes = BitConverter.GetBytes(maxPos);
			minbytes = BitConverter.GetBytes(minPos);

			maxPos /= 2;
			minPos /= 2;
			maxbytes = BitConverter.GetBytes(maxPos);
			minbytes = BitConverter.GetBytes(minPos);

			maxPos /= 2;
			minPos /= 2;
			maxbytes = BitConverter.GetBytes(maxPos);
			minbytes = BitConverter.GetBytes(minPos);

			short test = -32768;
			test = (short)(test >> 1);
			var testBytes = BitConverter.GetBytes(test);
		}
	}
}
