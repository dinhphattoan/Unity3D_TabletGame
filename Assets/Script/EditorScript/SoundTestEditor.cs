using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SoundManager))]
public class SoundTestEditor : Editor {
    public override void OnInspectorGUI() {
        base.OnInspectorGUI();
        SoundManager soundManager = target as SoundManager;
        if(GUILayout.Button("Play Next Sound")) {
            soundManager.PlayNextSong(true);
        }
    }
}