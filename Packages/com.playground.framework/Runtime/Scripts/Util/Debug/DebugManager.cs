using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Debug = UnityEngine.Debug;

public class DebugManager
{
    public static void Log(object message, bool bForce = false, 
        [CallerFilePath] string pathName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
    {
	    if(DataUtil.IsTestMode)
			Debug.Log(message + _GetLogPrefix(pathName, memberName,lineNumber));
	    else
			if(bForce) Debug.Log(message + _GetLogPrefix(pathName, memberName,lineNumber));
    }
    public static void LogFormat(object message, bool bForce = false, 
        [CallerFilePath] string pathName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0,
        params object[] data)
    {
	    if(DataUtil.IsTestMode)
			Debug.Log(message + _GetLogPrefix(pathName, memberName,lineNumber));
		else
			if(bForce) Debug.Log(message + _GetLogPrefix(pathName, memberName,lineNumber));

    }
    public static void LogWarning(object message, bool bForce = false, 
        [CallerFilePath] string pathName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0,
        params object[] data)
    {
	    if(DataUtil.IsTestMode)
			Debug.LogWarning(message + _GetLogPrefix(pathName, memberName,lineNumber));
		else
			if(bForce) Debug.LogWarning(message + _GetLogPrefix(pathName, memberName,lineNumber));
    }
    public static void LogError(object message, bool bForce = false,
        [CallerFilePath] string pathName = "", [CallerMemberName] string memberName = "", [CallerLineNumber] int lineNumber = 0)
    {
	    if(DataUtil.IsTestMode)
			Debug.LogError(message + _GetLogPrefix(pathName, memberName,lineNumber));
	    else if(bForce)
		    Debug.LogError(message + _GetLogPrefix(pathName, memberName,lineNumber));
    }

    public static void LogStackTrace(object message)
    {
	    if(DataUtil.IsTestMode)
	    {
		    var st = new StackTrace(1, true);
		    string strStackTrace = message.ToString();
		    for (int i = 0; i < st.FrameCount; i++)
		    {
			    StackFrame sf = st.GetFrame(i);
			    string strTemp = "	at ";
			    strTemp = StringUtil.Append(strTemp, sf.GetFileName(), ".", sf.GetMethod().Name, " ()  in ", sf.GetFileLineNumber().ToString());
			    strStackTrace = StringUtil.AppendLine(strStackTrace, strTemp);
		    }

		    Debug.LogError(strStackTrace);
	    }
    }

    static string _GetLogPrefix(string pathName, string memberName, int lineNumber)
    {
        return $"  {new StackFrame(2).GetMethod().DeclaringType?.Name}.{memberName} (at {pathName.Substring(pathName.IndexOf("Asset", StringComparison.Ordinal)).Replace("/", "//")}:{lineNumber})";
    }
}
