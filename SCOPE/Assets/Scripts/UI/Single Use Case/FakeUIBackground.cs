using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FakeUIBackground : MonoBehaviour
{
    [SerializeField] private RectTransform centre;
    [SerializeField] private RectTransform realBackground;
    [SerializeField] private RectTransform fakeBackground;

    [ContextMenu("ADJUST SIZE")]
    public void AdjustPositionAndSize() {
        fakeBackground.position = centre.position;
        fakeBackground.sizeDelta = new Vector2(realBackground.rect.width,
            realBackground.rect.height);
    }
}
