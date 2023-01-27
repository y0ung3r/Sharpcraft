using UnityEngine;

public class CursorAccessor : MonoBehaviour
{
    public bool Shown
    {
        get => Cursor.visible;

        set
        {
            Cursor.visible = value;
            Cursor.lockState = value switch
            {
                true => CursorLockMode.Confined,
                false => CursorLockMode.Locked
            };
        }
    }

    public void Start()
        => Shown = false;
}