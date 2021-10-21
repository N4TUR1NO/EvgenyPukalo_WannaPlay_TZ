using System.Collections.Generic;
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

    [Header("Rotation")] 
    [SerializeField] private float angleOfYRotation;
    [SerializeField] private float angleOfXRotation;
    
    private GameObject _vfxShot;
    private GameObject _spawn;

    private Queue<GameObject> _bulletsPool;
    
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
        _bulletsPool       = new Queue<GameObject>();
        for (int i = 0; i < poolSize; i++)
        {
            GameObject newBullet = Instantiate(bulletPrefab, transform.parent, true);
            newBullet.SetActive(false);
            _bulletsPool.Enqueue(newBullet);
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
            LaunchBullet();
            ShotEffects();
        }
    }

    private void ShotEffects()
    {
        VFX();
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
    
    private void Shake()
    {
        Vector3 weaponStartPosition = transform.localPosition;
        transform.DOShakePosition(duration, Vector3.one * power)
                 .OnComplete(() => { transform.localPosition = weaponStartPosition; });
    }
    
    private void LaunchBullet()
    {
        //Take from the pool
        GameObject bullet = _bulletsPool.Dequeue();
        bullet.transform.position = _spawn.transform.position;
        bullet.SetActive(true);
        bullet.GetComponent<Rigidbody>().velocity = _spawn.transform.forward * bulletSpeed;
        
        //return to the pool
        Sequence deactivateSequence = DOTween.Sequence();
        deactivateSequence.AppendInterval(1f).OnComplete(() =>
        {
            bullet.SetActive(false);
            AddToPool(bullet);
        });
    }

    private void AddToPool(GameObject objectToAdd)
    {
        _bulletsPool.Enqueue(objectToAdd);
    }
}
