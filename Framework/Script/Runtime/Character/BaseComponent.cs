using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseComponent : BaseObject
{
    public GameCharacter Character
    {
        get
        {
            if (_character == null)
            {
                _character = SelfObject.GetComponent<GameCharacter>();
            }

            return _character;
        }
    }

    private GameCharacter _character = null;
}
