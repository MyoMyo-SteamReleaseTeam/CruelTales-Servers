using System;

namespace CT.Common.Tools.Data
{
	public enum JobResultType
	{
		Success = 0,
		Failed = 1,
		Canceled = 2,
	}

	/// <summary>작업 결과입니다.</summary>
	public readonly struct JobResult
	{
		/// <summary>성공 여부입니다.</summary>
		public readonly JobResultType ResultType;
		/// <summary>읽거나 쓴 값 입니다.</summary>
		public readonly string Value;
		/// <summary>예외입니다.</summary>
		public readonly Exception Exception;

		public JobResult(JobResultType result,
						 string value = "",
						 Exception? exception = null)
		{
			ResultType = result;
			Value = value;
			Exception = exception ?? new Exception();
		}
	}

	/// <summary>작업 결과입니다.</summary>
	public readonly struct JobResult<T> where T : new()
	{
		/// <summary>성공 여부입니다.</summary>
		public readonly JobResultType ResultType;
		/// <summary>읽거나 쓴 값 입니다.</summary>
		public readonly T Value;
		/// <summary>예외입니다.</summary>
		public readonly Exception Exception;

		public JobResult(JobResultType result,
						 T? value = default,
						 Exception? exception = null)
		{
			ResultType = result;
			Value = value ?? new T();
			Exception = exception ?? new Exception();
		}
	}
}
