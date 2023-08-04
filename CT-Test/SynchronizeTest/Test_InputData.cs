using CT.Common.DataType.Input;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CTC.Networks.SyncObjects.TestSyncObjects
{
	[TestClass]
	public class Test_InputData
	{
		[TestMethod]
		public void MovementDataTest()
		{
			MovementInputData data = new MovementInputData();

			data.MoveDirection = InputDirection.Up;
			Assert.AreEqual(data.MoveDirection, InputDirection.Up);

			data.MoveInputType = MovementType.Walk;
			Assert.AreEqual(data.MoveInputType, MovementType.Walk);
		}
	}
}
