using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DiscSampling))]
public class PoissonDiskSamplerEditor : Editor
{
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        DiscSampling sampler = target as DiscSampling;
        if(GUILayout.Button("Generate Samplings")) {
            sampler.Generate();
        }
    }
}
