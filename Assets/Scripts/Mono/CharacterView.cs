using System;
using Mono;
using UnityEngine;

public class CharacterView : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private CollisionCatcher _collider;
    private int[] _stateHash;


    public enum AnimationHash
    {
        Idle, Run, Dead, Jump, MoveSpeed, Grounded
    }

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        var enums = Enum.GetNames(typeof(AnimationHash));
        
        _stateHash = new int[enums.Length];
        for (var i = 0; i < enums.Length; i++)
        {
            _stateHash[i] = Animator.StringToHash(enums[i]);
        }
    }
    
    public void SetState(AnimationHash state, bool on)
    {
        if (_animator)
        {
            if (_stateHash == null)
            {
                Init();
            }

            _animator.SetBool(_stateHash[(int)state], on);
        }
    }
    
    public CollisionCatcher GetCollider () => _collider;

    private void OnValidate()
    {
        if (_collider == false)
        {
            _collider = GetComponentInChildren<CollisionCatcher>();
        }
    }

    public void SetRunning(bool run)
    {
        // _animator.SetFloat("MoveSpeed", run ? 1 : 0);
        _animator.SetFloat(_stateHash[(int)AnimationHash.MoveSpeed], run ? 1 : 0);
    }
    public void SetJumping(bool jump)
    {
        _animator.SetBool("Grounded", !jump);
        // _animator.SetBool(_stateHash[(int)AnimationHash.Grounded], !jump);
    }
}
