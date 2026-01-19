using System.Collections.Generic;

public class AttributeSetter<T>
{
    public ulong Attr { get { return _attr; } }
    private ulong _attr = 0;
    
    public AttributeSetter() {}

    public AttributeSetter(T attr)
    {
        SetAttr(attr);
    }

    public void Clear()
    {
        _attr = 0;
    }

    public void SetValue(ulong value)
    {
        _attr = value;
    }

    public bool IsAttr(T attr)
    {
        ulong temp = (ulong)System.Convert.ChangeType(attr, typeof(ulong));
        return DataUtil.IsValue(_attr, temp);
    }

    public bool IsAttrAll(T attr)
    {
        ulong temp = (ulong)System.Convert.ChangeType(attr, typeof(ulong));
        return DataUtil.IsValueAll(_attr, temp);
    }

    public void SetAttr(T attr, bool isSet = true)
    {
        if (isSet)
        {
            ulong temp = (ulong)System.Convert.ChangeType(attr, typeof(ulong));
            _attr |= temp;
        }
        else
        {
            RemoveAttr(attr);
        }
    }

    public void RemoveAttr(T attr)
    {
        ulong temp = (ulong)System.Convert.ChangeType(attr, typeof(ulong));
        _attr &= ~temp;
    }
}
