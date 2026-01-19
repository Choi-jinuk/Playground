using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateComponent : BaseComponent
{
    public void RotationToTarget(GameCharacter target)
    {
        Vector3 direction = target.position - Character.position;
        RotationToTarget(direction);
    }
    public void RotationToTarget(Vector3 direction)
    {
        if (Character.Info?.IsDead ?? true)
            return;

#if GAME_2D
        float dot = Vector3.Dot(direction, Vector3.right);
        Character.SelfTransform.forward = dot > 0f ? Vector3.forward : Vector3.back;
#else
        Quaternion destRotation = new Quaternion();
        destRotation.SetLookRotation(direction);
        Character.rotation = Quaternion.Slerp(Character.rotation, destRotation, 0.1f);   
#endif
    }
}
