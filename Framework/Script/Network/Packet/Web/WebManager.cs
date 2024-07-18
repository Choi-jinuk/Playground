using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

public class WebManager : Singleton<WebManager>
{
    class WebPacketInfo
    {
        public WebPacket Packet;
        public bool IsShowIndicate = false;

        public WebPacketInfo(WebPacket packet, bool isShowIndicate)
        {
            Packet = packet;
            IsShowIndicate = isShowIndicate;
        }
    }
    
    private static DateTime _lastTime;

    private WebPacket _sendPacket = null;
    private WebPacketInfo _failedPacket = null;
    private Queue<WebPacketInfo> _packetList = new Queue<WebPacketInfo>();
    
    public void Request(WebPacket packet, bool showIndicate, bool isCryptography = true)
    {
        if (CheckLastPacketTime() == false)
        {
            return;
        }

        if (_failedPacket != null && _failedPacket.Packet.IsEqual(packet))
        {   //실패 패킷과 중복 체크
            if (packet.IsCheckOverlap == false)
            {   //중복가능한 패킷
                AddPacket(packet, showIndicate);
            }
            return;
        }

        if (_sendPacket != null && _sendPacket.IsEqual(packet) && _sendPacket.IsCheckOverlap)
        {   //처리중인 패킷과 중복 체크
            return;
        }

        if (_sendPacket != null && _failedPacket != null)
        {
            if (IsDuplicate(packet))
            {
                return;
            }
            AddPacket(packet, showIndicate);
        }
        
        RequestAsync(packet, showIndicate, isCryptography).Forget();
    }

    async UniTask RequestAsync(WebPacket packet, bool showIndicate, bool isCryptography = true)
    {
        _sendPacket = packet;
        if (showIndicate)
        {   //Show Loading
            
        }

        WebPacket.WaitResponse = true;
        if (string.IsNullOrEmpty(packet.Data))
        {
            WebPacket.WaitResponse = false;
            return;
        }

        float timer = 0f;
        float timeOut = 0f;
        bool failed = false;
        UnityWebRequest www = SendPacket(packet, isCryptography);
        while (www.isDone == false || _CheckResponseCode(www))
        {
            if (timer > timeOut)
            {
                if (_CheckResponseCode(www))
                {
                    
                }

                failed = true;
                break;
            }

            timer += Time.unscaledDeltaTime;
        }

        if (failed || string.IsNullOrEmpty(www.error) == false)
        {
            _failedPacket = new WebPacketInfo(packet, true);
        }

        WebPacket.WaitResponse = false;
        if (failed == false && showIndicate)
        {   //Hide Loading
            
        }

        string response = www.downloadHandler.text;
        if (string.IsNullOrEmpty(response))
        {
            return;
        }
        packet.Deserialize(response);
        packet.OnResponse();
    }

    bool _CheckResponseCode(UnityWebRequest www)
    {
        return www.responseCode > 0 && www.responseCode != (long)System.Net.HttpStatusCode.OK;
    }

    UnityWebRequest SendPacket(WebPacket packet, bool isCryptography)
    {
        string data = packet.Data;
        if (isCryptography)
        {
            data = MD5CryptoUtil.EncodeMD5(data);
        }

        byte[] byteData = System.Text.Encoding.UTF8.GetBytes(data);
        UnityWebRequest www = new UnityWebRequest(packet.GetURL(), "POST");
        www.uploadHandler = new UploadHandlerRaw(byteData);
        www.downloadHandler = new DownloadHandlerBuffer();

        www.SendWebRequest();
        return www;
    }

    bool CheckLastPacketTime()
    {
        TimeSpan ts = TimeManager.Instance.NOW - _lastTime;
        _lastTime = TimeManager.Instance.NOW;
        if (ts.Minutes >= 5f)
        {
            return false;
        }

        return true;
    }
    
    void AddPacket(WebPacket packet, bool showIndicate)
    {
        WebPacketInfo info = new WebPacketInfo(packet, showIndicate);
        _packetList.Enqueue(info);
    }
    
    bool IsDuplicate(WebPacket packet)
    {
        if(packet.IsCheckOverlap)
        {
            foreach(WebPacketInfo info in _packetList)
            {
                if(info.Packet.IsEqual(packet))
                {
                    return true;
                }
            }
        }
        return false;
    }
}
