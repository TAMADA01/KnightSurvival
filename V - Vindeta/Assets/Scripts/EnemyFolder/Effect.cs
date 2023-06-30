using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour, IPauseHandler
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        ProjectContext.Instance.PauseManager.Registered(this);
    }

    private void OnDisable()
    {
        ProjectContext.Instance.PauseManager.UnRegistered(this);
    }

    void IPauseHandler.SetPaused(bool isPaused)
    {
        _animator.speed = isPaused ? 0f : 1f;
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}
