using UnityEngine;

namespace Interface
{
    public abstract class LevelEffectBase : ITickable
    {
        protected LevelDirector level;
        protected IEffectHandler handler;
        private LevelEffectBase () { }
        protected bool Active = false;

        public float duration { get; set; }

        public LevelEffectBase(LevelDirector lvl, float dur = 0f)
        {
            level = lvl;
            duration = dur;
        }

        public void ActivateHandler(IEffectHandler handler)
        {
            this.handler = handler;
            this.handler?.Start(this);
        }

        public virtual void Tick(float t)
        {
            if (duration > 0f)
            {
                if (!Active)
                {
                    OnStart();
                    Active = true;
                }

                duration = Mathf.Max(0f, duration - t);
            }
            
            if (Active && duration <= 0f)
            {
                OnFinish();
                Active = false;
                handler?.Finish(this);
            }
        }
        protected abstract void OnStart();
        protected abstract void OnFinish();
    }
}