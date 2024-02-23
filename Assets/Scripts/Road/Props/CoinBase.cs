
using System;
using UnityEngine;

public class CoinBase : Prop
{
    private int _IdleState = Animator.StringToHash("Idle");
    private int _CollectState = Animator.StringToHash("Collect");
    
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject[] _views;
    
    public enum CoinEffect
    {
        Coin, SlowDown, SpeedUp, Fly
    }

    [SerializeField] private CoinEffect _effect;
    [SerializeField] private int _count;

    public CoinEffect effect => _effect;
    public int count => _count;

    public void Config(CoinEffect eff, int cnt)
    {
        for (var i = 0; i < _views.Length; i++)
        {
            _views[i]?.SetActive(i == (int)eff);
        }
        _effect = eff;
        _count = cnt;
    }

    private void OnEnable()
    {
        _animator?.Play(_IdleState);
    }
    
    public void OnCollect()
    {
        _animator?.Play(_CollectState);
    }
}
