namespace Interface
{
    public class FlyEffect : LevelEffectBase
    {
        public FlyEffect(LevelDirector lvl, float dur = 0) : base(lvl, dur)
        {
        }
        
        protected override void OnStart()
        {
            level.Player.Fly = true;
        }

        protected override void OnFinish()
        {
            level.Player.Fly = false;
        }
    }
}