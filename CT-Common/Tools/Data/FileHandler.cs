using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace CT.Common.Tools.Data
{
	/// <summary>예외를 발생시키지 않는 파일 입출력 도우미 클래스입니다.</summary>
	public static class FileHandler
	{
		/// <summary>텍스트 파일을 저장합니다.</summary>
		/// <param name="folderPath">저장할 파일의 폴더 경로입니다.</param>
		/// <param name="fileNameWithExtension">저장할 파일의 이름입니다. 확장자를 포함합니다.</param>
		/// <param name="data">저장할 데이터입니다.</param>
		/// <param name="makeDirectory">
		/// true일 경우 경로가 없다면 생성합니다.
		/// false일 경우 경로가 없다면 예외가 발생합니다.
		/// </param>
		/// <returns>성공 여부입니다.</returns>
		public static JobResult TryWriteText(string folderPath,
											 string fileNameWithExtension,
											 string data,
											 bool makeDirectory = false)
		{
			string filePath = Path.Combine(folderPath, fileNameWithExtension);
			return TryWriteText(filePath, data, makeDirectory);
		}

		/// <summary>텍스트 파일을 저장합니다.</summary>
		/// <param name="filePath">저장할 파일 경로입니다.</param>
		/// <param name="data">저장할 데이터입니다.</param>
		/// <param name="makeDirectory">
		/// true일 경우 경로가 없다면 생성합니다.
		/// false일 경우 경로가 없다면 예외가 발생합니다.
		/// </param>
		/// <returns>성공 여부입니다.</returns>
		public static JobResult TryWriteText(string filePath, string data, bool makeDirectory = false)
		{
			try
			{
				if (makeDirectory)
				{
					string? fileDirectory = Path.GetDirectoryName(filePath);
					if (fileDirectory != null && !Directory.Exists(fileDirectory))
					{
						Directory.CreateDirectory(fileDirectory);
					}
				}

				File.WriteAllText(filePath, data);
				return new JobResult(result: JobResultType.Success);
			}
			catch (Exception e)
			{
				return new JobResult(result: JobResultType.Failed, exception: e);
			}
		}

		/// <summary>텍스트 파일을 비동기로 저장합니다.</summary>
		/// <param name="folderPath">저장할 파일의 폴더 경로입니다.</param>
		/// <param name="fileNameWithExtension">저장할 파일의 이름입니다. 확장자를 포함합니다.</param>
		/// <param name="data">저장할 데이터입니다.</param>
		/// <param name="makeDirectory">
		/// true일 경우 경로가 없다면 생성합니다.
		/// false일 경우 경로가 없다면 예외가 발생합니다.
		/// </param>
		/// <returns>성공 여부입니다.</returns>
		public static async ValueTask<JobResult> TryWriteTextAsync(string folderPath,
																   string fileNameWithExtension,
																   string data,
																   bool makeDirectory = false,
																   CancellationToken cancellationToken = default)
		{
			string filePath = Path.Combine(folderPath, fileNameWithExtension);
			return await TryWriteTextAsync(filePath, data, makeDirectory, cancellationToken);
		}

		/// <summary>텍스트 파일을 비동기로 저장합니다.</summary>
		/// <param name="filePath">저장할 파일 경로입니다.</param>
		/// <param name="data">저장할 데이터입니다.</param>
		/// <param name="makeDirectory">
		/// true일 경우 경로가 없다면 생성합니다.
		/// false일 경우 경로가 없다면 예외가 발생합니다.
		/// </param>
		/// <returns>성공 여부입니다.</returns>
		public static async ValueTask<JobResult> TryWriteTextAsync(string filePath, string data,
																   bool makeDirectory = false,
																   CancellationToken cancellationToken = default)
		{
			try
			{
				if (makeDirectory)
				{
					string? fileDirectory = Path.GetDirectoryName(filePath);
					if (fileDirectory != null && !Directory.Exists(fileDirectory))
					{
						Directory.CreateDirectory(fileDirectory);
					}
				}

				await File.WriteAllTextAsync(filePath, data, cancellationToken);
				return new JobResult(result: JobResultType.Success);
			}
			catch (Exception e)
			{
				return new JobResult(result: JobResultType.Failed, exception: e);
			}
		}

		/// <summary>텍스트 파일을 읽습니다.</summary>
		/// <param name="folderPath">읽을 파일의 폴더 경로입니다.</param>
		/// <param name="fileNameWithExtension">읽을 파일의 이름입니다. 확장자를 포함합니다.</param>
		/// <returns>읽기 결과입니다. 읽은 데이터를 포함합니다.</returns>
		public static JobResult TryReadText(string folderPath, string fileNameWithExtension)
		{
			string filePath = Path.Combine(folderPath, fileNameWithExtension);
			return TryReadText(filePath);
		}

		/// <summary>텍스트 파일을 읽습니다.</summary>
		/// <returns>읽기 결과입니다. 읽은 데이터를 포함합니다.</returns>
		public static JobResult TryReadText(string filePath)
		{
			try
			{
				return new JobResult(result: JobResultType.Success, value: File.ReadAllText(filePath));
			}
			catch (Exception e)
			{
				return new JobResult(result: JobResultType.Failed, exception: e);
			}
		}

		/// <summary>비동기로 텍스트 파일을 읽습니다.</summary>
		/// <param name="folderPath">읽을 파일의 폴더 경로입니다.</param>
		/// <param name="fileNameWithExtension">읽을 파일의 이름입니다. 확장자를 포함합니다.</param>
		/// <returns>읽기 결과입니다. 읽은 데이터를 포함합니다.</returns>
		public static async ValueTask<JobResult> TryReadTextAsync(string folderPath, string fileNameWithExtension,
																  CancellationToken cancellationToken = default)
		{
			string filePath = Path.Combine(folderPath, fileNameWithExtension);
			return await TryReadTextAsync(filePath, cancellationToken);
		}

		/// <summary>비동기로 텍스트 파일을 읽습니다.</summary>
		/// <returns>읽기 결과입니다. 읽은 데이터를 포함합니다.</returns>
		public static async ValueTask<JobResult> TryReadTextAsync(string filePath,
																  CancellationToken cancellationToken = default)
		{
			try
			{
				return new JobResult(result: JobResultType.Success,
									 await File.ReadAllTextAsync(filePath, cancellationToken));
			}
			catch (Exception e)
			{
				return new JobResult(result: JobResultType.Failed, exception: e);
			}
		}

		public static bool TryDeleteFilesIn(string path, out string[] removeFiles)
		{
			if (Directory.Exists(path))
			{
				removeFiles = Directory.GetFiles(path);
				foreach (var removeFile in removeFiles)
				{
					File.Delete(removeFile);
				}
				return true;
			}

			removeFiles = new string[0];
			return false;
		}

		public static bool TryDeleteFilesIncludeDirectoies(string path)
		{
			if (!Directory.Exists(path))
				return false;

			TryDeleteFilesIn(path, out _);

			foreach (var removeDir in Directory.GetDirectories(path))
			{
				Directory.Delete(removeDir, true);
			}

			return true;
		}
	}
}