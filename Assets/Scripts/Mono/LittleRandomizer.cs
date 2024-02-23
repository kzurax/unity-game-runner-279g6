using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LittleRandomizer : MonoBehaviour
{
    [SerializeField] private float rotation = 0f;

    private void OnEnable() => Randomize();

    private void Randomize()
    {
        transform.localRotation = Quaternion.Lerp(Quaternion.identity, Random.rotation, rotation);
    }
}
