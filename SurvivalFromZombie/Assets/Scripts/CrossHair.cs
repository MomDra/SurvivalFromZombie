using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrossHair : MonoBehaviour
{
    [SerializeField] Image[] hitEffect;
    [SerializeField] float hitEffectTime;

    public void HitHead()
    {
        foreach (var eft in hitEffect)
        {
            eft.transform.gameObject.SetActive(true);
            eft.color = Color.red;
        }

        StartCoroutine(EndEffect());
    }

    public void HitBody()
    {
        foreach (var eft in hitEffect)
        {
            eft.transform.gameObject.SetActive(true);
            eft.color = Color.white;
        }

        StartCoroutine(EndEffect());
    }

    IEnumerator EndEffect()
    {
        yield return new WaitForSeconds(hitEffectTime);

        foreach (var eft in hitEffect)
        {
            eft.transform.gameObject.SetActive(false);
        }
    }
}
