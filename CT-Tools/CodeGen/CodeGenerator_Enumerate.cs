using System;
using System.Collections.Generic;
using System.Text;

namespace CT.Tools.CodeGen
{
	public static class CodeGenerator_Enumerate
	{
		/// <summary>Enum 코드를 생성합니다.</summary>
		/// <param name="enumName">Enum의 이름입니다.</param>
		/// <param name="enumNamespace">Enum이 속한 Namespace입니다.</param>
		/// <param name="hasNone">
		/// 첫 요소로 None을 포함하는지 여부입니다. 포함한다면 "None = 0"이 첫번재 요소가 됩니다.
		/// </param>
		/// <param name="useTab">들여쓰기가 탭인지 여부입니다.</param>
		/// <param name="usingList">using 목록입니다.</param>
		/// <param name="items">Enum 요소 리스트입니다.</param>
		/// <returns>생성된 코드입니다.</returns>
		public static string Generate(string enumName, string enumNamespace,
									  bool hasNone, bool useTab,
									  IList<string> usingList, IList<string> items, bool addUsingAndSemicolon = true)
		{
			string indent = useTab ? "\t" : "    ";

			StringBuilder sb = new StringBuilder(1024 * 16);

			foreach (var u in usingList)
			{
				if (addUsingAndSemicolon)
				{
					sb.AppendLine($"using {u};");
				}
				else
				{
					sb.AppendLine(u);
				}
			}

			// Add namespace
			if (usingList.Count != 0)
			{
				sb.AppendLine("");
			}

			sb.AppendLine($"namespace {enumNamespace}");
			sb.AppendLine("{");

			// Add declaration
			sb.AppendLine($"{indent}public enum {enumName}");
			sb.AppendLine(indent + "{");

			// Add items
			if (hasNone)
			{
				sb.AppendLine($"{indent}{indent}None = 0,");
			}

			foreach (var item in items)
			{
				sb.AppendLine($"{indent}{indent}{item},");
			}

			// Add tails
			sb.AppendLine(indent + "}");
			sb.AppendLine("}");

			return sb.ToString();
		}

