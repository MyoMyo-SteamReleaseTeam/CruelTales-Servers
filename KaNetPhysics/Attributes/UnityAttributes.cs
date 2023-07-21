using System;

#if NET
namespace Sirenix.OdinInspector
{
	public class ShowInInspectorAttribute : Attribute
	{
	}
}

namespace UnityEngine
{
	public class SerializeFieldAttribute : Attribute
	{

	}
}
#endif