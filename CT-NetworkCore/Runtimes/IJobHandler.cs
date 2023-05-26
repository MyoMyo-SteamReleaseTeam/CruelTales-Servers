namespace CT.Networks.Runtimes
{
	public interface IJobHandler
	{
		public void Clear();
		public void Flush();
	}

	public interface IJobHandler<FArg> where FArg : struct
	{
		public void Clear();
		public void Flush(FArg arg);
	}
}
