using Microsoft.SqlServer.Server;
using System;
using System.Data.SqlTypes;
using System.Text.RegularExpressions;

public static class UserDefinedFunctions {

	/// <summary>
	/// Performs a regular expression comparison on the input passed, using the regular expression. Returns 0 on no match and 1 on success.
	/// </summary>
	/// <param name="input">The value to check</param>
	/// <param name="pattern">The regular expression to check against</param>
	/// <param name="options">The regular expression options as a string of flags. Options include m = MultiLine, s = SingleLine, i = IgnoreCase</param>
	/// <returns></returns>
	[SqlFunction(DataAccess = DataAccessKind.None)]
	public static SqlBoolean RegexIsMatch(SqlChars input, SqlString pattern, SqlString options) {

		var regex = new Regex(pattern.Value, options.IsNull ? RegexOptions.None : ResolveOptions(options.Value));

		return regex.IsMatch(new string(input.Value));
	}

	/// <summary>
	/// Performs a regular expression comparison on the input passed, using the regular expression and replaces any matches with the replacement
	/// </summary>
	/// <param name="input">The value to check</param>
	/// <param name="pattern">The regular expression to check against</param>
	/// <param name="replacement">The string to replace any values with</param>
	/// <param name="options">The regular expression options as a string of flags. Options include m = MultiLine, s = SingleLine, i = IgnoreCase</param>
	/// <returns></returns>
	[SqlFunction(DataAccess = DataAccessKind.None)]
	public static SqlChars RegexReplace(SqlChars input, SqlString pattern, SqlString replacement, SqlString options) {

		var regex = new Regex(pattern.Value, options.IsNull ? RegexOptions.None : ResolveOptions(options.Value));

		return new SqlChars(regex.Replace(new string(input.Value), replacement.Value).ToCharArray());
	}

	static RegexOptions ResolveOptions(string options) {

		RegexOptions ops = RegexOptions.None;

		if (options.Contains("i")) {
			ops |= RegexOptions.IgnoreCase;
			ops |= RegexOptions.CultureInvariant;
		}

		if (options.Contains("m")) {
			ops |= RegexOptions.Multiline;
		}

		if (options.Contains("s")) {
			ops |= RegexOptions.Singleline;
		}

		return ops;
	}
}