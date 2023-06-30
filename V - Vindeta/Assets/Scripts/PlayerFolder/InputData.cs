using UnityEngine;

public class InputData
{
    [HideInInspector] public static string Horizontal = "Horizontal";
    [HideInInspector] public static string Vertical = "Vertical";

    public KeyCode EscKey = KeyCode.Escape;
    public KeyCode ShotKey = KeyCode.Mouse0;
}
