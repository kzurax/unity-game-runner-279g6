using System;

namespace Interface
{
    public interface IInput
    {
        void Handle();
        
        event Action onJump;
        event Action<int> onTurnSide;
    }
}