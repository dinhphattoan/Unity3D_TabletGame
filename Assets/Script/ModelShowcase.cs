using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelShowcase : MonoBehaviour
{
    public float rotateSpeed=25.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(Vector3.down, rotateSpeed * Time.deltaTime);
    }
}
