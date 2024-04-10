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