using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextWave : MonoBehaviour
{
    [SerializeField] private Text _text;
    [SerializeField] private Transform _startTransform;
    [SerializeField] private Transform _midleTransform;
    [SerializeField] private Transform _endTransform;

    [SerializeField] private float _speedStart = 1f;
    [SerializeField] private float _speedEnd = 1f;

    [SerializeField] private AnimationCurve _speedCurve1;
    [SerializeField] private AnimationCurve _speedCurve2;
    [SerializeField] private AnimationCurve _sizeCurve;

    private bool _isPaused => ProjectContext.Instance.PauseManager.IsPaused;
    private IEnumerator WaitForSecondsWithPause(float waitTime) => ProjectContext.Instance.PauseManager.WaitForSecondsWithPause(waitTime);

    private void Start()
    {
        _text.enabled = false;
    }

    public IEnumerator Show(int wave)
    {
        yield return StartCoroutine(WaitForSecondsWithPause(1f));
        _text.enabled = true;

        _text.text = $"ÂÎËÍÀ {wave}";
        _text.transform.position = _startTransform.position;
        _text.transform.localScale = Vector3.one * _sizeCurve.Evaluate(0);

        float time = 0;
        while (time <= 1) 
        {
            if (!_isPaused)
            {
                _text.transform.position = Vector3.Lerp(_startTransform.position, _midleTransform.position, _speedCurve1.Evaluate(time));
                time += Time.deltaTime * _speedStart; 
            }
            yield return null;
        }
        
        yield return StartCoroutine(WaitForSecondsWithPause(0.5f));

        time = 0;

        while (time <= 1)
        {
            if (!_isPaused)
            {
                _text.transform.position = Vector3.Lerp(_midleTransform.position, _endTransform.position, _speedCurve2.Evaluate(time));
                _text.transform.localScale = Vector3.one * _sizeCurve.Evaluate(time);
                time += Time.deltaTime * _speedEnd;
            }
            yield return null;
        }
    }
}
