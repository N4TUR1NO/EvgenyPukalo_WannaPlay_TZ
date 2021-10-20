using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public delegate void WithVector2Parameter(Vector2 touchPos);

    public static event WithVector2Parameter PlayerClickedScreen;
    public static event WithVector2Parameter PlayerTouchingScreen;
    public static event Action PlayerReleasedScreen;

    private void Update()
    {
        if (!GameManager.CanShoot)
            return;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PlayerClickedScreen?.Invoke(Input.mousePosition);
        }
        else if (Input.GetKey(KeyCode.Mouse0))
        {
            PlayerTouchingScreen?.Invoke(Input.mousePosition);
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            PlayerReleasedScreen?.Invoke();
        }
    }
}
