using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public partial class AddressableManager
{
    public static async UniTask<T> LoadAssetAsync<T>(string name) where T : Object
    {
        var operation = Addressables.LoadAssetAsync<T>(name);
        await operation;
        return operation.Result;
    }
    
    public static async UniTask<SceneInstance> LoadSceneAsync(string name, 
        LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
    {
        var operation = Addressables.LoadSceneAsync(name, loadMode, activateOnLoad, priority);
        await operation;
        return operation.Result;
    }
}
