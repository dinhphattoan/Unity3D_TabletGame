using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelShowcase : MonoBehaviour
{
    public float rotateSpeed=25.0f;
    void Start()
    {
        
    }

    void Update()
    {
        this.transform.Rotate(Vector3.down, rotateSpeed * Time.deltaTime);
    }
}
