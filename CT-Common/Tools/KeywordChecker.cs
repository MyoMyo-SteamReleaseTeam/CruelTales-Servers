using System.Collections.Generic;

namespace CT.Common.Tools
{
	public static class KeywordChecker
	{
		public static readonly HashSet<string> _keywordSet = new HashSet<string>()
		{
			"abstract", "as", "base", "bool", "break","byte", "case",
			"catch", "char", "checked", "class", "const", "continue", "decimal",
			"default", "delegate", "do", "double", "else", "enum", "event",
			"explicit", "extern", "false", "finally", "fixed", "float", "for",
			"foreach", "goto", "if", "implicit", "in", "int", "interface","internal",
			"is", "lock", "long", "namespace", "new", "null", "object", "operator",
			"out", "override", "params", "private", "protected", "public", "readonly",
			"ref", "return", "sbyte", "sealed", "short", "sizeof", "stackallocstatic",
			"string", "struct", "switch", "this", "throw", "true", "try", "typeof",
			"uint", "ulong", "unchecked", "unsafe", "ushort", "using", "virtual",
			"void", "volatile", "while",
			
			// Contextual keywords
			"add", "and", "alias", "ascending", "args", "async", "await", "by",
			"descending", "dynamic", "equals", "file", "from", "get", "global",
			"group", "init", "into", "join", "let", "managed", "nameof", "nint",
			"not", "notnull", "nuint", "on", "or","orderby", "partial", "record",
			"remove", "required", "scoped", "select", "set", "unmanaged", "value",
			"var", "when", "where", "with", "yield",
		};

		public static IReadOnlyCollection<string> Keywords() => _keywordSet;

		public static bool IsKeyword(string value) => _keywordSet.Contains(value);
	}
}
