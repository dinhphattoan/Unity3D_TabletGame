using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] float moveSpeed = 0.2f;


    [SerializeField] Vector3 cameraPlayerOffSet = new Vector3(0, 10, 0);
    [SerializeField] GameObject cameraPivot;
    [SerializeField] private float distanceToTarget = 10;
    [SerializeField] GameManager game;
    [SerializeField] List<Transform> listTransforms = new List<Transform>();
    public int indexSwitcher = -1;
    Quaternion globalRotation;
    Vector3 globalPosition;
    Vector3 velocity;

    private void Start()
    {
        listTransforms.Add(this.transform);
        globalRotation = this.transform.rotation; ;
        globalPosition = this.transform.position;
        foreach (var player in game.GetAllPlayers())
        {
            listTransforms.Add(player.modelFigure.transform);
        }
    }

    private void Update()
    {
        if (indexSwitcher != -1)
            if (Input.GetMouseButton(0))
            {
                if (Input.mousePosition.x < (float)Screen.width / 2f)
                {
                    cameraPivot.transform.localRotation= new Quaternion(0, cameraPivot.transform.localRotation.y+ (Time.deltaTime * moveSpeed * 3), 0, 0);
                }
                else if (Input.mousePosition.x > (float)Screen.width / 2f)
                {
                    cameraPivot.transform.localRotation= new Quaternion(0, cameraPivot.transform.localRotation.y- (Time.deltaTime * moveSpeed * 3), 0, 0);
                }
            }
        UpdatePlayerValue();
        ChangeFocus(indexSwitcher);
    }
    void ChangeFocus(int index)
    {
        if (indexSwitcher != -1 && indexSwitcher < listTransforms.Count)
        {
            if (indexSwitcher != 0)
            {
                if (cameraPivot.transform.parent != listTransforms[indexSwitcher].transform)
                {
                    cameraPivot.transform.SetParent(listTransforms[indexSwitcher]);
                    cameraPivot.transform.localPosition = cameraPlayerOffSet;
                    cameraPivot.transform.localRotation = new Quaternion(0, listTransforms[indexSwitcher].rotation.y, 0, 0);
                }

                Vector3 behindCameraPivot = cameraPivot.transform.position + (-cameraPivot.transform.forward * distanceToTarget);
                this.transform.position = Vector3.SmoothDamp(this.transform.position, behindCameraPivot + cameraPlayerOffSet, ref velocity, Time.deltaTime * moveSpeed);
                this.transform.LookAt(listTransforms[indexSwitcher].position);

            }
            else
            {
                this.transform.position = Vector3.SmoothDamp(this.transform.position, globalPosition, ref velocity, Time.deltaTime * moveSpeed); ;
                this.transform.rotation = Quaternion.Slerp(this.transform.rotation, globalRotation, Time.deltaTime * moveSpeed);
            }

        }

    }
    public void UpdatePlayerValue()
    {
        var players = game.GetAllPlayers();
        for (int i = 1; i < listTransforms.Count; i++)
        {
            listTransforms[i] = players[i - 1].modelFigure.transform;
        }
    }
}