		/// <summary>
		/// Enum 코드를 Name 테이블과 함께 생성합니다. Enum 이름의 정적 Extension 클래스가 함께 생성됩니다.
		/// 생성된 확장 클래스에는 각 Enum의 요소와 이름이 매칭된 정적 Dictionary 클래스가 함께 선언됩니다.
		/// 인자로 들어오는 items와 names는 반드시 같은 길이어야합니다.
		/// </summary>
		/// <param name="enumName">Enum의 이름입니다.</param>
		/// <param name="enumNamespace">Enum이 속한 Namespace입니다.</param>
		/// <param name="hasNone">
		/// 첫 요소로 None을 포함하는지 여부입니다. 포함한다면 "None = 0"이 첫번재 요소가 됩니다.
		/// </param>
		/// <param name="useTab">들여쓰기가 탭인지 여부입니다.</param>
		/// <param name="usingList">using 목록입니다.</param>
		/// <param name="items">Enum 요소 리스트입니다.</param>
		/// <param name="names">이름 리스트입니다.</param>
		/// <param name="noneName">None이 있는 경우 None의 name 이름입니다.</param>
		/// <returns>생성된 코드입니다.</returns>
		public static string GenerateWithNameTable(string enumName, string enumNamespace, bool hasNone, bool useTab,
												   List<string> usingList, IList<string> items,
												   IList<string> names, string noneName)
		{
			if (names.Count != items.Count)
			{
				throw new ArgumentException("The number of \"Names\" and \"Items\" are different.");
			}

			string indent1 = useTab ? "\t" : "    ";
			string indent2 = indent1 + indent1;
			string indent3 = indent1 + indent2;

			StringBuilder sb = new StringBuilder(1024 * 16);

			// Add using code
			string defaultUsing = "System.Collections.Generic";

			if (!usingList.Contains(defaultUsing))
			{
				usingList.Add(defaultUsing);
			}

			// Enum section
			// Add using code
			foreach (var u in usingList)
			{
				sb.AppendLine($"using {u};");
			}

			// Add namespace
			if (usingList.Count != 0)
			{
				sb.AppendLine("");
			}

			sb.AppendLine($"namespace {enumNamespace}");
			sb.AppendLine("{");

			// Add declaration
			sb.AppendLine($"{indent1}public enum {enumName}");
			sb.AppendLine(indent1 + "{");

			// Add items
			if (hasNone)
			{
				sb.AppendLine($"{indent2}None = 0,");
			}

			foreach (var item in items)
			{
				sb.AppendLine($"{indent2}{item},");
			}

			// Add tails
			sb.AppendLine(indent1 + "}");

			// Extension section
			string tableName = $"m{enumName}Table";

			sb.AppendLine();

			sb.AppendLine($"{indent1}public static class {enumName}Extension");
			sb.AppendLine(indent1 + "{");
			sb.AppendLine($"{indent2}private static Dictionary<{enumName}, string> {tableName} = new()");
			sb.AppendLine($"{indent2}" + "{");
			if (hasNone)
			{
				sb.Append(indent3);
				sb.Append("{ ");
				sb.Append($"{enumName}.None, \"{noneName}\"");
				sb.AppendLine(" },");
			}
			for (int i = 0; i < items.Count; i++)
			{
				sb.Append(indent3);
				sb.Append("{ ");
				sb.Append($"{enumName}.{items[i]}, \"{names[i]}\"");
				sb.AppendLine(" },");
			}
			sb.AppendLine($"{indent2}" + "};");
			sb.AppendLine();
			sb.AppendLine($"{indent2}public static string GetName(this {enumName} value)");
			sb.AppendLine($"{indent2}" + "{");
			sb.AppendLine($"{indent3}return {tableName}[value];");
			sb.AppendLine($"{indent2}" + "}");
			sb.AppendLine(indent1 + "}");

			sb.AppendLine("}");

			return sb.ToString();
		}

		public static string GenerateValueDropdownList(string collectionName, string collectionNamespace,
													   bool hasNone, bool useTab,
													   List<string> usingList, IList<string> items)
		{
			string indent1 = useTab ? "\t" : "    ";
			string indent2 = indent1 + indent1;
			string indent3 = indent1 + indent2;

			StringBuilder sb = new StringBuilder(1024 * 16);

			// Add using code
			string defaultUsing_1 = "System.Collections";
			string defaultUsing_2 = "Sirenix.OdinInspector";

			if (!usingList.Contains(defaultUsing_1))
				usingList.Add(defaultUsing_1);

			if (!usingList.Contains(defaultUsing_2))
				usingList.Add(defaultUsing_2);

			foreach (var u in usingList)
			{
				sb.AppendLine($"using {u};");
			}

			// Add namespace
			if (usingList.Count != 0)
			{
				sb.AppendLine("");
			}

			sb.AppendLine($"namespace {collectionNamespace}");
			sb.AppendLine("{");

			sb.AppendLine($"{indent1}public static class {collectionName}Extension");
			sb.AppendLine(indent1 + "{");

			// Add declaration
			sb.AppendLine($"{indent2}public static IEnumerable {collectionName} = new ValueDropdownList<string>()");
			sb.AppendLine(indent2 + "{");

			// Add items
			if (hasNone)
			{
				sb.AppendLine($"{indent3}" + "{ \"None\", \"none\" },");
			}

			foreach (var item in items)
			{
				sb.Append(indent3);
				sb.AppendLine("{ \"" + item + "\", \"" + item + "\" },");
			}

			// Add tails
			sb.AppendLine(indent2 + "};");
			sb.AppendLine(indent1 + "}");
			sb.AppendLine("}");

			return sb.ToString();
		}
	}
}
