
using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static RulesManager;

public class RuleOptionUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button OptionButton => _button;
    [SerializeField] Button _button;

    [SerializeField] TMP_Text _triggerText;
    [SerializeField] TMP_Text _effectText;

    [SerializeField] RuleTrigger _trigger;
    [SerializeField] RuleEffect _effect;
    [SerializeField] RuleProbability _probability;
    public void SetUpRuleOption()
    {
        _button.onClick.AddListener(RuleSelected);

        _trigger = ScriptableEnum.GetRandomValue<RuleTrigger>();
        _effect = ScriptableEnum.GetRandomValue<RuleEffect>();
        _probability = _trigger.defaultProbability;
        _probability.outOf *= _effect.improbabilityMultiplier;

        _triggerText.text = _trigger.name + " " + _probability.portion + "/" + _probability.outOf + " chance";
        _effectText.text = _effect.name;
    }

    void RuleSelected()
    {
        RulesManager.Instance.AddRulePair(_trigger, _effect, _probability);
        RulesUI.Instance.DeactivateRulesUI();
    }

    public void OnSelect(BaseEventData eventData)
    {

    }

    public void OnDeselect(BaseEventData data)
    {

    }
}
