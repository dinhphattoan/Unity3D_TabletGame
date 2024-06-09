using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Splines;
using UnityEngine.UIElements;

public class SplineTreeDistribution : MonoBehaviour
{
    // Start is called before the first frame update
    public float side_wide;
    public SplineContainer splineContainer;
    public int m_splineIndex;
    public float m_time;
    public float3 position;
    public float3 forward;
    public float3 upVector;
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        splineContainer.Evaluate(m_splineIndex, m_time, out position, out forward, out upVector);
        float3 right = Vector3.Cross(forward, upVector).normalized;
        
    }
}
