using UnityEngine;

public class DiceSideChecker : MonoBehaviour
{
    public int value=0;
    public bool isTriggered =false;
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag=="Ground")
        {
            isTriggered=true;
        }
        else 
        {
            isTriggered=false;
        }
    }
    private void OnTriggerExit(Collider other) {
        if(other.gameObject.tag=="Ground")
        {
            isTriggered=false;
        }
    }
}
