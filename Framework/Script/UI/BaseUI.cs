using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public abstract class BaseUI : BaseObject
{
    #region Binding
    private Dictionary<Type, Object[]> _dicComponent = new Dictionary<Type, Object[]>();

    /// <summary> Enum에 등록된 UI 컴포넌트 바인딩 </summary>
    /// <param name="type"> Enum 타입 </param>
    /// <typeparam name="T"> 컴포넌트 타입 </typeparam>
    protected void Bind<T>(Type type) where T : Object
    {
        var names = Enum.GetNames(type);
        var components = new Object[names.Length];
        _dicComponent[typeof(T)] = components;

        for (int i = 0; i < names.Length; i++)
        {
            if (typeof(T) == typeof(GameObject))
                components[i] = GameObjectUtil.FindChild(gameObject, names[i]);
            else
                components[i] = GameObjectUtil.FindChild<T>(gameObject, names[i]);

            if (components[i] == null)
            {
                DebugManager.LogError($"Failed To Bind ({names[i]})");
                //Open Error Box
            }
        }
    }

    protected T Get<T>(int index) where T : Object
    {
        var type = typeof(T);
        
        if (_dicComponent.ContainsKey(type) == false)
            return null;
        if (index >= _dicComponent[type].Length)
            return null;
        
        return _dicComponent[type][index] as T;
    }

    protected GameObject GetObject(int index) { return Get<GameObject>(index); }
    protected Text GetText(int index) { return Get<Text>(index); }
    protected UI_Button GetButton(int index) { return Get<UI_Button>(index); }
    protected Image GetImage(int index) { return Get<Image>(index); }

    #endregion
    
    public GlobalEnum.ePriorityType PriorityType { get; set; }
    public bool IsRootUI { get; set; } = false;

    /// <summary> UI 요소 초기화 </summary>
    public abstract void Init();
    /// <summary> 뒤로가기 눌렀을 때 처리 </summary>
    public abstract void OnBack();
}
