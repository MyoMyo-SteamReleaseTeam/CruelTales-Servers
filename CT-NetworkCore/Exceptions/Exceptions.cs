using System;

namespace CT.Network.Exceptions
{
	public class TooLongSteamDataException : Exception
	{
		private const string mDefualtMessage = "스트림 데이터가 너무 깁니다.";

		public TooLongSteamDataException(int currentLength, int maxLength)
			: base($"{mDefualtMessage}\n현재 : {currentLength}\n최대 : {maxLength}") { }

		public TooLongSteamDataException(int currentLength)
			: base($"{mDefualtMessage}\n현재 : {currentLength}") { }
	}

	//public class TooManyPropertiesOnNetObjectException : Exception
	//{
	//	public TooManyPropertiesOnNetObjectException(int maxPropertySize)
	//		: base($"네트워크 객체의 프로퍼티가 너무 많습니다. {maxPropertySize}개 이하여야합니다.") { }
	//}

	public class MtuSizeOutOfRange : Exception
	{
		public MtuSizeOutOfRange() : base($"MTU 크기를 넘었습니다.") { }
	}

	//public class WrongRpcCallerName : Exception
	//{
	//	public WrongRpcCallerName(int minimumLength)
	//		: base($"RPC 필드 이름이 너무 짧습니다. 최소 {minimumLength}자 이상이어야 합니다.") { }

	//	public WrongRpcCallerName(string rpcCallerName)
	//		: base($"RPC 필드 이름이 잘못되었습니다. {rpcCallerName}는 RPC_ 접두사를 가져아합니다.") { }
	//}

	//public class CannotFoundRpcMethod : Exception
	//{
	//	public CannotFoundRpcMethod(Type type, string methodName)
	//		: base($"{type.Name}에 {methodName} 함수가 없습니다.") { }
	//}

	//public class CannotFoundField : Exception
	//{
	//	public CannotFoundField(Type type, string fieldName)
	//		: base($"{type.Name}에 {fieldName} 필드가 없습니다") { }
	//}

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

	public class AuthorityError : Exception
	{
		public AuthorityError(string message)
			: base($"유효하지 않은 권한입니다. {message}") { }
	}

	public class DataParseError : Exception
	{
		public DataParseError(string name)
			: base($"데이터 파싱에 실패했습니다. Key : {name}") { }
	}
}
