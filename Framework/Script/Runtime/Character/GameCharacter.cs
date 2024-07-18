using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameCharacter : BaseObject
{
    public string Key { get { return _info?.Key ?? string.Empty; } }
    public CharacterInfo Info { get { return _info; } }
    
    public AIComponent AIComponent { get { return _aiComponent; } }
    public ActionComponent ActionComponent { get { return _actionComponent; } }
    public SkillComponent SkillComponent { get { return _skillComponent; } }
    public TargetComponent TargetComponent { get { return _targetComponent; } }
    public RotateComponent RotateComponent { get { return _rotateComponent; } }
    public MoveComponent MoveComponent { get { return _moveComponent; } }
    
    public Vector3 position { get { return SelfTransform.position; } set { SelfTransform.position = value; } }
    public Quaternion rotation { get { return SelfTransform.rotation; } set { SelfTransform.rotation = value; } }
    
    private CharacterInfo _info;
    
    //Component
    private AIComponent _aiComponent;
    private ActionComponent _actionComponent;
    private SkillComponent _skillComponent;
    private TargetComponent _targetComponent;
    private RotateComponent _rotateComponent;
    private MoveComponent _moveComponent;

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
