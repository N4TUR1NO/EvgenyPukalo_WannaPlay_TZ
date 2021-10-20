using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Fields
    
    public static bool CanShoot = true;
    private static int _cubesCount;
    
    #endregion
    
    #region Events

    public static event Action CubeTouch;
    public static event Action RewindStarted;
    public static event Action CubesAtRest;

    #endregion

    #region OnEnable/OnDisable

    private void OnEnable()
    {
        RewindButton.ButtonClicked += StartRewind;
    }

    private void OnDisable()
    {
        RewindButton.ButtonClicked -= StartRewind;
    }

    #endregion

    #region Init

    #endregion

    public static void CubeWasTouch()
    {
        if (CanShoot)
        {
            CanShoot = false;
            CubeTouch?.Invoke();
            _cubesCount = CubeTouch.GetInvocationList().Length;
        }
    }

    private static void StartRewind()
    {
        RewindStarted?.Invoke();
    }

    public static void IncCubesAtRest()
    {
        if (--_cubesCount == 0)
            CubesAtRest?.Invoke();
    }
}
