using UnityEngine;

public class AppStarted : MonoBehaviour
{
    private void Start()
    {
        ProjectContext.Instance.Initialized();
    }
}
