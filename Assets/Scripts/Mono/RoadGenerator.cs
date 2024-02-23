using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private RoadPartBase[] possibleOptions;
    [SerializeField] private int preinstanceCount = 3;
    [SerializeField] private Transform holder;
    
    [SerializeField] private RoadDecorator decorator;

    private readonly List<RoadPartBase> _available = new(); 
    private readonly List<RoadPartBase> _active = new();

    private LinesInfo _linesInfo;


    public void Setup(LinesInfo lines)
    {
        _linesInfo = lines;
        decorator.Setup(_linesInfo);
        
        holder ??= transform;
        _available.Capacity = preinstanceCount * possibleOptions.Length;
        for (var i = 0; i < preinstanceCount; i++)
        {
            PreInstantiateOnce();
        }
    }
    
    
    public RoadPartBase GetRoadPart()
    {
        if (_available.Count < 1)
        {
            PreInstantiateOnce();
        }

        var part = Activate(_available[Random.Range(0, _available.Count)], true);
        decorator.SetupProps(part);
        
        return part;
    }

    public void Utilise(RoadPartBase obj)
    {
        obj.Clear();
        Activate(obj, false);
    }
    
    
    
    private void PreInstantiateOnce()
    {
        foreach (var opt in possibleOptions)
        {
            var pooled = Instantiate(opt.gameObject, holder).GetComponent<RoadPartBase>();
            pooled.gameObject.SetActive(false);
            _available.Add(pooled);
        }
    }


    private RoadPartBase Activate(RoadPartBase obj, bool act)
    {
        if (act)
        {
            _available.Remove(obj);
            _active.Add(obj);
        }
        else
        {
            _active.Remove(obj);
            _available.Add(obj);
        }
        obj.gameObject.SetActive(act);
        return obj;
    }
}