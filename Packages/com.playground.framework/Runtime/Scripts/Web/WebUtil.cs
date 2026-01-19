using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebUtil
{
    public static async UniTask<string> WebGet(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {   // 에러 발생 시
            DebugManager.Log(request.error, true);
            return string.Empty;
        }
        else
        {
            return request.downloadHandler.text;
        }
    }

    public static async UniTask<string> WebGet(string url, Callback_float progress)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();

        while (request.isDone == false)
        {
            progress?.Invoke(request.downloadProgress * 100);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {   // 에러 발생 시
            DebugManager.Log(request.error, true);
            return string.Empty;
        }
        else
        {
            return request.downloadHandler.text;
        }
    }
    
    public static async UniTask<object> WebGetByte(string url)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        await request.SendWebRequest();
        
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {   // 에러 발생 시
            DebugManager.Log(request.error, true);
            return null;
        }
        else
        {
            return request.downloadHandler.data;
        }
    }

    public static async UniTask<object> WebGetByte(string url, Callback_float progress)
    {
        UnityWebRequest request = UnityWebRequest.Get(url);
        request.SendWebRequest();

        while (request.isDone == false)
        {
            progress?.Invoke(request.downloadProgress * 100);
            await UniTask.Yield(PlayerLoopTiming.Update);
        }
        
        if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
        {   // 에러 발생 시
            DebugManager.Log(request.error, true);
            return string.Empty;
        }
        else
        {
            return request.downloadHandler.data;
        }
    }
}
