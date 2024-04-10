/* ------------------------ */
/* ---- AUTO GENERATED ---- */
/* ---- AVOID TOUCHING ---- */
/* ------------------------ */

using UnityEngine;
using System.Collections.Generic;
using Framework;

public enum LayerName
{
	Default = 0,
	TransparentFX = 1,
	IgnoreRaycast = -1,
	Water = 4,
	UI = 5,
	Enemies = 13,
	RootedVegetables = -1,
	UprootedVegetables = -1
}

public enum SortingLayerName
{
	Default = 0
}

public static class Layer
{

	public const int Default = 0;
	public const int TransparentFX = 1;
	public const int IgnoreRaycast = -1;
	public const int Water = 4;
	public const int UI = 5;
	public const int Enemies = 13;
	public const int RootedVegetables = -1;
	public const int UprootedVegetables = -1;

}

public static class SortingLayer
{

	public const int Default = 0;

}

public static class Tag
{

	public const string Untagged = "Untagged";
	public const string Respawn = "Respawn";
	public const string Finish = "Finish";
	public const string EditorOnly = "EditorOnly";
	public const string MainCamera = "MainCamera";
	public const string Player = "Player";
	public const string GameController = "GameController";

}

public static partial class LayerMasks
{

	public static readonly LayerMask ALL_LAYERS = ~0;
	public static readonly LayerMask NO_LAYERS = 0;
	public static readonly LayerMask Default = 1;
	public static readonly LayerMask TransparentFX = 2;
	public static readonly LayerMask IgnoreRaycast = -2147483648;
	public static readonly LayerMask Water = 16;
	public static readonly LayerMask UI = 32;
	public static readonly LayerMask Enemies = 8192;
	public static readonly LayerMask RootedVegetables = -2147483648;
	public static readonly LayerMask UprootedVegetables = -2147483648;

}

public static class CollisionMatrix
{

	public static readonly LayerMask ALL_LAYERS = ~0;
	public static readonly LayerMask NO_LAYERS = 0;
	public static readonly LayerMask DefaultCollisionMask = -1;
	public static readonly LayerMask TransparentFXCollisionMask = -1;
	public static readonly LayerMask IgnoreRaycastCollisionMask = -1;
	public static readonly LayerMask WaterCollisionMask = -1;
	public static readonly LayerMask UICollisionMask = -1;
	public static readonly LayerMask EnemiesCollisionMask = -1;
	public static readonly LayerMask RootedVegetablesCollisionMask = -1;
	public static readonly LayerMask UprootedVegetablesCollisionMask = -1;

}

public static class SceneNames
{

	public const string SampleScene = "SampleScene";

}

[CreateAssetMenu(fileName = "Rule Effect", menuName = "Scriptable Enum/Rule Effect")]
public partial class RuleEffect
{

	public static RuleEffect[] AllRuleEffects { get { if (__allRuleEffects == null) __allRuleEffects = GetValues<RuleEffect>(); return __allRuleEffects; } }
	public static RuleEffect SpawnBomb { get { if (__spawnBomb == null) __spawnBomb = GetValue<RuleEffect>("Spawn Bomb"); return __spawnBomb; } }
	public static RuleEffect SpawnCarrotSeed { get { if (__spawnCarrotSeed == null) __spawnCarrotSeed = GetValue<RuleEffect>("Spawn Carrot Seed"); return __spawnCarrotSeed; } }
	
	protected static RuleEffect[] __allRuleEffects;
	protected static RuleEffect __spawnBomb;
	protected static RuleEffect __spawnCarrotSeed;

}

[CreateAssetMenu(fileName = "Rule Passive", menuName = "Scriptable Enum/Rule Passive")]
public partial class RulePassive
{

	public static RulePassive[] AllRulePassives { get { if (__allRulePassives == null) __allRulePassives = GetValues<RulePassive>(); return __allRulePassives; } }
	
	protected static RulePassive[] __allRulePassives;
	

}

[CreateAssetMenu(fileName = "Rule Trigger", menuName = "Scriptable Enum/Rule Trigger")]
public partial class RuleTrigger
{

	public static RuleTrigger[] AllRuleTriggers { get { if (__allRuleTriggers == null) __allRuleTriggers = GetValues<RuleTrigger>(); return __allRuleTriggers; } }
	public static RuleTrigger BumpEnemy { get { if (__bumpEnemy == null) __bumpEnemy = GetValue<RuleTrigger>("Bump Enemy"); return __bumpEnemy; } }
	public static RuleTrigger Overcharge { get { if (__overcharge == null) __overcharge = GetValue<RuleTrigger>("Overcharge"); return __overcharge; } }
	public static RuleTrigger VolvyEat { get { if (__volvyEat == null) __volvyEat = GetValue<RuleTrigger>("Volvy Eat"); return __volvyEat; } }
	public static RuleTrigger VolvyHurt { get { if (__volvyHurt == null) __volvyHurt = GetValue<RuleTrigger>("Volvy Hurt"); return __volvyHurt; } }
	
	protected static RuleTrigger[] __allRuleTriggers;
	protected static RuleTrigger __bumpEnemy;
	protected static RuleTrigger __overcharge;
	protected static RuleTrigger __volvyEat;
	protected static RuleTrigger __volvyHurt;

}

[CreateAssetMenu(fileName = "Rule Recipe", menuName = "Scriptable Enum/Rule Recipe")]
public partial class RuleRecipe
{

	public static RuleRecipe[] AllRuleRecipes { get { if (__allRuleRecipes == null) __allRuleRecipes = GetValues<RuleRecipe>(); return __allRuleRecipes; } }
	public static RuleRecipe CarrotOnionBomb { get { if (__carrotOnionBomb == null) __carrotOnionBomb = GetValue<RuleRecipe>("Carrot+Onion=Bomb"); return __carrotOnionBomb; } }
	
	protected static RuleRecipe[] __allRuleRecipes;
	protected static RuleRecipe __carrotOnionBomb;

}