using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : IPauseHandler
{
    private List<IPauseHandler> _handlers = new List<IPauseHandler>();
    public bool IsPaused { get; private set; }

    public void Registered(IPauseHandler handler)
    {
        _handlers.Add(handler);
    }

    public void UnRegistered(IPauseHandler handler)
    {
        _handlers.Remove(handler);
    }

    public void SetPaused(bool isPaused) 
    {
        IsPaused = isPaused;
        foreach (var handler in _handlers)
        {
            handler.SetPaused(IsPaused);
        }
    }

    public IEnumerator WaitForSecondsWithPause(float waitTime)
    {
        float currentTime = 0f;
        while (currentTime < waitTime)
        {
            if (!IsPaused)
            {
                currentTime += Time.deltaTime;
            }
            yield return null;
        }
    }
}
