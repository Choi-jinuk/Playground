using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalUtil
{
    private static GameObject _effectObjectRoot = null;
    public static GameObject EffectObjectRoot
    {
        get
        {
            if (_effectObjectRoot == null)
            {
                _effectObjectRoot = new GameObject(GlobalConst.EFFECT_OBJECT_ROOT);
            }
            return _effectObjectRoot;
        }
    }
}
