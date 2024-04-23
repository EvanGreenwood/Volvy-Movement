using Framework;
using System.Collections;
using UnityEngine;

public class DamageEffectManager : SingletonBehaviour<DamageEffectManager>
{
    [SerializeField] private CanvasGroup _canvasGroup;

    public IEnumerator FlashRoutine(float time)
    {
        _canvasGroup.alpha = 1f;
        float timeLeft = time;
        while (timeLeft > 0f)
        {
            _canvasGroup.alpha = timeLeft / time;

            timeLeft -= Time.deltaTime;
            yield return null;
        }
        _canvasGroup.alpha = 0f;
    }
}