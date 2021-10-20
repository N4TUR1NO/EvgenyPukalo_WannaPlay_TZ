using UnityEngine;
using DG.Tweening;

public class Weapon : MonoBehaviour
{

    #region Fields
    [Header("Bullet")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private int poolSize = 10;
    
    [Header("Weapon Shake")] 
    [SerializeField] private float duration = 0.2f;
    [Range(0.01f, 0.1f)] [SerializeField] private float power = 0.03f;
    
    [Header("Sound")]
    [SerializeField] private AudioClip sfxShot;

    [Header("Rotation")] 
    [SerializeField] private float angleOfYRotation;
    [SerializeField] private float angleOfXRotation;
    
    private GameObject _vfxShot;
    private GameObject _spawn;
    
    private GameObject[] _bulletsPool;
    private int _bulletPoolPointer;
    
    #endregion
   
    #region Init

    private void Start()
    {
        _vfxShot = transform.GetChild(0).GetChild(0).gameObject;
        _spawn = transform.GetChild(0).GetChild(1).gameObject;

        if (_bulletsPool == null)
            InitPool();
    }

    private void InitPool()
    {
        _bulletPoolPointer = 0;
        _bulletsPool       = new GameObject[poolSize];

        for (int i = 0; i < _bulletsPool.Length; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform, true);
            newBullet.SetActive(false);
            _bulletsPool[i] = newBullet;
        }
    }
    
    #endregion
    
    #region OnEnable/OnDisable
    
    private void OnEnable()
    {
        Aim.AimPositionChanged                += UpdateWeaponRotation;
        InputManager.OnRelease += Shot;
    }

    private void OnDisable()
    {
        Aim.AimPositionChanged                -= UpdateWeaponRotation;
        InputManager.OnRelease -= Shot;
    }
    
    #endregion

    private void UpdateWeaponRotation(Vector2 aimPosition)
    {
        float interpolantY = Mathf.InverseLerp(0, Screen.currentResolution.height, aimPosition.x);
        float interpolantX = Mathf.InverseLerp(0, Screen.currentResolution.width, aimPosition.y);

        float weaponAngleY = Mathf.Lerp(-angleOfYRotation, angleOfYRotation, interpolantY);
        float weaponAngleX = Mathf.Lerp(angleOfXRotation, -angleOfXRotation, interpolantX);

        transform.localEulerAngles = new Vector3(weaponAngleX, weaponAngleY, 0f);
    }

    private void Shot()
    {
        if (GameManager.CanShoot)
        {
            LaunchBall();
            ShotEffects();
        }
    }

    private void ShotEffects()
    {
        VFX();
        SFX();
        Shake();
    }

    private void VFX()
    {
        if (!_vfxShot.activeSelf)
        {
            _vfxShot.SetActive(true);
            Sequence seq = DOTween.Sequence();
            seq.AppendInterval(0.2f)
               .OnComplete(() => { _vfxShot.SetActive(false); });
        }
    }

    private void SFX()
    {
        if (sfxShot)
            AudioSource.PlayClipAtPoint(sfxShot, Vector3.zero);
    }

    private void Shake()
    {
        Vector3 weaponStartPosition = transform.localPosition;
        transform.DOShakePosition(duration, Vector3.one * power)
                 .OnComplete(() => { transform.localPosition = weaponStartPosition; });
    }
    
    private void LaunchBall()
    {
        while (true)
        {
            _bulletPoolPointer = (_bulletPoolPointer + 1) % _bulletsPool.Length;
            if (!_bulletsPool[_bulletsPool.Length - 1 - _bulletPoolPointer].activeSelf)
                break;
        }

        _bulletsPool[_bulletsPool.Length - 1 - _bulletPoolPointer].transform.position = _spawn.transform.position;
        _bulletsPool[_bulletsPool.Length - 1 - _bulletPoolPointer].SetActive(true);
        _bulletsPool[_bulletsPool.Length - 1 - _bulletPoolPointer].GetComponent<Rigidbody>().velocity = _spawn.transform.forward * bulletSpeed;
    }
}
