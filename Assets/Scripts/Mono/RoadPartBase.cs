using System.Collections.Generic;
using Dreamteck.Splines;
using UnityEngine;

public class RoadPartBase : MonoBehaviour
{
    public Transform _startPos;
    public Transform _endPos;
    // public SplineComputer _spline;

    public Transform _container;

    private List<Prop> _props = new(); 

    private float _lenght = -1f;
    
    
    
    
    public virtual Vector3 GetPathPosition(float distance)
    {
        var pos = Vector3.LerpUnclamped(
            _startPos.localPosition, 
            _endPos.localPosition, distance / Lenght);

        return transform.TransformPoint(pos);
    }

    public virtual  Quaternion GetPathOrientation(float distance)
    {
        var rot = Quaternion.LerpUnclamped(
            _startPos.rotation, 
            _endPos.rotation, distance / Lenght);
        return rot;
    }

    public virtual float Lenght => _lenght < 0f ? CalcLenght() : _lenght;

    private float CalcLenght() => 
        _lenght = (_endPos.localPosition - _startPos.localPosition).magnitude;

    
    
    
    public Prop AppendProp(Prop prop)
    {
        prop.transform.SetParent(_container, false);
        _props.Add(prop);

        return prop;
    }
    
    public void Clear()
    {
        foreach (var p in _props)
        {
            p.ReturnToPool();
        }
        _props.Clear();
    }
    
}
