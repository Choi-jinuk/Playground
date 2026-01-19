using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDataManager
{
    private string _GetLocalCRCPath()
    {
        return StringUtil.Append(Application.persistentDataPath, "/", GlobalString.SAVE_DATA_CRC);
    }
    private string _GetLocalGameDataPath()
    {
        return StringUtil.Append(Application.persistentDataPath, "/", GlobalString.DATA_JSON);
    }

    private string _GetCdnCRCPath()
    {
#if UNITY_IOS
        return StringUtil.Append(GlobalString.CDN_ADDRESS, GlobalString.CDN_IOS, Application.version, "/", GlobalString.CRC_TXT);
#else
        return StringUtil.Append(GlobalString.CDN_ADDRESS, GlobalString.CDN_ANDROID, Application.version, "/", GlobalString.CRC_TXT);
#endif
    }
    private string _GetCdnGameDataPath()
    {
#if UNITY_IOS
        return StringUtil.Append(GlobalString.CDN_ADDRESS, GlobalString.CDN_IOS, Application.version, "/", GlobalString.DATA_JSON);
#else
        return StringUtil.Append(GlobalString.CDN_ADDRESS, GlobalString.CDN_ANDROID, Application.version, "/", GlobalString.DATA_JSON);
#endif
    }
}
