using UnityEngine;

public class DiceSideChecker : MonoBehaviour
{
    public int value=0;
    public bool isTriggered =false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
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
