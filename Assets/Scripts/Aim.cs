using System;
using UnityEngine;
using UnityEngine.UI;

public class Aim : MonoBehaviour
{
    #region Fields
    
    private RectTransform _rect;
    private Image _image;
    private Vector2 _offset;

    private float _screenHeight;
    private float _screenWidth;

    #endregion

    public static event Action<Vector2> AimPositionChanged; 
    
    #region Init
    
    private void Start()
    {
        _rect = GetComponent<RectTransform>();
        _image = GetComponent<Image>();

        _screenHeight = Screen.currentResolution.height;
        _screenWidth  = Screen.currentResolution.width;
    }

    #endregion
    
    #region OnEnable/OnDisable
    
    private void OnEnable()
    {
        InputManager.OnTap  += SetOffset;
        InputManager.OnDrag += UpdatePosition;
        InputManager.OnRelease += DeactivateAim;
    }

    private void OnDisable()
    {
        InputManager.OnTap  -= SetOffset;
        InputManager.OnDrag -= UpdatePosition;
        InputManager.OnRelease -= DeactivateAim;
    }
    
    #endregion

    private void SetOffset(Vector2 touchPosition)
    {
        _image.enabled = true;
        //ResetPosition();
        _offset        = (Vector2)transform.position - touchPosition;
    }

    private void UpdatePosition(Vector2 touchPosition)
    {
        _rect.position = new Vector3(
            Mathf.Clamp(touchPosition.x + _offset.x, 0, _screenHeight),
            Mathf.Clamp(touchPosition.y + _offset.y, 0, _screenWidth), 
            0);
        
        AimPositionChanged?.Invoke(_rect.position);
    }

    private void DeactivateAim() => _image.enabled = false;

    private void ResetPosition() => _rect.position = new Vector2(_screenHeight / 2, _screenWidth / 2);
}
