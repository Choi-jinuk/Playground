using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObject : MonoBehaviour
{
    protected GameObject _gameObject;
    protected Transform _transform;

    protected bool _getGameObject = false;
    protected bool _getTransform = false;

    public virtual GameObject SelfObject
    {
        get
        {
            if (_gameObject == null && _getGameObject == false)
            {
                _gameObject = gameObject;
                _getGameObject = true;
            }
            return _gameObject;
        }
    }

    public virtual Transform SelfTransform
    {
        get
        {
            if (_transform == null && _getTransform == false)
            {
                _transform = transform;
                _getTransform = true;
            }

            return _transform;
        }
    }
}
