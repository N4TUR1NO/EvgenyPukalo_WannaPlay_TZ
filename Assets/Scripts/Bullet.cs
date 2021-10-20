using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1f).OnComplete(() => { gameObject.SetActive(false); });
    }
}
