using System;
using UnityEngine;
using UnityEngine.UI;

public class RewindButton : MonoBehaviour
{
    #region Fields
    
    private GameObject _button;

    #endregion

    #region OnEnable/OnDisable
    
    private void OnEnable()
    {
        GameManager.CubesAtRest += DisplayButton;
    }

    private void OnDisable()
    {
        GameManager.CubesAtRest -= DisplayButton;
    }

    #endregion

    #region Init

    private void Start()
    {
        _button = transform.GetChild(0).gameObject;
        _button.GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    private void TaskOnClick()
    {
        GameManager.StartRewind();
        HideButton();
    }

    #endregion
    
    private void DisplayButton() => _button.SetActive(true);

    private void HideButton() => _button.SetActive(false);
}
