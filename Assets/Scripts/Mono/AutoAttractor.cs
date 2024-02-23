
using System;
using UnityEngine;

// TODO: Replace to animator
public class AutoAttractor : MonoBehaviour
{
    public Vector3 rotationAxis;
    public float scale;
    public float scalePeriod;

    private void Update()
    {
        var r = rotationAxis * Time.deltaTime;
        transform.Rotate(r);
        transform.localScale = Vector3.one * (1f + Mathf.Sin(Time.time * scalePeriod) * scale);
    }
}
