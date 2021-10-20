using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    private void OnEnable()
    {
        Sequence deactivateSequence = DOTween.Sequence();
        deactivateSequence.AppendInterval(1f).OnComplete(() => { gameObject.SetActive(false); });
    }
}
