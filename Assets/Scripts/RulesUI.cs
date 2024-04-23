using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using System.Linq;
using UnityEngine.EventSystems;

public class RulesUI : SingletonBehaviour<RulesUI>
{
    [SerializeField] CanvasGroup _rulesUICanvasGroup;
    [SerializeField] RuleOptionUI _ruleOptionPrefab;
    [SerializeField] Transform _ruleOptionsParent;
    List<RuleOptionUI> _currentRuleOptions = new List<RuleOptionUI>();

    [Header("Passives")]
    [SerializeField] CanvasGroup _passivesUICanvasGroup;
    [SerializeField] Transform _passiveOptionsParent;
    List<Passive> _availablePassives = new List<Passive>();
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ActivateRulesUI()
    {
        StartCoroutine(ActivateRulesUIRoutine());
    }

    IEnumerator ActivateRulesUIRoutine()
    {
        Time.timeScale = 0f;
        _rulesUICanvasGroup.alpha = 1f;
        _rulesUICanvasGroup.interactable = true;

        yield return new WaitForSecondsRealtime(0.25f);

        for (int i = 0; i < 3; i++)
        {
            RuleOptionUI option = Instantiate(_ruleOptionPrefab, _ruleOptionsParent);
            _currentRuleOptions.Add(option);
            option.SetUpRuleOption();


            yield return new WaitForSecondsRealtime(0.15f);
        }

        if(_currentRuleOptions.Count > 0 )
            EventSystem.current.SetSelectedGameObject(_currentRuleOptions[0].OptionButton.gameObject);
    }

    public void DeactivateRulesUI()
    {
        Time .timeScale = 1f;
        _rulesUICanvasGroup.alpha = 0f;
        _rulesUICanvasGroup.interactable = false;

        List<RuleOptionUI> currentOptions = _currentRuleOptions.ToList();
        _currentRuleOptions.Clear();
        foreach (RuleOptionUI option in currentOptions)
            Destroy(option.gameObject);
    }

    IEnumerator ActivatePassiveUIRoutine()
    {
        Time.timeScale = 0f;
        _rulesUICanvasGroup.alpha = 1f;
        _rulesUICanvasGroup.interactable = true;

        yield return new WaitForSecondsRealtime(0.25f);

        for (int i = 0; i < 3; i++)
        {
            RuleOptionUI option = Instantiate(_ruleOptionPrefab, _ruleOptionsParent);
            _currentRuleOptions.Add(option);
            option.SetUpRuleOption();


            yield return new WaitForSecondsRealtime(0.15f);
        }

        if (_currentRuleOptions.Count > 0)
            EventSystem.current.SetSelectedGameObject(_currentRuleOptions[0].OptionButton.gameObject);
    }
}
