using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class VersionText : MonoBehaviour
{
    void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = "v" + Application.version;
    }
}
