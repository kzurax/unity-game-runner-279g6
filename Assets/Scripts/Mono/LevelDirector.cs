using System;
using System.Collections.Generic;
using Interface;
using Mono;
using Runner;
using UnityEngine;

public class LevelDirector : MonoBehaviour
{
    [Serializable]
    public class LevelConfig
    {
        public float CharacterSpeed = 20;
        public float InvincibleInSec = 2;
        public Vector2 SaveDistBackFront = new(10, 50);
        public LinesInfo Lines;

        public float EffBaseDuration = 8f;
        public float EffectSpeedSclStep = 0.3f;
    }
    
    [Serializable]
    public class LevelData
    {
        public bool gameActive = false;
        public int coins = 0;

        public float SpeedScale = 1;
        public EffectHandler effects = new();
    }
    
    [SerializeField] private CharControl.Config _characterCfg;
    [SerializeField] private LevelConfig _levelCfg;
    [SerializeField] private RoadGenerator _roadGenerator;
   
    [SerializeField] private CharacterView _plView;
    [SerializeField] private Hud _ui;
    [SerializeField] private float _recenterThreshold = 1000f;
    
    private Character _player;
    private IInput _input;
    
    private Road _road;
    private readonly List<Movable> _movables = new();

    private LevelData _data = new();
    
    public Character Player => _player;

    public void InitLevel()
    {
        // Instance Road
        _roadGenerator.Setup(_levelCfg.Lines);
        _road = new Road(_roadGenerator, _levelCfg.SaveDistBackFront);
        _road.Refresh();

        
        // Init player
        _player = new Character(_plView, 
            new CharControl(_characterCfg, _levelCfg.Lines), 
            new Movable(), 
            _road);
        _player.OnDie += PlayerOnDie;
        _player.OnCatch += PlayerOnCatch;
         
        
        // Init input
#if UNITY_EDITOR
        _input = new InputKeyboard();
#else
        _input = new InputSwipe();
#endif
        _input.onTurnSide += _player.SwitchLine;
        _input.onJump += _player.TriggerJump;
        
        // Hud
        _data = new();
        _ui.onRestartClick += OnRestart;
        
        // Start
        StartGame();
    }

    public void ScaleSpeed(float scl)
    {
        _data.SpeedScale *= scl;
        var spd = _levelCfg.CharacterSpeed * _data.SpeedScale; 
        _player.move.SetVelocity(new Vector3(0,0, spd));
    }

    private void StartGame()
    {
        _ui.Coins = _data.coins;
        _ui.GameFail = false;
        _data.gameActive = true;
        _player.StartRunning(_levelCfg.CharacterSpeed);
        ActivateInvincible(_levelCfg.InvincibleInSec);
    }

    private void PlayerOnCatch(CoinBase obj) => EffectFromCoin(obj)?.ActivateHandler(_data.effects);

    private LevelEffectBase EffectFromCoin(CoinBase c)
    {
        switch (c.effect)
        {
            case CoinBase.CoinEffect.Coin:
                _data.coins += c.count;
                _ui.Coins = _data.coins;
                break;
            case CoinBase.CoinEffect.SlowDown:
                return new SpeedEffect(
                    1f - _levelCfg.EffectSpeedSclStep * c.count,
                    this, _levelCfg.EffBaseDuration);
            case CoinBase.CoinEffect.SpeedUp:
                return new SpeedEffect(
                    1f + _levelCfg.EffectSpeedSclStep * c.count,
                    this, _levelCfg.EffBaseDuration);
            case CoinBase.CoinEffect.Fly:
                return new FlyEffect(this, _levelCfg.EffBaseDuration / 2f);
        }

        return null;
    }

    private void PlayerOnDie()
    {
        _data.gameActive = false;
        _ui.GameFail = true;
        
        _player.Die();
        // Debug.Log("<size=40><color=red>Die (x _ x) </color></size>");
    }

    private void OnRestart()
    {
        if (_data.gameActive)
        {
            return;
        }
        
        _data = new();
        StartGame();
    }

    
    private void ActivateInvincible(float sec)
    {
        if (sec > 0)
        {
            new InvincibleEffect(this, sec).ActivateHandler(_data.effects);
        }
    }
    

    private void CheckRecenter()
    {
        // Avoid floating point quiality loose on big distances 
        if (_player.move.Position.z > _recenterThreshold)
        {
            _road.Recenter(_recenterThreshold);

            var decraceDist =  new Vector3(0, 0, _recenterThreshold);
            _player.move.Position -= decraceDist;
            foreach (var mv in  _movables)
            {
                mv.Position -= decraceDist;
            }
        }
    }
    

    private void Start() => InitLevel();

    void Update()
    {
        if (!_data.gameActive )
        {
            return;
        }

        CheckRecenter();
        
        var dt = Time.deltaTime;
        _data.effects.Tick(dt);
        _input.Handle();
        _player.Tick(dt);
    }
}
