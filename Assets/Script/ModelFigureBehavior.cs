using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelFigureBehavior : MonoBehaviour
{
    Rigidbody figureRigid;
    bool isStatic = false;
    SoundManager soundManager;
    [Header("Sound clips")]
    [Space]
    public List<AudioClip> exitementSound;
    // Start is called before the first frame update
    void Start()
    {
        soundManager = FindObjectOfType<SoundManager>();
        figureRigid =  GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
