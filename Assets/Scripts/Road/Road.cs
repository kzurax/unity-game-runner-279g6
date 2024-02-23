using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Road
{
    private readonly float _safeDistInFront = 50f;
    private readonly float _safeDistBack = 10f;
    private readonly RoadGenerator _generator; 

    private int _startIndex = 0;
    private List<RoadPartBase> _activeParts = new(5);
    // private List<RoadPart> _removing;

    private float _pastDistance = 0f;
    private float _reachedDist = 0f;
    private float _refreshSafeDistanceOn = 0f;

    private Road() { }
    public Road(RoadGenerator roadGenerator, Vector2 safeDistance = new ()) : base()
    {
        _generator = roadGenerator;

        if (safeDistance != Vector2.zero)
        {
            _safeDistBack = safeDistance.x;
            _safeDistInFront = safeDistance.y;
        }
    }

    public void OnReachDistance(float dist)
    {
        _reachedDist = dist;
        if (_reachedDist > _refreshSafeDistanceOn)
        {
            OnPartsUpdateRequired(dist);
        }
    }

    public void GetPointAt(float distance, out Vector3 pos, out Quaternion rot)
    {
        const int maxExtraElements = 1;
        var passedDist = _pastDistance;
        var extras = 0;
        
        for (var i = 0; i < _activeParts.Count; i++)
        {
            // _activeParts[i].transform.localScale = Vector3.one;
            if (distance - passedDist < _activeParts[i].Lenght)
            {
                var currRoad = _activeParts[i];
                var loc = distance - passedDist;
                pos = currRoad.GetPathPosition(loc);
                rot = currRoad.GetPathOrientation(loc);
                
                // _activeParts[i].transform.localScale = Vector3.one * 0.98f;
                return;
            }

            passedDist += _activeParts[i].Lenght;
                
            if (i >= _activeParts.Count - 1 && extras < maxExtraElements)
            {
                // GenerateNextPart();
                extras++;
            }
        }
        
        if (_activeParts.Count == 0)
        {
            pos = Vector3.zero;
            rot = Quaternion.identity;
        }
        else
        {
            var currRoad = _activeParts.Last();
            pos = currRoad.GetPathPosition(distance);
            rot = currRoad.GetPathOrientation(distance);
        }
    }

    public void Recenter(float forgeDistance)
    {
        _pastDistance -= forgeDistance;
        _reachedDist -= forgeDistance;
        _refreshSafeDistanceOn -= forgeDistance;
        
        if (_activeParts.Count == 0)
        {
            return;
        }
        var offset = _activeParts[0].transform.localPosition;
        foreach (var pt in _activeParts)
        {
            pt.transform.localPosition -= offset;
        }
    }

    private void OnPartsUpdateRequired(float distReached)
    {
        var dist = distReached - _pastDistance;
        while (dist > _activeParts[0].Lenght + _safeDistBack)
        {
            _startIndex++;
            var remove = _activeParts[0];
            dist -= remove.Lenght;
            _pastDistance += remove.Lenght;
            _activeParts.RemoveAt(0);
            _generator.Utilise(remove);
        }
        
        Refresh();
    }

    public void Refresh()
    {
        var activeLenght = _activeParts.Sum(p => p.Lenght);
        activeLenght -= _reachedDist - _pastDistance;
        while (activeLenght < _safeDistInFront)
        {
            var newPart = GenerateNextPart();
            activeLenght += newPart.Lenght;
        }

        _refreshSafeDistanceOn =
            Mathf.Min(
                _pastDistance + _activeParts[0].Lenght + _safeDistBack,
                _pastDistance + activeLenght - _safeDistInFront);
    }

    private RoadPartBase GenerateNextPart()
    {
        var newPart = _generator.GetRoadPart();
        PlaceRoadAfterLast(newPart);
        
        _activeParts.Add(newPart);
        return newPart;
    }

    private void PlaceRoadAfterLast(RoadPartBase road)
    {
        var pos = Vector3.zero;
        var rot = Quaternion.identity;
    
        if (_activeParts.Count > 0)
        {
            var last = _activeParts.Last();
            pos = last.GetPathPosition(last.Lenght);
            rot = last.GetPathOrientation(last.Lenght);
        }
        
        road.transform.SetPositionAndRotation(pos, rot);
        
    }
}
