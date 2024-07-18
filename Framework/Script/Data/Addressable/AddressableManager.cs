using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using SimpleJSON;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class AssetList
{
    public List<string> assets = new List<string>();
}
public partial class AddressableManager
{

    public static async UniTask<T> Load<T>(string key) where T : Object
    {
        var result = await LoadAssetAsync<T>(key);
        return result;
    }

    public static async UniTask<AssetList> LoadAssetList(string key)
    {
        var textAsset = await Load<TextAsset>(key);
        if (textAsset == null)
        {
            DebugManager.LogError($"Not in Addressable ({key})", true);
            return null;
        }

        var text = textAsset.text;
        return JsonUtility.FromJson<AssetList>(text);
    }
    
    public static async UniTask<T> LoadJson<T>(string key) where T : JSONNode
    {
        var textAsset = await Load<TextAsset>(key);
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
        var op = Addressables.InstantiateAsync(name, parent);
        GameObject ob = await op;

        GameObjectUtil.AddComponent<SelfCleanupInstance>(ob);
        return ob;
    }
    public static GameObject LoadPrefab(string name, Transform parent = null)
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
    public Sprite _sprite;
    private void OnDestroy()
    {
        Addressables.Release(_sprite);
    }
}