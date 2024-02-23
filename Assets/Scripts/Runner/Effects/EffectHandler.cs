using System.Collections.Generic;

namespace Interface
{
    public class EffectHandler : IEffectHandler
    {
        private readonly List<LevelEffectBase> _effects = new(5);
        private readonly List<LevelEffectBase> _effectsFinished = new(1);
        
        public void Tick(float dt)
        {
            foreach (var eff in _effects)
            {
                eff.Tick(dt);
            }

            foreach (var rm in _effectsFinished)
            {
                _effects.Remove(rm);
            }
            _effectsFinished.Clear();
        }

        void IEffectHandler.Start(LevelEffectBase effect)
        {
            _effects.Add(effect);
        }

        void IEffectHandler.Finish(LevelEffectBase effect)
        {
            _effectsFinished.Add(effect);
        }
    }
}