using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteWithAnimation : MonoBehaviour
{
    protected bool _isEndAnimation;
    public IEnumerator WaitEndAnimation()
    {
        yield return new WaitUntil(() => _isEndAnimation);
        _isEndAnimation = false;
    }

    public void EndAnimation()
    {
        _isEndAnimation = true;
    }
}
