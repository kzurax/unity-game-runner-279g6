using System;
using Interface;
using UnityEngine;

namespace Runner
{
    public class InputKeyboard : IInput
    {
        public event Action onJump;
        public event Action<int> onTurnSide;
    
        public void Handle()
        {
            // Player Controls
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                onTurnSide.Invoke(-1);
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                onTurnSide.Invoke(1);
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow) || 
                     Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))
            {
                onJump.Invoke();
            }
        }
    }
}