using System;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CT.Tool.Data
{
	public static class JsonHandler
	{
		public static JsonSerializerSettings LoadOption { get; } = new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.Auto,
			NullValueHandling = NullValueHandling.Ignore,
		};

		public static JsonSerializerSettings SaveOption { get; } = new JsonSerializerSettings()
		{
			TypeNameHandling = TypeNameHandling.Auto,
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.Indented
		};

		public static JobResult TryToJsonText<T>(T instance)
		{
			try
			{
				var data = JsonConvert.SerializeObject(instance, SaveOption);
				return new JobResult(result: JobResultType.Success, data);
			}
			catch (Exception e)
			{
				return new JobResult(result: JobResultType.Failed, exception: e);
			}
		}

		public static JobResult<T> TryJsonToInstance<T>(string json) where T : new()
		{
			try
			{
				T? instance = JsonConvert.DeserializeObject<T>(json, LoadOption);
				if (instance == null)
				{
					return new JobResult<T>(result: JobResultType.Failed,
											exception: new Exception("Json deserialize failed!"));
				}

				return new JobResult<T>(result: JobResultType.Success, instance);
			}
			catch (Exception e)
			{
				return new JobResult<T>(result: JobResultType.Failed, exception: e);
			}
		}

		public static JobResult TryWriteObject<T>(string filePath, T instance, bool makeDirectory = false)
		{
			var result = TryToJsonText(instance);
			if (result.ResultType == JobResultType.Success)
			{
				return FileHandler.TryWriteText(filePath, result.Value, makeDirectory);
			}
			return result;
		}

		public static async ValueTask<JobResult> TryWriteObjectAsync<T>(string filePath, T instance,
																   bool makeDirectory = false,
																   CancellationToken cancellationToken = default)
		{
			var result = TryToJsonText(instance);
			if (cancellationToken.IsCancellationRequested)
			{
				return new JobResult(result: JobResultType.Canceled);
			}
			if (result.ResultType == JobResultType.Success)
			{
				return await FileHandler.TryWriteTextAsync(filePath, result.Value, makeDirectory, cancellationToken);
			}
			return result;
		}

		public static JobResult<T> TryReadObject<T>(string filePath) where T : new()
		{
			var result = FileHandler.TryReadText(filePath);
			if (result.ResultType == JobResultType.Success)
			{
				return TryJsonToInstance<T>(result.Value);
			}
			return new JobResult<T>(result: JobResultType.Failed, exception: result.Exception);
		}

		public static async ValueTask<JobResult<T>> TryReadObjectAsync<T>(string filePath,
																		  CancellationToken cancellationToken = default)
																		  where T : new()
		{
			var result = await FileHandler.TryReadTextAsync(filePath);
			if (cancellationToken.IsCancellationRequested)
			{
				return new JobResult<T>(result: JobResultType.Canceled);
			}
			if (result.ResultType == JobResultType.Success)
			{
				return TryJsonToInstance<T>(result.Value);
			}
			return new JobResult<T>(result: JobResultType.Failed, exception: result.Exception);
		}
	}
}
