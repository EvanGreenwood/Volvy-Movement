using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Framework;

public class EffectsController : SingletonBehaviour<EffectsController>
{
    //  
    [SerializeField] private Shrapnel _shrapnelPrefab; 
     

    //  
    void Update()
    {
        
    }
    public void SpawnShrapnel(int count, Vector3 pos, float upwardForce, float sidewaysForce)
    {
        if (count == 1)
        {
            Vector3 randomForce = Random.onUnitSphere;
            //
            Shrapnel shrapnel = Instantiate(_shrapnelPrefab, pos, Quaternion.identity);
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
                Shrapnel shrapnel = Instantiate(_shrapnelPrefab, pos, Quaternion.identity);
                shrapnel.Launch(upwardForce + randomForce.y * upwardForce* 0.3f, new Vector2((v.x + randomForce.x * 0.25f) * sidewaysForce,( v.z + randomForce.z * 0.25f) * sidewaysForce));
            }
        }
    }
}
