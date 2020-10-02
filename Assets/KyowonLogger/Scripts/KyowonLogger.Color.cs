using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static partial class Debug
{
	//각 로그 칼라값 
	private static string GetColorForamt(UnityEngine.LogType type)
	{
		switch (type)
		{
			case UnityEngine.LogType.Log:
				return string.Format("<p style =\"color:#{0}{1}",ColorUtility.ToHtmlStringRGBA(  config.GetLogColor), "\";>");
			case UnityEngine.LogType.Warning:
				return string.Format("<p style =\"color:#{0}{1}", ColorUtility.ToHtmlStringRGBA(config.GetWarningColor), "\";>");
			case UnityEngine.LogType.Error:
				return string.Format("<p style =\"color:#{0}{1}", ColorUtility.ToHtmlStringRGBA(config.GetErrorColor), "\";>");
			case UnityEngine.LogType.Exception:
				return string.Format("<p style =\"color:#{0}{1}", ColorUtility.ToHtmlStringRGBA(config.GetExceptionColor), "\";>");
			default:

				return string.Empty;
		}

	}
	 

}
