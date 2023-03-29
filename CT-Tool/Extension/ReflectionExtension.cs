using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ReflectionExtension
{
	/// <summary>지정한 어셈블리에서 지정한 타입의 자식 클래스 타입들을 반환합니다.</summary>
	/// <param name="typeInAssembly">추출할 어셈블리입니다.</param>
	/// <param name="baseType">부모 타입입니다.</param>
	/// <returns>부모 타입을 상속받은 클래스의 타입들입니다.</returns>
	public bool TryGetSubclassTypesFromAssembly(Type typeInAssembly,
												Type baseType,
												out IEnumerable<Type>? types)
	{
		types = Assembly
			.GetAssembly(typeInAssembly)?
			.GetTypes()
			.Where(t => t.IsSubclassOf(baseType));
		
		return types != null;
	}
}
