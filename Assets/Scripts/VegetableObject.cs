using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VegetableObject : MonoBehaviour
{
    [SerializeField] private CharacterAnimator.CharacterAnimation _rootedAnimation;
    [SerializeField] private CharacterAnimator.CharacterAnimation _uprootedAnimation;
    [SerializeField] private CharacterAnimator.CharacterAnimation _growingAnimation;
    //
    private CharacterAnimator.CharacterAnimation _currentAnimation;

    private SpriteRenderer _spriteRenderer;
    //
    [SerializeField] private SpriteRenderer _shadow;
    //
    private float _uprootedTime = -1;
    private bool _collecting = false;
    private Vector3 _velocity;
    private bool _growing = true;
    //
   
    //
    private bool _rooted = true;
    void Start()
    {
        _shadow.enabled = false;
        _spriteRenderer = GetComponent<SpriteRenderer>();
        transform.position = transform.position.WithZ(transform.position.y * 0.02f);
        _currentAnimation = _rootedAnimation;
        _rootedAnimation.frame = 1000 - (int)(transform.position.x );
    }

    // 
    void Update()
    {
        if (_growing)
        {
            RunAnimation(_growingAnimation);
            //
            if (_growingAnimation.IsFinished) _growing = false;
        } 
        else if (_rooted)
        {
            RunAnimation(_rootedAnimation);
        }
        else
        {
            RunAnimation(_uprootedAnimation);
            // 
            //
            if (_collecting)
            {
                if (UnitManager.Instance.IsVolvyBurrowing)
                {
                    _velocity *= (1 - Time.deltaTime * 9);
                    if (_velocity.sqrMagnitude < 0.1f)
                    {
                        _collecting = false;
                    }
                }
                else
                {
                    Vector3 diff = (UnitManager.Instance.playerTransform.position - transform.position).WithZ(0);
                    if (diff.sqrMagnitude < 0.2f)
                    {
                        if(StomachManager.HasInstance)
                            StomachManager.Instance.SpawnVegetable();

                        //
                        if (RulesManager.HasInstance) RulesManager.Instance.TryTrigger(RuleTrigger.VolvyEat, UnitManager.Instance.playerTransform.position);

                        AbilitiesManager.Instance.chargeBar.AddCharge(0.25f);
                        Destroy(gameObject);
                    }
                    else
                    {
                        _velocity += diff * Time.deltaTime / diff.sqrMagnitude * 64;
                        if (diff.sqrMagnitude < 3)
                        {
                            _velocity *= (1 - Time.deltaTime * (3 - diff.sqrMagnitude));
                        }
                       
                    }
                }
                transform.position += _velocity * Time.deltaTime;
                transform.position = transform.position.WithZ(transform.position.y * 0.02f);
            }
            else if (Time.time - _uprootedTime > 0.6f)
            {
                if (!UnitManager.Instance.IsVolvyBurrowing)
                {
                    if (UnitManager.Instance.playerTransform != null)
                    {
                        Vector3 diff = (UnitManager.Instance.playerTransform.position - transform.position).WithZ(0);
                        if (diff.sqrMagnitude < 4)
                        {
                            _collecting = true;
                            _velocity = -diff * 3.5f;
                        }
                    }
                }
            }
        }
    }
    public void RunAnimation(CharacterAnimator.CharacterAnimation animation)
    {
        if (_currentAnimation != animation)
        {
            _currentAnimation = animation;
            _currentAnimation.Reset(_spriteRenderer);
        }
        else
        {
            _currentAnimation.Run(_spriteRenderer, Time.time);
        }
        // 
    }
    public void Uproot()
    {
        _uprootedTime = Time.time;
        _rooted = false;
        gameObject.layer =  LayerMask.NameToLayer("Uprooted Vegetables");
        _shadow.enabled = true;
    }
}
