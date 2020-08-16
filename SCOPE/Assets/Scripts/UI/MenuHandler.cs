using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField] Canvas mainMenuCanvas, scenarioSelectionCanvas, settingsCanvas;

    public void MAINMENU_PLAYBUTTON() {
        mainMenuCanvas.gameObject.SetActive(false);
        scenarioSelectionCanvas.gameObject.SetActive(true); 
    }

    public void MAINMENU_SETTINGSBUTTON() {
        mainMenuCanvas.gameObject.SetActive(false); 
        settingsCanvas.gameObject.SetActive(true);
    }

    public void MAINMENU_EXITBUTTON() {
        Application.Quit();
    }

    public void SETTINGSMENU_RETURNBUTTON() {
        settingsCanvas.gameObject.SetActive(false); 
        mainMenuCanvas.gameObject.SetActive(true); 
    }

    public void SCENARIOSELECTIONMENU_RETURNBUTTON() {
        scenarioSelectionCanvas.gameObject.SetActive(false); 
        mainMenuCanvas.gameObject.SetActive(true);
    }
}
