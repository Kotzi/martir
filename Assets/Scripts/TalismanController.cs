using UnityEngine;
using DG.Tweening;

public class TalismanController: MonoBehaviour
{
    void Start()
    {
        DOTween.Sequence()
                .Append(this.transform.DOPunchScale(Vector3.one * 0.05f, 0.75f))
                .AppendInterval(0.5f)
                .SetLoops(-1);
    }
}
