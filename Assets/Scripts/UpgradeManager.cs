using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Framework;
using System.Linq;
using UnityEngine.EventSystems;

public class UpgradeManager : SingletonBehaviour<UpgradeManager>
{
    [SerializeField] CanvasGroup _rulesUICanvasGroup;
    [SerializeField] RuleOptionUI _ruleOptionPrefab;
    [SerializeField] Transform _ruleOptionsParent;
    List<RuleOptionUI> _currentRuleOptions = new List<RuleOptionUI>();

    [SerializeField] PassiveOptionUI _passiveOptionPrefab;
    List<PassiveOptionUI> _currentPassiveOptions = new List<PassiveOptionUI>();
    [SerializeField] List<Passive> _availablePassives = new List<Passive>();

    int _currentUpgradeIndex = 0;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void ActivateUpgradeUI()
    {
        _currentUpgradeIndex++;

        if(_currentUpgradeIndex % 2 == 0)
            StartCoroutine(ActivatePassiveUIRoutine()); 
        else
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
        List<Passive> availablePassives = _availablePassives.ToList();

        yield return new WaitForSecondsRealtime(0.25f);

        for (int i = 0; i < 2; i++)
        {
            Passive randomPassive = availablePassives.ChooseRandom();
            availablePassives.Remove(randomPassive);
            PassiveOptionUI option = Instantiate(_passiveOptionPrefab, _ruleOptionsParent);
            _currentPassiveOptions.Add(option);
            option.SetUpPassiveOption(randomPassive);

            yield return new WaitForSecondsRealtime(0.15f);
        }

        if (_currentPassiveOptions.Count > 0)
            EventSystem.current.SetSelectedGameObject(_currentPassiveOptions[0].OptionButton.gameObject);
    }


    public void DeactivatePassiveUI()
    {
        Time.timeScale = 1f;
        _rulesUICanvasGroup.alpha = 0f;
        _rulesUICanvasGroup.interactable = false;

        List<PassiveOptionUI> currentOptions = _currentPassiveOptions.ToList();
        _currentPassiveOptions.Clear();
        foreach (PassiveOptionUI option in currentOptions)
            Destroy(option.gameObject);
    }
}
