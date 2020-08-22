using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ConfirmBox : MonoBehaviour
{
    [SerializeField] private GameObject container;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI bodyText;

    public delegate void confirmAction();

    private confirmAction confirm;

    public void SetTitle(string title) {
        titleText.text = title;
    }

    public void SetBody(string body) {
        bodyText.text = body;
    }

    public void Enter() {
        container.SetActive(true);
    }

    public void Exit() {
        container.SetActive(false);
    }

    public void Confirm() {
        confirm?.Invoke();
    }

    public void SetConfirmAction(confirmAction e) {
        confirm = e;
    }
}
