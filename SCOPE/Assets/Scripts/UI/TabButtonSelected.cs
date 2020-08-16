using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabButtonSelected : MonoBehaviour, ISelectable
{
    [SerializeField] private GameObject linesParent;

    public void Deselect() {
        linesParent.SetActive(false);
    }

    public void Select() {
        linesParent.SetActive(true);
    }
}
