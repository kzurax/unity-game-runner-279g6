using UnityEngine;

public class Movable : ITickable    // MonoBehaviour, 
{
    [SerializeField] private Transform _body;
    [SerializeField] private float _acceleration = 5f;
        
    private Vector3 _velocityTg;

    private Vector3 _position;
    private Vector3 _velocity;

    public Vector3 Velocity
    {
        get => _velocity;
        set => _velocity = value;
    }

    public Vector3 Position
    {
        get => _position;
        set => _position = value;
    }
    
    public Vector3 VelocityTarget
    {
        get => _velocityTg;
        set => SetVelocity(value);
    }


    // a < 0 - keep same
    // a = 0 - set speed immediately
    // other set new acceleration param
    public void SetVelocity(Vector3 targetVel, float accel = -1f)
    {
        _velocityTg = targetVel;
        _acceleration = accel > 0f ? accel : _acceleration;

        if (accel == 0)
        {
            _velocity = _velocityTg;
        }
    }
    
    // UNity
    
    // private void Update()
    public void Tick(float dt)
    {
        _position += _velocity * dt;
        _velocity = 
            Vector3.MoveTowards(_velocity, _velocityTg, 
                dt * _acceleration);

        if (_body)
        {
            _body.localPosition = _position;
        }
    }
}