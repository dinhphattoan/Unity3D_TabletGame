using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMapHostEventHandler : IUIScript
{

    // Start is called before the first frame update
    void Start()
    {
        base.Initialize();
        BackgroundTransistionOpen();
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void OnClick_NavigateToMenu()
    {
        BackgroundTransistionClose(0);

    }
}
