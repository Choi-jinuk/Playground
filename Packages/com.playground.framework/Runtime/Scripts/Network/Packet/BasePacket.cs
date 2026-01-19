using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

[Serializable]
public class Response
{
    public int Code;
    public string Message;
}

[Serializable]
public class Request
{
    
}

public class BasePacket
{
    public Callback OnResponseCB;
    public Callback_int OnResponseErrorCB;
    public virtual void Deserialize(string response) { }
    protected virtual Response GetResponseData()
    {
        return null;
    }
    protected virtual Request GetRequestData()
    {
        return null;
    }

    /// <summary> 현재 데이터가 정상적인 데이터 인지 체크 </summary>
    /// <param name="reqData"> WebPacket 일 경우  </param>
    public void CheckGameData(Request reqData)
    {
        GameDataManager.Instance.CheckNewGameData().Forget();
    }
    
    protected virtual void Request(BasePacket packet, bool showIndicate, bool isCryptography) { }

    public void OnResponse()
    {
        if (GetResponseData() == null)
        {
            return;
        }

        int code = GetResponseData().Code;
        if (code != 0)
        {
            if (OnResponseErrorCB != null)
                OnResponseErrorCB(code);
            else
                OnError(code, GetResponseData().Message);
            return;
        }
        
        OnResponseCB?.Invoke();
    }

    void OnError(int code, string message)
    {
        var result = (ServerPacketType.ePacketResult)code;
        
    }
}