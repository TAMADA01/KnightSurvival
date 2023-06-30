using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AppearingText : MonoBehaviour
{
    [SerializeField] private AnimationCurve _attenuationCurve;
    [SerializeField] private AnimationCurve _moveCurve;
    [SerializeField] private float _speed;

    [SerializeField] private Text _text;

    private void Start()
    {
        StartCoroutine(StartAnimation());
    }

    public IEnumerator StartAnimation()
    {
        Vector3 startPosition = _text.transform.localPosition;
        float time = 0;
        while (time <= 1)
        {
            _text.transform.localPosition = startPosition + Vector3.up * _moveCurve.Evaluate(time);
            var tempColor = _text.color;
            tempColor.a = _attenuationCurve.Evaluate(time);
            _text.color = tempColor;
            time += Time.deltaTime * _speed;
            yield return null;
        }
        Destroy(gameObject);
    }
}
