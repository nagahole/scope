using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Deselect : MonoBehaviour
{
    public void DESELECT() {
        EventSystem.current.SetSelectedGameObject(null);
    }
}
