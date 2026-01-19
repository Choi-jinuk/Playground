using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebPacket : BasePacket
{
    public static bool WaitResponse = false;
    
    public string Data;
    public bool IsCheckOverlap = false;
    protected string URL
    {
        get { return string.Empty; }
    }

    public virtual string GetURL()
    {
        return URL;
    }
    
    /// <summary> byte 패킷 방식을 사용할 때 RequestData 를 string 으로 변환하여 저장 </summary>
    public void SetData(Request reqData)
    {
        Data = JsonUtility.ToJson(reqData);
    }

    protected override void Request(BasePacket packet, bool showIndicate, bool isCryptography)
    {
        if (packet is WebPacket webPacket)
        {
            webPacket.SetData(webPacket.GetRequestData());
            WebManager.Instance.Request(webPacket, showIndicate, isCryptography);
        }
    }

    public bool IsEqual(WebPacket packet)
    {
        return GetURL().Equals(packet.GetURL());
    }
}
