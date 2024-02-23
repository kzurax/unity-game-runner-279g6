namespace Interface
{
    public class SpeedEffect : LevelEffectBase
    {
        public float speedScale = 1f;
        
        public SpeedEffect(float scale, LevelDirector lvl, float dur = 0f) : base(lvl, dur)
        {
            speedScale = scale;
        }
        
        protected override void OnStart()
        {
            level.ScaleSpeed(speedScale);
        }

        protected override void OnFinish()
        {
            level.ScaleSpeed(1f/speedScale);
        }
    }
}