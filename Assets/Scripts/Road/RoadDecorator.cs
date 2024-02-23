using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(menuName = "RoadDecorator")]
public class RoadDecorator : ScriptableObject
{
    [SerializeField] private Prop[] obstaclesList;
    [SerializeField] private Prop[] coinsList;
    
    [SerializeField] private CoinVariant[] coinVariants;
    [SerializeField] private int  defaultPoolCapacity = 5;
    
    [SerializeField] private InstanceParams _defaultCfg;

    private LinesInfo _lines;
    private List<PropsPool> _obstaclePools = new();
    private List<PropsPool> _coinsPools = new();

    [Serializable]
    public class InstanceParams
    {
        public Vector2 fillRate = Vector2.one;  // objs per 10 Meters, +random
        public Vector2 fillRateCoins = Vector2.one;  // per 10 Meters, +random
        public float snapStep = 2f; 
    }

    [Serializable]
    public class CoinVariant
    {
        public float rarity = 1f;   // 1 = default -> 0 - rare
        public CoinBase.CoinEffect effect = CoinBase.CoinEffect.Coin;
        public int count = 1;
    }

    public void Setup( LinesInfo lines )
    {
        _lines = lines;
        
        _obstaclePools = new(obstaclesList.Length);
        foreach (var obst in obstaclesList)
        {
            _obstaclePools.Add(PreparePool(obst, defaultPoolCapacity));
        }
        
        _coinsPools = new(coinsList.Length);
        foreach (var coi in coinsList)
        {
            _coinsPools.Add(PreparePool(coi, defaultPoolCapacity));
        }
    }

    private PropsPool PreparePool(Prop forObj, int capacity = 10)
    {
        var poolGo = new GameObject("_poolFor_" + forObj, typeof(PropsPool));

        var pool = poolGo.GetComponent<PropsPool>();
        pool.Init(forObj, capacity);

        return pool;
    }
    
    public void SetupProps(RoadPartBase road, InstanceParams cfg = null)
    {
        cfg ??= _defaultCfg;

        var obstaclesCnt = (int)(0.1f * road.Lenght * cfg.fillRate.x + Random.Range(0, cfg.fillRate.y));
        var coinsCnt = (int)( 0.1f * road.Lenght * cfg.fillRateCoins.x + Random.Range(0, cfg.fillRateCoins.y));

        DoDistribution(AnyObstacle, ChooseRandomPlace, (int)obstaclesCnt);
        DoDistribution(AnyCoin, ChooseRandomPlace, coinsCnt);

        
        Prop AnyObstacle() => road.AppendProp(_obstaclePools[Random.Range(0, _obstaclePools.Count)].Spawn());
        Prop AnyCoin() => ConfigureCoin(
            road.AppendProp(_coinsPools[Random.Range(0, _coinsPools.Count)].Spawn()) as CoinBase);

        void ChooseRandomPlace(Prop p)
        {
            var roadPos = cfg.snapStep * Random.Range(0, (int) road.Lenght / cfg.snapStep); 
            var pos = road.GetPathPosition(roadPos);
            var rot = road.GetPathOrientation(roadPos);
            var linePos = _lines.GetLineOffset(Random.Range(_lines.Min, _lines.Max+1));
            
            p.transform.SetPositionAndRotation (pos + rot * linePos, rot);
        }
        
        // TODO: Add Raycaster on spawning Coins
    }

    CoinBase ConfigureCoin(CoinBase coin)
    {
        var max = coinVariants.Sum(c => c.rarity);
        if (coin != false && max > 0f)
        {
            var sel = Random.Range(0f, max);
            foreach (var var in coinVariants)
            {
                if (sel <= var.rarity)
                {
                    coin.Config(var.effect, var.count);
                    break;
                }
                sel -= var.rarity;
            }
        }
        return coin;
    }
    

    private void DoDistribution(Func<Prop> provide, Action<Prop> selectPlace, int count) 
    {
        for (var i = 0; i < count; i++)
        {
            selectPlace.Invoke(provide.Invoke());
        }
    }
    
}
