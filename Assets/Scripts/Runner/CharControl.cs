using System;
using UnityEngine;

public class CharControl : ITickable 
{
    [Serializable]
    public class Config
    {
        public Transform body;
        public float lineDump = 20;

        public AnimationCurve jumpCurve;
        public float jumpHeight;
        public float jumpDuration;
    }
    
    private Config _cfg;
    public  LinesInfo _lines;
    
    private int   _line = 0;
    private Vector3 _lineChangeSpd = Vector3.zero;
    
    private Vector3 _groundPos = Vector3.zero;
    private float _jumpBegin = -100f;
    
    private bool _isFlying = false;
    
    
    

    // Public 
    
    public Vector3 Position => _groundPos + Vector3.up * JumpHg;


    private CharControl() { }
    public CharControl(Config config, LinesInfo lines)
    {
        _cfg = config;
        _lines = lines;
        _line = _lines.LineDefault;
    }
    
    public bool Fly
    {
        get => _isFlying;
        set => _isFlying = value;
    }

    
    public bool TriggerJump()
    {
        if (JumpProgress >= 1f && _isFlying == false)
        {
            _jumpBegin = Time.time;
            return true;
        }

        return false;
    }

    public void SwitchLine(int dir)
    {
        Line = Mathf.Clamp(Line + dir, -_lines.Min, _lines.Max);
    }


    public void Reset()
    {
        Line = _lines.LineDefault;
    }
    
    public void Tick(float dt) 
    {
        // Move to line 
        _groundPos =
            Vector3.SmoothDamp(_groundPos, GroundPos + _lines.GetLineOffset(_line),
                ref _lineChangeSpd, _cfg.lineDump * dt);
        
        if (_cfg.body)
        {
            _cfg.body.localPosition = Position;
        }
    }

    
    // Private
    private int Line
    {
        get => _line;
        set => _line = value;
    }


    public float JumpProgress => Mathf.Clamp01((Time.time - _jumpBegin) / _cfg.jumpDuration);
    private float JumpHg
    {
        get
        {
            var progress = JumpProgress;
            return progress < 1f ? _cfg.jumpCurve.Evaluate(progress) * _cfg.jumpHeight : 0;
        }
    }

    private Vector3 GroundPos => new(0, _isFlying.GetHashCode() * _cfg.jumpHeight, 0);
}
