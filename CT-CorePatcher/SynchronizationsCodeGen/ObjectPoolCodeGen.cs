using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CT.CorePatcher.SynchronizationsCodeGen
{
	internal static class ObjectPoolCodeGen
	{
		public static string GenerateMasterNetworkObjectPoolCode(List<SyncObjectInfo> objList)
		{
			var netObjs = objList.Where(obj => obj.SyncObjectType == SyncObjectType.NetworkObject);

			StringBuilder content = new StringBuilder(2048);
			foreach (var netObj in netObjs)
			{
				string capacityStr = netObj.Capacity.ToString();
				string capacityDeclare = netObj.MultiplyByMaxUser ? $"{capacityStr} * {ObjectPoolFormat.MaxUserCount}" : capacityStr;
				content.AppendFormat(ObjectPoolFormat.PoolByType, netObj.ObjectName, capacityDeclare);
				content.AppendLine();
			}

			CodeFormat.AddIndent(content, 3);
			return string.Format(ObjectPoolFormat.MasterNetworkObjectPool, content);
		}
	}
}
