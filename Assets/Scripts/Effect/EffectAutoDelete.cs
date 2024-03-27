using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class EffectAutoDelete : MonoBehaviour
{
    [SerializeField] private float destroyTime = 1f;

    private void Start()
    {
        transform.localScale = new Vector3(0, 0, 0);
        transform.DOScale(new Vector3(0.7f,0.7f,0.7f), 1f);
        Destroy(gameObject, destroyTime);
    }
}
