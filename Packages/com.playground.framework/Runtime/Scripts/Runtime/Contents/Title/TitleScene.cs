using System;
using System.Collections.Generic;
using UnityEngine;

public class TitleScene : MonoBehaviour
{
    private async void Awake()
    {
        //Cdn 주소 설정
        
        await AddressableManager.Init();
    }

    private async void Start()
    {
        //게임 데이터 다운로드 & 데이터 매니저 Load
        
        var listDownload = new List<stDownloadData>();
        var downloadSize = await AddressableManager.CheckDownload(listDownload);
        await AddressableManager.StartDownload(downloadSize, listDownload);

        //외부 플러그인 Init
        
        //게임 매니저 Init
        UIManager.Instance.Init();
        SoundManager.Instance.Init();
        
        //게임 데이터 Load
        
        SceneManager.LoadScene(GlobalString.LOBBY_SCENE);
    }
}
