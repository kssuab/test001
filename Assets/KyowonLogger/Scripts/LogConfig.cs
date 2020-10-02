using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Kyowon Logger", menuName = "Kyowon Logger Config")]
public class LogConfig : ScriptableObject
{

    [SerializeField]
    private Debug.LogLevel loglevel;
     public Debug.LogLevel LogLevel
    {
        get => loglevel;
        set => loglevel = value;
    }

    [SerializeField, Header("로그 파일 저장 경로"), Tooltip("로그 파일이 저장될 위치 Editor : Application.streamingasset/ + \"SavePath\"")]
    private string savePath ;

    [SerializeField, Header("타이머 대기시간(ms)"), Range(1000, 60000)]
    private int waittime;
    [SerializeField, Header("저장파일 타입")]
    private Debug.FileType fileType;
    [SerializeField, Header("로그 트레이스 정보 기록 여부")]
    private bool isIgnoreTraceInfo;

    [SerializeField, Header("로그파일 크기"), Range(1, 100), Tooltip("용량 단위 : mb")]
    private int maxsizelogfile;
    [SerializeField, Header("자동삭제 기능")]
    private bool autodelete;
    [SerializeField, Header("자동삭제 대기날짜"), Range(1, 30)]
    public int autodeletewaitdays ;

    [SerializeField, Header("Log 색상(Html에서만 확인 가능")]
    private Color logcolor;
    [SerializeField, Header("Warning 색상(Html에서만 확인 가능")]
    private Color warningcolor;
    [SerializeField, Header("Error 색상(Html에서만 확인 가능")]
    private Color errorcolor;
    [SerializeField, Header("Exception 색상(Html에서만 확인 가능")]
    private Color exceptioncolor;

    //// get
   
    public string GetSavePath => savePath;
    public int GetWaitTime => waittime;
    public Debug.FileType GetFileType => fileType;
    public bool GetIgnoreTraceInfo => isIgnoreTraceInfo;
    public int GetMaxSizeLogFile => maxsizelogfile;
    public bool GetAutoDelete => autodelete;
    public int GetAutoDeleteWaitDays => autodeletewaitdays;
    public Color GetLogColor => logcolor;
    public Color GetWarningColor => warningcolor;
    public Color GetErrorColor => errorcolor;
    public Color GetExceptionColor => exceptioncolor;

    
}
