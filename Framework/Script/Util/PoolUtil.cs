using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IPoolComponent
{
    /// <summary> 풀링 오브젝트 가져올 때 데이터 세팅 처리 </summary>
    public void SetData();
    /// <summary> 풀링 오브젝트 Release 할 때 처리 </summary>
    public void ReleaseData();
    /// <summary> 풀링 오브젝트 Destroy 전에 연결된 데이터 삭제 </summary>
    public void ClearData();
}

public class PoolUtil<T> : IPoolDebug where T : IPoolComponent, new()
{
    private List<T> _totalList;
    private Stack<T> _enableList;
    private List<T> _usingList;

    public void Init(int size)
    {
        if (size == 0)
        {
            return;
        }

        _totalList = new List<T>(size);
        _enableList = new Stack<T>(size);
        _usingList = new List<T>(size);
        
#if UNITY_EDITOR
        PoolDebugManager.AddPool(this);
#endif
    }

    public T New()
    {
        if (_totalList == null)
        {
            DebugManager.LogError("Not Initialize Pool");
            return default(T);
        }

        if (_enableList.Count == 0)
        {
            var item = new T();
            _totalList.Add(item);
            _usingList.Add(item);
            item.SetData();
            return item;
        }
        else
        {
            var item = _enableList.Pop();
            _usingList.Add(item);
            item.SetData();
            return item;
        }
    }

    public void Remove(T item)
    {
        if (_CheckEqualCount())
        {
            if (_usingList.Remove(item))
            {
                item.ReleaseData();
                _enableList.Push(item);
            }
            else
            {
                DebugManager.LogError($"{item} Pool Remove Error");
            }
        }
        else
        {
            DebugManager.LogError($"{item} is Not Matching Pool Count");
        }
    }

    public void RemoveAll()
    {
        for (int i = _usingList.Count - 1; i >= 0; --i)
        {
            T item = _usingList[i];
            _enableList.Push(item);
            item.ReleaseData();
            _usingList.Remove(item);
        }
    }

    public void Clear()
    {
        for (int i = _totalList.Count - 1; i >= 0; --i)
        {
            _totalList[i].ClearData();
        }

        _totalList.Clear();
        _enableList.Clear();
        _usingList.Clear();
    }

    private bool _CheckEqualCount()
    {
        return _totalList.Count == _enableList.Count + _usingList.Count;
    }


    //Debug
    public string DebugName()
    {
        return typeof(T).ToString();
    }
    public string DebugCount()
    {
        return $" ({_usingList.Count}/{_totalList.Count})";
    }
}

public class ObjectPoolUtil<T> : IPoolDebug where T : UnityEngine.Component, IPoolComponent
{
    public bool IsInit { get { return _original != null; } }
    public int Size { get { return _totalList.Count; } }
    public string Name { get { return IsInit ? _original.name : string.Empty; } }
    
    private GameObject _original;
    private List<T> _totalList;
    private Stack<T> _enableList;
    private List<T> _usingList;

    private bool _isCleanup = false;

    public async UniTask Init(Transform parent, string prefab, int size)
    {
        _isCleanup = true;
        var go = await AddressableManager.LoadPrefabAsync(prefab, parent);
        go.transform.SetParent(parent, false);
        go.transform.position = Vector3.zero;
        Init(go, size);
    }

    public async UniTask InitUI(Transform parent, string prefab, int size)
    {
        _isCleanup = true;
        var go = await AddressableManager.LoadPrefabAsync(prefab, parent);
        go.transform.SetParent(parent, false);
        var rt = go.GetComponent<RectTransform>();
        rt.position = Vector3.zero;
        Init(go, size);
    }

    public void Init(GameObject original, int size)
    {
        if (size == 0)
        {
            return;
        }

        _original = original;
        _original.SetActive(false);
        _totalList = new List<T>(size);
        _enableList = new Stack<T>(size);
        _usingList = new List<T>(size);
        
#if UNITY_EDITOR
        PoolDebugManager.AddPool(this);
#endif
    }

    public async UniTask<T> New()
    {
        if (IsInit == false)
        {
            DebugManager.LogError("Not Initialize Pool");
            return null;
        }

        T item = _enableList.Count == 0
            ? await _CreateAndAdd()
            : _enableList.Pop();

        item.name = $"{_original.name}{_usingList.Count}";
        _usingList.Add(item);
        item.gameObject.SetActive(true);
        item.SetData();
        
        if (_CheckEqualCount() == false)
        {
            DebugManager.LogError($"{_original.name} is Not Matching Pool Count");
        }

        return item;
    }

    private async UniTask<T> _CreateAndAdd()
    {
        GameObject go = _isCleanup
            ? await AddressableManager.LoadPrefabAsync(_original.name, _original.transform.parent)
            : Object.Instantiate(_original, _original.transform.parent);

        T item = GameObjectUtil.AddComponent<T>(go);
        _totalList.Add(item);
        return item;
    }

    public void Remove(T item)
    {
        if (Contains(item) == false)
        {
            DebugManager.LogError($"Not Have item {item.name} in UsingList while Remove");
            return;
        }
        
        if (_CheckEqualCount())
        {
            if (_usingList.Remove(item))
            {
                _enableList.Push(item);
                item.ReleaseData();
                item.gameObject.SetActive(false);
                _usingList.Remove(item);
            }
            else
            {
                DebugManager.LogError($"{item.name} Pool Remove Error");
            }
        }
        else
        {
            DebugManager.LogError($"{_original.name} is Not Matching Pool Count");
        }
    }

    public void RemoveAll()
    {
        for (int i = _usingList.Count - 1; i >= 0; i--)
        {
            T item = _usingList[i];
            _enableList.Push(item);
            item.gameObject.SetActive(false);
            item.ReleaseData();
            _usingList.Remove(item);
        }
    }

    public void Clear()
    {
        if (IsInit == false)
            return;
        
        foreach (var item in _totalList)
        {
            if (item.gameObject != null)
            {
                item.ClearData();
                GameObjectUtil.Destroy(item.gameObject);
            }
        }
        
        _totalList.Clear();
        _enableList.Clear();
        _usingList.Clear();
    }

    private bool _CheckEqualCount()
    {
        return _totalList.Count == _enableList.Count + _usingList.Count;
    }

    public bool Contains(T pool)
    {
        return _usingList.Contains(pool);
    }
    
    
    //Debug
    public string DebugName()
    {
        return typeof(T).ToString();
    }
    public string DebugCount()
    {
        return $" ({_usingList.Count}/{_totalList.Count})";
    }
}
