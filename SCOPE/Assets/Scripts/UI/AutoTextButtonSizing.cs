using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AutoTextButtonSizing : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI targetText;
    [SerializeField] private RectTransform targetTransform;
    [SerializeField] private Vector2 padding;

    private void Awake() {
        targetTransform.sizeDelta = new Vector2(
            targetText.preferredWidth, 
            targetText.preferredHeight) 
            + padding * 2;

    }
}
