
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RuleOptionUI : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    public Button OptionButton => _button;
    [SerializeField] Button _button;
    Image _buttonImage;

    public void SetUpRuleOption()
    {
        _button.onClick.AddListener(RuleSelected);
        _buttonImage = _button.GetComponent<Image>();
    }

    void RuleSelected()
    {
        RulesUI.Instance.DeactivateRulesUI();
    }

    public void OnSelect(BaseEventData eventData)
    {
        //_buttonImage.enabled = true;
    }

    public void OnDeselect(BaseEventData data)
    {
        //_buttonImage.enabled = false;
    }
}
