using Framework;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VegetableObject : MonoBehaviour
{
    public VegetableType Type => type;
     [SerializeField] private VegetableType type;
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
    public bool CanBeEaten => _edibleDelay <= 0;
    private float _edibleDelay = 0.4f;
    //
    [SerializeField] private bool _startRooted = true;
    //
    private bool _rooted = true;
    [SerializeField] int _numberOfBumpsToUproot = 1;
    float _bumpCollisionRate;
    [SerializeField] TMP_Text _bumpText;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_startRooted)
        {
            _rooted = true;
            _shadow.enabled = false;
           
            transform.position = transform.position.WithZ(transform.position.y * 0.02f);
            _currentAnimation = _rootedAnimation;
            _currentAnimation.Reset(_spriteRenderer);
            _rootedAnimation.frame = 1000 - (int)(transform.position.x);
        }
        else
        {
            _rooted = false;
            _growing = false;
            //
            transform.position = transform.position.WithZ(transform.position.y * 0.02f);
            _currentAnimation = _uprootedAnimation;
            _currentAnimation.Reset(_spriteRenderer);
            gameObject.layer = LayerMask.NameToLayer("Uprooted Vegetables");
        }
    }
    //
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

            if(_bumpCollisionRate > 0) 
                _bumpCollisionRate -= Time.deltaTime;
        }
        else
        {
            if (_edibleDelay > 0)
            {
                _edibleDelay -= Time.deltaTime;
            }
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
                    if (UnitManager.Instance.playerTransform == null)
                        return;

                    Vector3 diff = (UnitManager.Instance.playerTransform.position - transform.position).WithZ(0);
                    if (diff.sqrMagnitude < 0.2f)
                    {
                        if(StomachManager.HasInstance)
                            StomachManager.Instance.SpawnVegetable(type);

                        //
                        if (RulesManager.HasInstance)
                        {
                            RulesManager.Instance.TryTrigger(RuleTrigger.VolvyEat, UnitManager.Instance.playerTransform.position);
                            if (type.eatTrigger != null)  RulesManager.Instance.TryTrigger(type.eatTrigger, UnitManager.Instance.playerTransform.position);
                        }

                        //AbilitiesManager.Instance.chargeBar.AddCharge(0.25f);
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
                        if (diff.sqrMagnitude < UnitManager.Instance.VolvyMover.CollectionRange)
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
            if (_currentAnimation != null)  _currentAnimation.Exit(_spriteRenderer);
            _currentAnimation = animation;
            _currentAnimation.Reset(_spriteRenderer);
        }
        else
        {
            _currentAnimation.Run(_spriteRenderer, Time.time);
        }
        // 
    }
    public void TryUproot()
    {
        if (_bumpCollisionRate <= 0)
        {
            _numberOfBumpsToUproot--;
            _bumpCollisionRate = 1f;

            if (_bumpText != null)
                _bumpText.text = _numberOfBumpsToUproot.ToString();
        }

        if (_numberOfBumpsToUproot <= 0)
        {
            _uprootedTime = Time.time;
            _rooted = false;
            gameObject.layer = LayerMask.NameToLayer("Uprooted Vegetables");
            _shadow.enabled = true;

            if (_bumpText != null)
                Destroy(_bumpText);
        }
    }
}
