using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour {
    public string optionString;
    private Text SliderPercentLabel;
    private Slider slider;
	// Use this for initialization
	void Start () {
        SliderPercentLabel = transform.Find("Handle Slide Area/Handle/Text").GetComponent<Text>();
        slider = GetComponent<Slider>();
        if (PlayerPrefs.HasKey(optionString))
            slider.value = PlayerPrefs.GetInt(optionString)/100.0f;
	}

    public void SliderUpdate()
    {
        int newValue = (int)(100*slider.value);
        SliderPercentLabel.text = (newValue).ToString() + '%';
        PlayerPrefs.SetInt(optionString, newValue);
        PlayerPrefs.Save();
    }
}
