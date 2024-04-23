using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PassiveOptionUI : MonoBehaviour
{
    public Button OptionButton => _button;
    [SerializeField] Button _button;
    [SerializeField] TMP_Text _passiveNameText;
    [SerializeField] TMP_Text _passiveDescriptionText;

    [SerializeField] Passive _passive; 
    
    public void SetUpPassiveOption(Passive chosenPassive)
    {
        _button.onClick.AddListener(PassiveSelected);

        _passive = chosenPassive;

        _passiveNameText.text = _passive.passiveName;
        _passiveDescriptionText.text = _passive.passiveDescription;
    }

    void PassiveSelected()
    {
        UnitManager.Instance.VolvyMover.GetComponent<BurrowTrail>().IncreaseBurrowDamage(_passive.extraBurrowDamage);
        UpgradeManager.Instance.DeactivatePassiveUI();
    }
}
