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

    public static void CubeWasTouch()
    {
        if (CanShoot)
        {
            CanShoot = false;
            CubeTouch?.Invoke();
            if (CubeTouch != null) _cubesCount = CubeTouch.GetInvocationList().Length;
        }
    }

    public static void StartRewind() => RewindStarted?.Invoke();

    public static void IncCubesAtRest()
    {
        if (--_cubesCount == 0)
            CubesAtRest?.Invoke();
    }
}
