using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static event Action<Vector2> OnTap;
    public static event Action<Vector2> OnDrag;
    public static event Action          OnRelease;

    private void Update()
    {
        if (!GameManager.CanShoot) return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            OnTap?.Invoke(Input.mousePosition);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            OnDrag?.Invoke(Input.mousePosition);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            OnRelease?.Invoke();
        }
    }
}
