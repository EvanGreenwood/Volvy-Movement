using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DamageNumber : MonoBehaviour
{
    [SerializeField] TMP_Text _damageNumberText;
    float _life = 1;
    public void SetUpDamageNumber(int damageAmount)
    {
        _damageNumberText.text = damageAmount.ToString();
    }

    void Update()
    {
        if (_life > 0)
            _life -= Time.deltaTime;
        else
            Destroy(gameObject);
    }
}
