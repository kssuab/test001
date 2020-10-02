using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;


public  static partial class Debug {
	
	
	public static bool isDebugBuild { get { return UnityEngine.Debug.isDebugBuild; } }
	public enum LogLevel { Normal, Debug, Deep }
	public static Thread logchecker;
	public static LogConfig config;

	 
	static Debug() {
	 
		if (config == null)
		{
			config = Resources.Load<LogConfig>("KyowonLogger/Asset/Kyowon Logger");//  AssetDatabase.LoadAssetAtPath<LogConfig>("Assets /KyowonLogger/Scripts/Kyowon Logger.asset");
		}
		 
		CheckLogFiles();

		if ((Application.isEditor || isDebugBuild) && config.LogLevel < LogLevel.Debug)
			config.LogLevel = LogLevel.Debug;
       // 익셉션 발생시 Debug.Log를 거치지 않으므로 따로 추가 처리함
        Application.logMessageReceived += (condition, stackTrace, type) =>
        {
			if (! config.GetIgnoreTraceInfo)
			{
				logToFile(condition, stackTrace, type);
			}
			else
			{
				logToFile(condition, string.Empty, type);
			} 
		};
	}


	public static void LogPrimary(object message)
	{
		UnityEngine.Debug.Log(message);
		//logToFile(message.ToString());
	}
	public static void LogError(object message, Object context = null)
	{
		UnityEngine.Debug.LogError(message, context);
		//logToFile(message.ToString());
	}
	public static void LogErrorFormat(string message, params object[] args)
	{
		UnityEngine.Debug.LogErrorFormat(message, args);
		//logToFile(message);
	}
	public static void LogException(Exception message, Object context = null)
	{
		// 수동으로 익셉션 로그 발생시 시스템 크래시와 구분이 되지 않으므로 저장 루틴 생략하고 추가 처리 루틴에서 저장하도록 함
		UnityEngine.Debug.LogException(message, context);
	}
	public static void Log(object message)
	{
		if (config.LogLevel > LogLevel.Normal)
		{
			UnityEngine.Debug.Log(message);
		}
	}
	public static void Log(object message, Object context)
	{
		if (config.LogLevel > LogLevel.Normal)
		{
			UnityEngine.Debug.Log(message, context);
		}
	}
	public static void LogWarning(object message, Object context = null)
	{
		if (config.LogLevel > LogLevel.Normal)
		{
			UnityEngine.Debug.LogWarning(message, context);
		}
	}
	public static void LogFormat(string message, params object[] args)
	{
		if (config.LogLevel > LogLevel.Normal)
		{
			UnityEngine.Debug.LogFormat(message, args);
		}
	} 
	public static void LogDeep(object message)
	{
		if (config.LogLevel >= LogLevel.Deep)
		{
			UnityEngine.Debug.Log(message);
		}
	} 
	#region LogFile
	static Queue<string> logQueue = new Queue<string>();
	static void logToFile(string log ,string trace , LogType type)
	{
		switch (config.GetFileType)
		{
			case FileType.Txt:
				logQueue.Enqueue(string.Concat("===",
					                           DateTime.Now.ToString("yyyy-MM-dd h:mm tt"),
											   "====",
											   "\n",
											   log,
											   "\n",
											   trace));
				break;
			case FileType.Html:
				logQueue.Enqueue(string.Concat(
											"<p>===", DateTime.Now.ToString("yyyy-MM-dd h:mm tt"), "====</p>",
											GetColorForamt(type),
											log,
											"</p>",
											"<p>",
											trace,
											"</p>"));
				break;
		} 
		CheckChecker();
	}
	#endregion
	#region Checker(Thread)
	private static void CheckChecker()
	{
		if (logchecker == null)
			StartChecker();
	}
	 
	private static void StartChecker()
	{
		
		if (logchecker == null)
		{
			logchecker = new Thread(CheckQueue);
			logchecker.Start();
			UnityEngine.Application.quitting += KillThread;
		}
	
	}
	private static void CheckQueue()
	{
		while (true)
		{
			if (logQueue.Count > 0)
			{ 
				SaveFile();
			}
			
			Thread.Sleep(config.GetWaitTime);
		} 
	}
	private static void KillThread()
	{
		SaveFile();
		if (logchecker != null)
		{ 
			logchecker.Abort();
		} 
	} 
 
	#endregion

}
