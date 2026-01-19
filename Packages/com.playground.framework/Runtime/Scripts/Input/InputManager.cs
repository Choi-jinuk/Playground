using System;
using UnityEngine;

public class InputManager : MonoSingleton<InputManager>
{
    private Callback _escapeAction;

    public bool BlockEscapeAction { get; set; } = false;
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            if (BlockEscapeAction)
                return;
            
            _escapeAction?.Invoke();
        }
    }

    public void AddEscapeAction(Callback cb)
    {
        _escapeAction -= cb;
        _escapeAction += cb;
    }
}
