using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataUtil
{
    private static System.Random _random = new System.Random();
    public static bool IsTestMode = false;
    
    public static bool IsAttr<T>(ulong attr, T compare)
    {
        ulong temp = (ulong)System.Convert.ChangeType(compare, typeof(ulong));
        return IsValue(attr, temp);
    }

    public static bool IsValue(ulong dataValue, ulong checkValue)
    {
        return (dataValue & checkValue) > 0;
    }

    public static bool IsValueAll(ulong dataValue, ulong checkValue)
    {
        return (dataValue & checkValue) == checkValue;
    }

    public static int IntRandom(int min, int max)
    {
        if (min >= max)
            return min;
        return _random.Next(min, max);
    }

    public static int CalcSortingOrderByY(float y)
    {
        return Mathf.FloorToInt(5000f + (-y * 10f));
    }

    public static Quaternion GetApplyRotationPos(Vector3 dir, ref Vector3 offset)
    {
        return GetRotationPosAxis(dir, Vector3.up, ref offset);
    }

    public static Quaternion GetRotationPosAxis(Vector3 dir, Vector3 axis, ref Vector3 offset)
    {
        Quaternion rot = Quaternion.identity;
        if (dir != Vector3.right)
        {
            float angle = Vector3.Angle(dir, Vector3.right);
            if (Vector3.Dot(dir, Vector3.up) < 0)
                angle = 360f - angle;

            rot = Quaternion.AngleAxis(angle, axis);
        }

        offset = rot * offset;
        return rot;
    }
}
