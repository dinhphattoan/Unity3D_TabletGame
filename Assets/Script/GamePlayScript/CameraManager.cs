using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform playerTransform;
    public Transform mapTransform;
    public Vector2 mouseDownPosition;
    public Vector2 mouseUpPosition;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Select the object to focus in that priority order
    /// </summary>
    void FocusSelector()
    {
        //Player focus if not null
        if (playerTransform)
        {
            this.transform.LookAt(playerTransform);
        }
        //Default focus
        else
        {
            this.transform.LookAt(mapTransform);
        }

    }
    void CameraFocusChange(Transform gameObject)
    {

    }
    void HandleRotation()
    {
        //Player camera control
        if (Input.GetMouseButtonDown(0))
        {
            mouseDownPosition.x = Input.GetAxis("Mouse X");
            mouseDownPosition.y = Input.GetAxis("Mouse Y");

        }

    }

    private void OnApplicationQuit()
    {

    }
}
