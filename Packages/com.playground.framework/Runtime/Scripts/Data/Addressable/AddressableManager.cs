using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class AssetList
{
    public List<string> assets = new List<string>();
}
public partial class AddressableManager
{
    public static async UniTask Init()
    {
        await Addressables.InitializeAsync(true);
    }
    /// <summary> 카탈로그 업데이트 및 다운로드 사이즈 체크 </summary>
    public static async UniTask<long> CheckDownload(List<stDownloadData> listDownload)
    {
        #if UNITY_EDITOR
        return 0;
        #endif
        var checkCatalog = Addressables.CheckForCatalogUpdates();
        await checkCatalog;

        if (checkCatalog.Status == AsyncOperationStatus.Succeeded)
        {
            var updateInfos = checkCatalog.Result;
            if (updateInfos.Count > 0)
            {
                await Addressables.UpdateCatalogs(autoCleanBundleCache: true, updateInfos);
            }
        }
        return await _CheckDownloadSize(listDownload);
    }

    private static async UniTask<long> _CheckDownloadSize(List<stDownloadData> listDownload)
    {
        var labelNode = await LoadJson<JSONNode>("AssetLabelList", true);
        if (labelNode == null || labelNode.Count == 0)
            throw new NullReferenceException();
        
        var labelList = labelNode[0].AsArray;
        long downloadSize = 0;
        for (int i = 0; i < labelList.Count; i++)
        {
            var label = (string)labelList[i];
            var handle = Addressables.GetDownloadSizeAsync(label);
            var size = await handle;

            downloadSize += size;
            listDownload.Add(new stDownloadData(){Label = label, Size = size});
            
            Addressables.Release(handle);
        }

        return downloadSize;
    }

    public static async UniTask StartDownload(long downloadSize, List<stDownloadData> listDownload)
    {
        if (downloadSize == 0)
            return;

        await _Downloading(downloadSize, listDownload);
    }

    private static async UniTask _Downloading(long totalSize, List<stDownloadData> listDownload)
    {
        long downloadSize = 0;
        int nPercent = 0;
        foreach (var downloadData in listDownload)
        {
            var downloadDependenciesAsync = Addressables.DownloadDependenciesAsync(downloadData.Label, true);
            while (downloadDependenciesAsync.IsDone == false)
            {   //다운로드 진행도 표기 용 percent
                var percent = downloadDependenciesAsync.PercentComplete;
                nPercent = Convert.ToInt32((downloadSize + downloadData.Size * percent) / totalSize);
                nPercent *= 100;
                
                await UniTask.Yield();
            }

            downloadSize += downloadData.Size;
        }
    }
    public static async UniTask<T> Load<T>(string key, bool isRelease) where T : Object
    {
        var result = await LoadAssetAsync<T>(key, isRelease);
        return result;
    }

    public static T LoadImmediately<T>(string key, bool isRelease) where T : Object
    {
        var result = LoadAssetImmediately<T>(key, isRelease);
        return result;
    }

    public static async UniTask<AssetList> LoadAssetList(string key, bool isRelease)
    {
        var textAsset = await Load<TextAsset>(key, isRelease);
        if (textAsset == null)
        {
            DebugManager.LogError($"Not in Addressable ({key})", true);
            return null;
        }

        var text = textAsset.text;
        return JsonUtility.FromJson<AssetList>(text);
    }
    
    public static async UniTask<T> LoadJson<T>(string key, bool isRelease) where T : JSONNode
    {
        var textAsset = await Load<TextAsset>(key, isRelease);
        if (textAsset == null)
        {
            DebugManager.LogError($"Not in Addressable ({key})", true);
            return null;
        }

        var jsonNode = JSON.Parse(textAsset.text);
        if (jsonNode == null)
        {
            DebugManager.LogError($"JSONParsingError: ({key})", true);
            return null;
        }
        if (jsonNode.Tag == JSONNodeType.None)
            return null;
        
        return jsonNode as T;
    }

    public static async UniTask<GameObject> LoadPrefabAsync(string name, Transform parent = null)
    {
        try
        {
            var op = Addressables.InstantiateAsync(name, parent);
            GameObject ob = await op;

            GameObjectUtil.AddComponent<SelfCleanupInstance>(ob);
            return ob;
        }
        catch (InvalidKeyException)
        {
            return null;
        }
    }
    public static GameObject LoadPrefab(string name, Transform parent = null)
    {
        try
        {
            var op = Addressables.InstantiateAsync(name, parent);

            GameObject ob = null;
            if (op.Status == AsyncOperationStatus.Succeeded)
                ob = op.Result;
            else
                ob = op.WaitForCompletion();

            GameObjectUtil.AddComponent<SelfCleanupInstance>(ob);
            return ob;
        }
        catch (InvalidKeyException)
        {
            return null;
        }
    }

    public static async UniTask<SceneInstance> LoadScene(string name, 
        LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
    {
        var result = await LoadSceneAsync(name, loadMode, activateOnLoad, priority);
        return result;
    }

    public static void Release<TObject>(TObject obj)
    {
        Addressables.Release(obj);
    }
}

public class SelfCleanupInstance : MonoBehaviour
{
    private void OnDestroy()
    {
        Addressables.ReleaseInstance(gameObject);
    }
}

public class SelfCleanup : MonoBehaviour
{
    public Sprite sprite;
    private void OnDestroy()
    {
        Addressables.Release(sprite);
    }
}