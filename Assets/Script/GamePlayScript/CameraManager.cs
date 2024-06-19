using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameManager gameManager;
    GameObject landmass;
    public Transform mapTransform;
    public Vector2 mouseDownPosition;
    public Vector2 mouseUpPosition;
    public Vector3 cameraOffSet = new Vector3(0, 30f, 0);
    public Vector3 globalVector3;
    public float smoothSpeed = 0.5f;
    //0 is global, 1 is players, 2 single player
    public bool[] switcher = new bool[2];
    public int indexSwitcher = 0;
    // Start is called before the first frame update
    void Start()
    {
        globalVector3 = this.transform.position;
        gameManager = FindObjectOfType<GameManager>();
        landmass = GameObject.Find("Landscape");

    }

    // Update is called once per frame
    void Update()
    {
        if (indexSwitcher == 0)
        {
            GlobalFocus();
        }
        else if (indexSwitcher == 1)
        {
            PlayersFocus();
        }
        else if (indexSwitcher == 2)
        {
            SinglePlayerFocus(gameManager.GetPlayerAtIndex(0).modelFigure.transform);
        }
    }

    void PlayersFocus()
    {
        Bounds bounds = new Bounds(gameManager.GetPlayerAtIndex(0).modelFigure.transform.position, Vector3.zero);
        foreach (var player in gameManager.GetAllPlayers())
        {
            bounds.Encapsulate(player.modelFigure.transform.position);
        }
        transform.position = Vector3.Lerp(transform.position, bounds.center + cameraOffSet, smoothSpeed * Time.deltaTime);

        SmoothLookAt(bounds.center);

        Camera camera = GetComponent<Camera>();
        camera.orthographicSize = bounds.extents.magnitude * 100f; 
    }
    
    //Make camera travel around the map globaly
    void GlobalFocus()
    {
        this.transform.position = Vector3.Lerp(this.transform.position, globalVector3, smoothSpeed * Time.deltaTime);
        SmoothLookAt(landmass.transform.position);
    }
    void SinglePlayerFocus(Transform player)
    {
        SmoothLookAt(player.position);
        transform.position = Vector3.Lerp(transform.position, player.position + cameraOffSet, smoothSpeed * Time.deltaTime);

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

    // Smoothly look at a target object
    void SmoothLookAt(Vector3 target)
    {
        // Calculate the direction to the target
        Vector3 direction = target - transform.position;

        // Smoothly rotate the camera towards the target
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), smoothSpeed * Time.deltaTime);
    }
}
