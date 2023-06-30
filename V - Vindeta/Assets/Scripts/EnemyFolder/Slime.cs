using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : SimpleEnemy
{
    private AnimatorOverrideController _slimeOverrideController;
    [SerializeField] private SlimeStyle[] _styles;
    int numberStyle;

    private void Start()
    {
        int numberStyle = Random.Range(0, _styles.Length);
        _slimeOverrideController = new AnimatorOverrideController(_animator.runtimeAnimatorController);

        _animator.runtimeAnimatorController = _slimeOverrideController;

        _slimeOverrideController["RunBlue"] = _styles[numberStyle].RunAnimation;
        _effectDie = _styles[numberStyle].EffectDie;
    }
}
