

using System;
using Interface;
using UnityEngine;

namespace Runner
{
    public class Character : ITickable, ICollideHandler
    {
        public CharacterView view {get; private set; }
        public CharControl control {get; private set; }
        public Movable move {get; private set; }
        public Road road {get; private set; }


        public event Action OnDie;
        public event Action<CoinBase> OnCatch;

        private bool _onGround = false;
        private bool _invincible = false;
        private bool _flying = false;

        public bool OnGround
        {
            get => _onGround;
            private set
            {
                _onGround = value;
                view?.SetJumping(!_onGround || _flying);
            }
        }
        
        public bool Invincible
        {
            get => _invincible;
            set
            {
                _invincible = value;
                
                // Animate
            }
        }
        
        public bool Fly
        {
            get => _flying;
            set
            {
                _flying = value;
                control.Fly = _flying;
                OnGround = OnGround;

                // Animate
            }
        }
        
        

        public Character(CharacterView view, CharControl control, Movable move, Road road)
        {
            this.view = view;
            this.control = control;
            this.move = move;
            this.road = road;

            if (view && view.GetCollider() is { } collider)
            {
                collider.AddHandler(this);
                OnGround = true;
            }
        }

        public void StartRunning(float speed)
        {
            control.Reset();
            move.SetVelocity(Vector3.forward * speed);
            
            view.SetRunning(true);
            OnGround = true;
        }
        
        public void Die()
        {
            move.SetVelocity(Vector3.zero, 0);
            view.SetRunning(false);
            OnGround = true;
            Fly = false;
        }

        public void Tick(float dt)
        {
            control.Tick(dt);
            move.Tick(dt); 
            
            
            road.OnReachDistance(move.Position.z);
        
            if (view)
            {
                road.GetPointAt(move.Position.z, out var pos, out var rot);
                view.transform.SetPositionAndRotation(pos + rot * control.Position, rot);

                if (!OnGround && control.JumpProgress >= 0.95f)
                {
                    OnGround = true;
                }
            }
        }

        public void SwitchLine(int direction) => control.SwitchLine(direction);

        public void TriggerJump()
        {
            if (control.TriggerJump())
            {
                OnGround = false;
            }
        }
        
        // ICollideHandler // // // // 

        public void OnCollide(GameObject who, Prop element)
        {
        }

        public void OnTrigger(GameObject who, Prop element)
        {
            if (element is CoinBase coin)
            {
                coin.OnCollect();
                OnCatch?.Invoke(coin);
            }
            else
            {
                if (!_invincible)
                {
                    // Now we just track events of meet any obstacle here, what means we loose
                    OnDie?.Invoke();
                }
            }
        }
    }
}