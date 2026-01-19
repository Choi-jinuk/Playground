using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

public partial class AddressableManager
{
    private static async UniTask<T> LoadAssetAsync<T>(string name, bool isRelease) where T : Object
    {
        try
        {
            var operation = Addressables.LoadAssetAsync<T>(name);
            await operation;

            var result = operation.Result;
            if (isRelease)
                Addressables.Release(operation);
            
            return result;
        }
        catch (InvalidKeyException)
        {
            return null;
        }
    }

    private static T LoadAssetImmediately<T>(string name, bool isRelease) where T : Object
    {
        try
        {
            var operation = Addressables.LoadAssetAsync<T>(name);

            T result;
            if (operation.Status == AsyncOperationStatus.Succeeded)
                result = operation.Result;
            else
                result = operation.WaitForCompletion();

            if (isRelease)
                Addressables.Release(operation);
            
            return result;
        }
        catch (InvalidKeyException)
        {
            return null;
        }
    }
    
    private static async UniTask<SceneInstance> LoadSceneAsync(string name, 
        LoadSceneMode loadMode = LoadSceneMode.Single, bool activateOnLoad = true, int priority = 100)
    {
        try
        {
            var operation = Addressables.LoadSceneAsync(name, loadMode, activateOnLoad, priority);
            await operation;
            
            return operation.Result;
        }
        catch (InvalidKeyException)
        {
            return default;
        }
    }
}
