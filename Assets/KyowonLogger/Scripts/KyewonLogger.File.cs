using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using UnityEngine;

public static partial class Debug
{
	public enum FileType
	{
		Txt,
		Html
	} 
	private static string CreateLogFileName(string number)
	{
		if (config.GetFileType == FileType.Html)
		{
			return GetDataPath() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + number + ".html";

		}
		else
		{
			return GetDataPath() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "-" + number + ".txt"; 
		}
	}
	private static string GetCurrentLogFileName()
	{
		// 폴더 체크
		string dirpath = GetDataPath();
		if (!Directory.Exists(dirpath))
		{
			Directory.CreateDirectory(dirpath);
			if (config.GetFileType == FileType.Html)
			{
				return GetDataPath() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "-1.html"; 
			}
			else
			{
				return GetDataPath() + "/" + DateTime.Now.ToString("yyyy-MM-dd") + "-1.txt"; 
			}
		}
		//파일 읽어오기
		string[] files = Directory.GetFiles(GetDataPath());
		//오늘 잘짜로 구성된 파일이 있는지 판별
		int maxnumber = 1;
		for (int i = 0; i < files.Length; i++)
		{
			string filename = GetFileName(files[i]);
			string[] arr = filename.Split('-');
			int convertnumber = Convert.ToInt32(arr[arr.Length - 1]);
			if (convertnumber > maxnumber)
			{
				maxnumber = convertnumber;
			} 
		}
		string path = CreateLogFileName(maxnumber.ToString()); 
		FileStream fs = new FileStream(path, FileMode.Append); 
		if (fs.Length < config.GetMaxSizeLogFile * 1024 * 1024)
		{
			fs.Close();
			return path;
		}
		else
		{
			return CreateLogFileName((maxnumber + 1).ToString());
		} 
	}
	static void SaveFile()
	{
		if (logQueue.Count == 0) return;
		// TODO File
		string[] arrlog = logQueue.ToArray();
		//파일명(날짜)
		//날짜 , 시간 ,
		string filepath;
		filepath = GetCurrentLogFileName();
		string dirpath = GetDataPath();

		string _savestr = "";
		for (int i = 0; i < arrlog.Length; i++)
		{
			_savestr = string.Concat(_savestr, arrlog[i], "\n");
		}
		// 폴더 체크
		if (!Directory.Exists(dirpath))
		{
			Directory.CreateDirectory(dirpath);
		}
		// 파일 체크
		FileStream fs = null;
		if (!File.Exists(filepath))
		{
			fs = new FileStream(filepath, FileMode.Create, FileAccess.Write);
		}
		else
		{
			fs = new FileStream(filepath, FileMode.Append);
		}
		StreamWriter sw = new StreamWriter(fs);
		sw.Write(_savestr);
		sw.Close();
		fs.Close();
		logQueue.Clear();
	}
	public static string GetDataPath()
	{
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			return Application.persistentDataPath + "/" + config.GetSavePath;
		}
		// 안드로이드인 경우 폴더 경로 및 파일명이 포함된 경로 생성
		else if (Application.platform == RuntimePlatform.Android)
		{
			return Application.persistentDataPath + "/" + config.GetSavePath;
		}
		// 기타 운영 체제(여기서는 유니티)
		else
		{
			return Application.streamingAssetsPath + "/" + config.GetSavePath;
		}
	}
	private static string GetFileName(string filepath)
	{
		string[] arr = filepath.Split('/');
		string filename = arr[arr.Length - 1];
		filename = filename.Split('.')[0];
		return filename;

	}
	private static bool IsDeleteFile(string filename)
	{
		Match matches = Regex.Match(filename, @".+-", RegexOptions.IgnoreCase); 
		if (matches.Success)
		{
			string time = matches.Groups[0].Value;
			time = time.Substring(0, time.Length - 1);
			DateTime datetime = Convert.ToDateTime(time);

			TimeSpan result = DateTime.Now - datetime;
			if (result.TotalDays > config.GetAutoDeleteWaitDays  )
			{
				return true;
			}
		}
		return false;
	}
	//허용된 일정   날짜가 지난 파일을 정리한다
	public static void CheckLogFiles()
	{
		if (!config.GetAutoDelete) return;
		//저장 경로 내에 있는 모든 파일 읽기
		// 폴더 체크
		string dirpath = GetDataPath();
		if (!Directory.Exists(dirpath))
		{
			return;
		}
		// 파일 체크
		string[] pathfiles = Directory.GetFiles(dirpath);
		for (int i = 0; i < pathfiles.Length; i++)
		{
			bool isdelete = IsDeleteFile(GetFileName(pathfiles[i]));
			if (isdelete)
			{
				File.Delete(pathfiles[i]);
			}
		}
	}
}
