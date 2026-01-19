using Cysharp.Threading.Tasks;
using UnityEngine;

public static class SceneManager
{
    /// <summary> SceneManager 를 사용한 SceneLoad </summary>
    public static void LoadScene(string name)
    {
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(name);
    }
    /// <summary> Addressable 을 이용한 SceneLoad, Addressable Init 이후에 사용할 것 </summary>
    public static async UniTaskVoid LoadSceneAddressable(string name)
    {
        await AddressableManager.LoadScene(name);
    }
}
