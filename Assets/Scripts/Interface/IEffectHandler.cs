namespace Interface
{
    public interface IEffectHandler : ITickable
    {
        void Start(LevelEffectBase effect);
        void Finish(LevelEffectBase effect);
    }
}