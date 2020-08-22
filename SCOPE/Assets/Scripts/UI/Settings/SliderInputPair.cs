using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class SliderInputPair : MonoBehaviour
{
    [System.Serializable]
    public class SliderInputChanged  : UnityEvent<float> { }

    [SerializeField] private Slider slider;
    [SerializeField] private TMP_InputField input;
    [Space]
    [SerializeField] private Vector2 range = new Vector2(0, 100);
    [SerializeField] [ReadOnly] private float _value = 0;
    [SerializeField] [Range(0, 8)] private int displayDp;
    [Space]
    [SerializeField] private SliderInputChanged _onValueChanged;
    public SliderInputChanged onValueChanged => _onValueChanged;

    public float value => _value;

    private string format;

    private void Awake() {
        slider.minValue = range.x;
        slider.maxValue = range.y;
        SetValue(_value);
        format = string.Join(".", "0", new string('0', displayDp));
    }

    private void OnValidate() {
        if (range.y < range.x)
            Debug.LogWarning($"{gameObject}'s range settings aren't valid");
    }

    public void SilentSetValue(float value) {
        value = Mathf.Clamp(value, range.x, range.y);
        slider.value = value;
        input.text = value.ToString(format);
        this._value = value;
    }

    public void SetValue(float value) {
        SilentSetValue(value);
        _onValueChanged.Invoke(this.value);
    }

    /// <summary>
    /// Note: This does not change the value to match the range
    /// </summary>
    /// <param name="min"></param>
    /// <param name="max"></param>
    public void SetRange(float min, float max) {
        range.x = min;
        range.y = max;
        slider.minValue = min;
        slider.maxValue = max;
    }

    public void SetRange(Vector2 range) {
        SetRange(range.x, range.y);
    }

    //For text input field
    public void SetValue(string s) {
        float val = 0;
        if (float.TryParse(s, out val)) { //successful
            SetValue(float.Parse(s));
        } else
            SetValue(val);
    }
}
