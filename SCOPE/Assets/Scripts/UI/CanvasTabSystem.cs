using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasTabSystem : MonoBehaviour {
    [System.Serializable]
    public struct CanvasButtonPair {
        public Canvas canvas;
        public Button button;
    }

    [SerializeField] private bool canDeselectSelectedButton = false;
    [Tooltip("Set to negative if no canvas should be active at first")]
    [SerializeField] private int initialActiveCanvas = 0;
    [SerializeField] private CanvasButtonPair[] canvasButtonPairs;


    private ISelectable currentlySelected;
    private Canvas activeCanvas;

    public void Start() {
        NagaUnityUtilities.NagaUtils.ExecuteAfterOneFrame(Setup);
    }

    private void Setup() {
        for (int i = 0; i < canvasButtonPairs.Length; i++) {
            var pair = canvasButtonPairs[i];
            pair.canvas.enabled = true;
            pair.canvas.gameObject.SetActive(false);
            pair.button.GetComponent<ISelectable>()?.Deselect();

            pair.button.onClick.AddListener(() => {
                if (canDeselectSelectedButton && activeCanvas == pair.canvas) {
                    currentlySelected?.Deselect();
                    activeCanvas = null;
                    pair.canvas.gameObject.SetActive(false);
                } else {
                    currentlySelected?.Deselect();
                    currentlySelected = pair.button.GetComponent<ISelectable>(); //Ceebs optimising
                    currentlySelected?.Select();

                    if (activeCanvas != null) {
                        activeCanvas.gameObject.SetActive(false);
                    }

                    pair.canvas.gameObject.SetActive(true);
                    activeCanvas = pair.canvas;
                }
            });
        }

        if (!(initialActiveCanvas < 0)) {
            activeCanvas = canvasButtonPairs[initialActiveCanvas].canvas;
            activeCanvas.gameObject.SetActive(true);
            currentlySelected = canvasButtonPairs[initialActiveCanvas].button.GetComponent<ISelectable>();
            currentlySelected?.Select();
        }
    }
}