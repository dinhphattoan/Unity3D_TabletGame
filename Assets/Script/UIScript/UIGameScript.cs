using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameScript : IUIScript
{
    [Header("UI Reference")]
    [Space]
        [SerializeField] GameObject gameObjectRawImage;
    [SerializeField] GameObject briefMessageText;
    [SerializeField] GameObject forceInputUI;
    [SerializeField] GameObject rollDicePanel;
    [SerializeField] Slider forceSlider;
    [SerializeField] TextMeshProUGUI forceText;
    [SerializeField] List<Button> playerButtons;
    public float messageSecond = 2f;

    bool isMouseDown = false;
    public float forceDrag;
    public Vector3 mouseDown, mouseUp;
    [Header("Reference scripts")]
    [Space]
    [SerializeField] CameraManager cameraManager;
    [SerializeField] GameManager gameManager;
    //  is called before the first frame update
    void Start()
    {
        base.Initialize();
        var players = gameManager.GetAllPlayers();
        for (int i = 0; i < players.Count; i++)
        {
            playerButtons[i].gameObject.SetActive(true);

            playerButtons[i].gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = gameManager.GetAllPlayers()[i].name;
        }
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
    public IEnumerator SpaceToContinue(string message, int frontSize)
    {
        var fronttemp = briefMessageText.transform.GetComponent<TextMeshProUGUI>().fontSize;
        briefMessageText.transform.GetComponent<TextMeshProUGUI>().fontSize = frontSize;
        briefMessageText.transform.GetComponent<TextMeshProUGUI>().text = message;
        briefMessageText.gameObject.SetActive(true);
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
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
    public void OnClick_SinglePlayerCamera(int index)
    {
        cameraManager.indexSwitcher = index;
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
        RectTransform  rect= gameObjectRawImage.GetComponent<RectTransform>();
        while (forceDrag == -1)
        {
            if (Input.GetMouseButton(0) )
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
                forceSlider.value = distance > 1 ? 1 : distance;
            }
            if (Input.GetMouseButtonUp(0))
            {
                isMouseDown = false;
                //Get mouse on world space
                mouseUp = Input.mousePosition;
                float distance = Vector2.Distance(mouseDown, mouseUp);
                forceSlider.value = distance > 1 ? 1 : distance;
                forceDrag = (distance > 300 ? 300 : distance) / 4;
                forceText.text = "";
            }

            yield return null;
        }
    }
       
}
