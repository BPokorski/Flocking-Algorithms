using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using TMPro;
public class StopWatch : MonoBehaviour
{
    private TextMeshProUGUI _watchText;
    public bool isTimerActive = false;
    private float _timeStart ;

    private NumberFormatInfo _numberFormatInfo = new NumberFormatInfo();
    // Start is called before the first frame update
    void Start()
    {
        _numberFormatInfo.NumberDecimalSeparator = ".";
        _numberFormatInfo.NumberDecimalDigits = 2;
        _watchText = gameObject.GetComponent<TextMeshProUGUI>();
        _watchText.text = string.Format(_numberFormatInfo, "{0:N}", _timeStart);
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerActive)
        {
            _timeStart += Time.deltaTime;
            _watchText.text = string.Format(_numberFormatInfo, "{0:N}", _timeStart);
        }
    }

    public void StartStopTiming()
    {
        isTimerActive = !isTimerActive;
        Debug.Log(isTimerActive);
    }
}
