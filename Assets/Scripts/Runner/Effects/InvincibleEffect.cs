namespace Interface
{
    public class InvincibleEffect : LevelEffectBase
    {
        public InvincibleEffect(LevelDirector lvl, float dur = 0) : base(lvl, dur)
        {
        }
        
        protected override void OnStart() => level.Player.Invincible = true;
        protected override void OnFinish() => level.Player.Invincible = false;
    }
}