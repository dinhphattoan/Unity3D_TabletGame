using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScript : IUIScript
{
    public GameObject briefMessageText;
    public GameObject forceInputUI;
    public GameObject rollDicePanel;
    public Slider forceSlider;
    public float messageSecond = 2f;
    public Vector3 mouseDown, mouseUp;
    bool isMouseDown = false;
    public TextMeshProUGUI forceText;
    public float forceDrag;
    CameraManager cameraManager;
    //  is called before the first frame update
    void Start()
    {
        base.Initialize();
        cameraManager = FindObjectOfType<CameraManager>();
    }


    // Update is called once per frame
    void Update()
    {
        if (forceInputUI.activeSelf)
        {

        }
    }

    public IEnumerator ClickToContinue(string message, int frontSize)
    {
        var fronttemp = briefMessageText.transform.GetComponent<TextMeshProUGUI>().fontSize;
        briefMessageText.transform.GetComponent<TextMeshProUGUI>().fontSize = frontSize;
        briefMessageText.transform.GetComponent<TextMeshProUGUI>().text = message;
        briefMessageText.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetMouseButton(0));
        briefMessageText.gameObject.SetActive(false);
        briefMessageText.transform.GetComponent<TextMeshProUGUI>().fontSize = fronttemp;
    }
    //Coroutines
    public IEnumerator ShowBriefMessage(string message, int frontSize)
    {
        var fronttemp = briefMessageText.transform.GetComponent<TextMeshProUGUI>().fontSize;
        briefMessageText.transform.GetComponent<TextMeshProUGUI>().fontSize = frontSize;
        briefMessageText.transform.GetComponent<TextMeshProUGUI>().text = message;
        briefMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(messageSecond);
        briefMessageText.gameObject.SetActive(false);
        briefMessageText.transform.GetComponent<TextMeshProUGUI>().fontSize = fronttemp;
    }
    public bool IsUIIdle()
    {
        return !briefMessageText.activeSelf;
    }
    public void ShowForceInputUI()
    {
        forceInputUI.SetActive(true);
    }
    public void CloseForceInputUI()
    {
        forceInputUI.SetActive(false);
    }
    public void HideForceInputUI()
    {
        forceInputUI.SetActive(false);
    }
    public void OnClick_GlobalCamera()
    {
        cameraManager.indexSwitcher = 0;
    }
    public void OnClick_PlayersCamera()
    {
        cameraManager.indexSwitcher = 1;
    }
    public void OnClick_SinglePlayerCamera()
    {
        cameraManager.indexSwitcher = 2;
    }
    public IEnumerator ShowRollDicePanel()
    {
        forceText.gameObject.SetActive(true);
        rollDicePanel.SetActive(true);
        Color color = rollDicePanel.GetComponent<RawImage>().color;
        float a = color.a;
        while (color.a != 1)
        {
            color = new Color(color.r, color.g, color.b, a);
            a = Mathf.MoveTowards(a, 1, Time.deltaTime);
            rollDicePanel.GetComponent<RawImage>().color = color;
            yield return null;
        }
    }
    public IEnumerator CloseRollDicePanel()
    {
        forceText.gameObject.SetActive(false);
        rollDicePanel.SetActive(true);
        Color color = rollDicePanel.GetComponent<RawImage>().color;
        float a = color.a;
        while (color.a != 1)
        {
            color = new Color(color.r, color.g, color.b, a);
            a = Mathf.MoveTowards(a, 0, Time.deltaTime);
            rollDicePanel.GetComponent<RawImage>().color = color;
            yield return null;
        }
        rollDicePanel.SetActive(false);
    }
    public IEnumerator ForceInput()
    {
        forceDrag = -1;
        mouseDown = mouseUp = Vector3.zero;
        while (forceDrag == -1)
        {
            if (Input.GetMouseButton(0))
            {
                if (!isMouseDown)
                {
                    //Get mouse on world space
                    mouseDown = Input.mousePosition;
                    forceSlider.value = 0;
                }
                //OnMouseDrag
                isMouseDown = true;
                int distance = (int)Vector2.Distance(mouseDown, Input.mousePosition);
                forceText.text = "Force: " + (distance > 300 ? 300 : distance).ToString() + "/300";
                forceSlider.value = distance > 1 ? 1 : distance; // Drag distance
            }
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
                //Get mouse on world space
                mouseUp = Input.mousePosition;
                float distance = Vector2.Distance(mouseDown, mouseUp);
                forceSlider.value = distance > 1 ? 1 : distance; // Drag distance
                forceDrag = (distance > 300 ? 300 : distance) / 4;
                forceText.text = "";
            }

            yield return null;
        }
    }
}
