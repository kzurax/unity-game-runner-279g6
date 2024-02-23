using System;
using System.Collections.Generic;
using Interface;
using UnityEngine;

namespace Mono
{
    [RequireComponent(typeof(Collider))]
    public class CollisionCatcher : MonoBehaviour
    {
        private List<ICollideHandler> _handlers = new(1);

        
        public void AddHandler (ICollideHandler h) => _handlers.Add(h);
        public void RemoveHandler(ICollideHandler h)=> _handlers.Remove(h);
        
        private void OnCollisionEnter(Collision other)
        {
            // Debug.Log($"On Collision {other.gameObject.name}");
            
            if (GetPropOwner(other.collider) is {} prop)
            {
                foreach (var h in _handlers)
                {
                    h.OnCollide(gameObject, prop);
                }
            }
            else
            {
                // ignore
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            // Debug.Log($"On TriggerEnter {other.gameObject.name}");
            
            if (GetPropOwner(other) is {} prop)
            {
                foreach (var h in _handlers)
                {
                    h.OnTrigger(gameObject, prop);
                }
            }
            else
            {
                // ignore
            }
        }

        private Prop GetPropOwner(Collider col) => 
            col.gameObject.GetComponent<Prop>() ?? 
            col.gameObject.transform.parent.GetComponent<Prop>();
    }
}