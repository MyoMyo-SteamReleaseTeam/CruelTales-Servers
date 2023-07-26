using System;
using CT.Common.Synchronizations;

namespace CT.Common.Exceptions
{
	public class NetworkRuntimeException : Exception
	{
		public NetworkRuntimeException(string message) : base(message) { }
	}

	public class WrongSyncType : Exception
	{
		public WrongSyncType(SyncType syncType) : base($"You cannot synchronize when it's {syncType}") {}
	}

	public class TooLongSteamDataException : Exception
	{
		private const string mDefualtMessage = "스트림 데이터가 너무 깁니다.";

		public TooLongSteamDataException(int currentLength, int maxLength)
			: base($"{mDefualtMessage}\n현재 : {currentLength}\n최대 : {maxLength}") { }

		public TooLongSteamDataException(int currentLength)
			: base($"{mDefualtMessage}\n현재 : {currentLength}") { }
	}

	public class MtuSizeOutOfRange : Exception
	{
		public MtuSizeOutOfRange() : base($"MTU 크기를 넘었습니다.") { }
	}

	public class RequestConnectError : Exception
	{
		public RequestConnectError(Exception e)
			: base($"연결 요청 패킷 분석을 실패했습니다.\nErrorMessage : {e}") { }
	}

	public class RequestDisconnectError : Exception
	{
		public RequestDisconnectError(Exception e)
			: base($"접속 종료 요청 패킷 분석을 실패했습니다.\nErrorMessage : {e}") { }
	}

	public class RequestHeartbeatError : Exception
	{
		public RequestHeartbeatError(Exception e)
			: base($"연결 확인 요청 패킷 분석을 실패했습니다.\nErrorMessage : {e}") { }
	}

	public class RequestObjectSynchronizeError : Exception
	{
		public RequestObjectSynchronizeError(Exception e)
			: base($"객체 동기화 요청 패킷 분석을 실패했습니다.\nErrorMessage : {e}") { }
	}

	public class TooManySyncDataException : Exception
	{
		public TooManySyncDataException(int maxSize = 255)
			: base($"동기화 데이터 개수가 너무 많습니다. {maxSize}개 이하여야합니다.") { }
	}

	//public class SyncIndexError : Exception
	//{
	//	public SyncIndexError(NetworkObject no, int syncIndex)
	//		: base($"Sync index error! You try to get index : {syncIndex} in \"{no.GetType().Name}\"") { }
	//}

	//public class SyncDeserializeFieldError : Exception
	//{
	//	public SyncDeserializeFieldError(NetworkObject no, Synchronizer synchronizer)
	//		: base($"Sync field deserialize fail! \"{synchronizer.GetType().Name}\" in \"{no.GetType().Name}\", Index : {synchronizer.SyncIndex}") { }
	//}

	//public class SyncIgnoreDeserializeFieldError : Exception
	//{
	//	public SyncIgnoreDeserializeFieldError(NetworkObject no, Synchronizer synchronizer)
	//		: base($"Ignore field sync deserialize fail! \"{synchronizer.GetType().Name}\" in \"{no.GetType().Name}\", Index : {synchronizer.SyncIndex}") { }
	//}

	//public class SyncDeserializeRpcError : Exception
	//{
	//	public SyncDeserializeRpcError(NetworkObject no, RpcBase rpc)
	//		: base($"Sync RPC deserialize fail! \"{rpc.GetType().Name}\" in \"{no.GetType().Name}\", Index : {rpc.SyncIndex}") { }
	//}

	//public class SyncIgnoreDeserializeRpcError : Exception
	//{
	//	public SyncIgnoreDeserializeRpcError(NetworkObject no, RpcBase rpc)
	//		: base($"Ignore RPC sync deserialize fail! \"{rpc.GetType().Name}\" in \"{no.GetType().Name}\", Index : {rpc.SyncIndex}") { }
	//}

	//public class SyncCountParseError : Exception
	//{
	//	public SyncCountParseError()
	//		: base($"Counter paser error! 빌드 버전이 다를 수 있습니다.") { }
	//}
}
