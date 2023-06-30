using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezing : MonoBehaviour
{
    private float _timeFreeze = 0;
    private float _currentTimeFreeze = 3f;
    private float _procent = 0.1f;
    private Coroutine _currentFreezeCoroutine = null;

    private float _startingSpeed;
    private float _objectSpeed;

    private IEnumerator WaitForSecondsWithPause(float waitTime) => ProjectContext.Instance.PauseManager.WaitForSecondsWithPause(waitTime);
    private bool _isPaused => ProjectContext.Instance.PauseManager.IsPaused;

    public void Freeze(ref float speed)
    {
        if (_currentFreezeCoroutine != null)
        {
            _currentTimeFreeze = 0;
        }
        else
        {
            _startingSpeed = speed;
            _objectSpeed = speed;
            _currentFreezeCoroutine = StartCoroutine(FreezeEnumerator());
        }
        speed *= 1 - _procent;
    }

    public IEnumerator FreezeEnumerator()
    {
        while (_currentTimeFreeze <= _timeFreeze)
        {
            if (!_isPaused)
            {
                _currentTimeFreeze += Time.deltaTime;
            }
            yield return null;
        }
        _currentFreezeCoroutine = null;
        _objectSpeed = _startingSpeed;
    }
}
