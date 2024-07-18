using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInfo : MonoBehaviour
{
    public int Index { get { return _index; } }
    public string Key { get { return _infoParam.Key; } }
    public SkillTemplateData Data { get { return _data; } }
    public int Level { get { return _infoParam.Level; } }
    public bool IsLock { get { return _isLock; } }
    public bool IsAuto { get { return _isAuto; } }

    private stSkillInfoParam _infoParam;
    private SkillTemplateData _data;
    private int _index;
    private bool _isLock = true;
    private bool _isAuto = true;

}
