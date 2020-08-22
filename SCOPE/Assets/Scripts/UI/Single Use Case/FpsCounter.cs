using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FpsCounter : MonoBehaviour
{
    public static FpsCounter instance {
        get;
        private set;
    }

    private int frameCount = 0;
    private float dt = 0.0f;
    [SerializeField] [ReadOnly] private float fps = 0.0f;
    [SerializeField] private float updateRate = 4.0f;  // 4 updates per sec.
    [SerializeField] private TextMeshProUGUI text;

    private void Awake() {
        if(instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else {
            Destroy(gameObject);
        }
    }

    private void Update() {
        if (Time.timeScale != 1)
            return;
        frameCount++;
        dt += Time.deltaTime;
        if (dt > 1.0f / updateRate) {
            fps = frameCount / dt;
            text.text = fps.ToString("0");
            frameCount = 0;
            dt -= 1.0f / updateRate;
        }
    }
}
