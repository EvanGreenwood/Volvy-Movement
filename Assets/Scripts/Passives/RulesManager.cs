using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using UnityEngine.UIElements;

public class RulesManager : SingletonBehaviour<RulesManager> 
{
    [System.Serializable]
    public class RulePair
    {
        public RuleTrigger trigger;
        public RuleEffect effect;
        public RuleProbability probability;
    }
    [System.Serializable]
    public class RuleProbability {
        public int portion = 1;
        public int outOf = 2;
        private int _chances = 0;

        public RuleProbability(int portion, int outOf)
        {
            this.portion = portion;
            this.outOf = outOf;
        }
        public void ImproveChances()
        {
            if (portion == 0)
            {
                portion++;
                if (outOf == 0) outOf = 4;
            }
            else if (portion == 1 && outOf > 2)
            {
                outOf--;
                //
            } 
            else  if (outOf < 4)
            {
                outOf++;
                portion++;
            }
            else if (portion < outOf)
            {
                portion++;
            }
            else 
            {
                portion += 2;
            }
        }
        public void GetChances(out int fraction, out int denominator)
        {
            if (portion > outOf)
            {
                if (portion % 4 == 0)
                {
                    fraction = portion / 4;
                    denominator = outOf / 4;
                }
                else
                {
                    fraction = portion / 2;
                    denominator = outOf / 2;
                }
            }
            else if (portion % 2 == 0 && outOf % 2 == 0)
            {
                fraction = portion/2;
                denominator = outOf/2;
            }
            else
            {
                fraction = portion;
                denominator = outOf;
            }
        }
        public bool Roll( out int triggerCount)
        {
            triggerCount =  ( portion + _chances % outOf) / outOf;
            bool occurs = triggerCount > 0;
            //
            //Debug.Log(" Roll " + occurs + "  triggerCount " + triggerCount + ".... _chances " + _chances + " portion " + portion + " outOf " + outOf);
            //
            _chances++;
            if (occurs)
            { 
                return true;
            }
            else
            {
                
                return false;
            }
        }
    } 
    //
    [SerializeField]  private  List<RulePair> rulePairs = new List<RulePair>();
    [SerializeField] private List<RuleRecipe> ruleRecipes = new List<RuleRecipe>();
    
    //
    public void TryTrigger(RuleTrigger trigger, Vector3 position)
    { 
        foreach (RulePair pair in rulePairs)
        {
            if (pair.trigger == trigger && pair.probability.Roll(out int triggerCount))
            {
                for (int i = 0; i < triggerCount; i++)
                {
                    PerformEffect(pair.effect, position, i, triggerCount);
                }
            }
        }
    }
    public void PerformEffect(RuleEffect effect, Vector3 position, int index, int triggeredCount)
    {
        if (effect == null)
        {
            Debug.LogError("Null effect ");
        }
        else if (effect == RuleEffect.SpawnCarrotSeed  )
        {  
            float angle = Time.time * 2 + (Mathf.PI * 2 / triggeredCount) * index;
            Vector2 v = new Vector2(Mathf.Sin(angle),  Mathf.Cos(angle));
            //
            EffectsController.Instance.SpawnShrapnel(VegetableType.Carrot, position, v, 15, 2);
        }
        else if (effect == RuleEffect.SpawnBomb)
        {
            float angle = Time.time * 2 + (Mathf.PI * 2 / triggeredCount) * index;
            Vector2 v = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle));
            //
            UnitManager.Instance.VolvyDropBomb(position, v);
        }

    }
    public bool TryCombineVegetables(VegetableType vegetableThis, VegetableType vegetableOther, out VegetableType created)
    { 
        foreach (RuleRecipe recipe in ruleRecipes)
        { 
            if (recipe.vegetableIngredient == vegetableThis && recipe.vegetableIngredientOther == vegetableOther)
            {
                created = recipe.vegetableResult;  
                return true;
            }
        }
        created = null;
        return false;
    }

    public void AddRulePair(RuleTrigger trigger, RuleEffect effect, RuleProbability probability)
    {
        RulePair rulePair = new RulePair(); 
        rulePair.trigger = trigger;
        rulePair.effect = effect;
        rulePair.probability = probability;

        rulePairs.Add(rulePair);
    }
}
