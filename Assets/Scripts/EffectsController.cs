using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;
using UnityEngine.UIElements;

public class EffectsController : SingletonBehaviour<EffectsController>
{
    //  
    [SerializeField] private Shrapnel _shrapnelPrefab;
    [SerializeField] private Shrapnel _boneShrapnelPrefab;
    [SerializeField] private Shrapnel _carrotSeedPrefab;



    //  
    void Update()
    {
        
    }
    public void SpawnShrapnel(int count, Vector3 pos, float upwardForce, float sidewaysForce)
    {
        SpawnShrapnel(_shrapnelPrefab, count, pos, upwardForce, sidewaysForce);
    }
    public void SpawnBoneShrapnel(  int count, Vector3 pos, float upwardForce, float sidewaysForce)
    {
        SpawnShrapnel(_boneShrapnelPrefab, count, pos, upwardForce, sidewaysForce);
    }
    public void SpawnShrapnel(VegetableType type,  int count, Vector3 pos, float upwardForce, float sidewaysForce)
    {
        if (type == VegetableType.Carrot)
        {
            SpawnShrapnel(_carrotSeedPrefab, count, pos, upwardForce, sidewaysForce);
        }
    }
    public void SpawnShrapnel(Shrapnel prefab, int count, Vector3 pos, float upwardForce, float sidewaysForce)
    {
        if (count == 1)
        {
            Vector3 randomForce = Random.onUnitSphere;
            //
            Shrapnel shrapnel = Instantiate(prefab, pos, Quaternion.identity);
            shrapnel.Launch(upwardForce + randomForce.y, new Vector2(randomForce.x * sidewaysForce, randomForce.y * sidewaysForce));
        }
        else
        {
            for (int i = 0; i < count; i++)
            {
                float angle = (Mathf.PI * 2 / count) * i;
                Vector3 v = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
                //
                  Vector3 randomForce = Random.onUnitSphere;
                //
                Shrapnel shrapnel = Instantiate(prefab, pos, Quaternion.identity);
                shrapnel.Launch(upwardForce + randomForce.y * upwardForce* 0.3f, new Vector2((v.x + randomForce.x * 0.25f) * sidewaysForce,( v.z + randomForce.z * 0.25f) * sidewaysForce));
            }
        }
    }
    public void SpawnShrapnel(VegetableType type, Vector3 pos, Vector2 direction, float upwardForce, float sidewaysForce)
    {

        SpawnShrapnel(type.seedPrefab, pos, direction, upwardForce, sidewaysForce);

    }
    public void SpawnShrapnel(Shrapnel prefab,  Vector3 pos, Vector2 direction, float upwardForce, float sidewaysForce)
    {
         
            Vector3 randomForce = Random.onUnitSphere;
            //
            Shrapnel shrapnel = Instantiate(prefab, pos, Quaternion.identity);
            shrapnel.Launch(upwardForce * 0.75f  + upwardForce * 0.25f * randomForce.y, new Vector2((direction.x + randomForce.x* 0.25f) * sidewaysForce, (direction.y + randomForce.z * 0.25f) * sidewaysForce));
         
    }
}
