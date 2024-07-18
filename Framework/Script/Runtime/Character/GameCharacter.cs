using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCharacter : BaseObject
{
    public string Key { get { return _info.Key; } }
    public ActionComponent ActionComponent { get { return _actionComponent; } }
    public SkillComponent SkillComponent { get { return _skillComponent; } }
    
    private CharacterInfo _info;
    
    //Component
    private ActionComponent _actionComponent;
    private SkillComponent _skillComponent;

    public Vector3 GetForward()
    {
#if GAME_2D
        if (SelfTransform.forward == Vector3.forward)
        {
            return Vector3.right;
        }
        return Vector3.left;
#else
        return transform.forward;
#endif
    }
    public Transform GetBone(string boneName)
    {
        var bone = GameObjectUtil.FindChild(SelfObject, boneName);
        if (bone == null)
            return null;
        
        return bone.transform;
    }
}
