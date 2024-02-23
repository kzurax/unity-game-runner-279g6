using UnityEngine;

namespace Interface
{
    public interface ICollideHandler
    {
        void OnCollide(GameObject who, Prop element);
        // void OnCollisionEnd(GameObject who, Prop element);
        
        void OnTrigger(GameObject who, Prop element);
        // void OnTriggerEnd(GameObject who, Prop element);
    }
}