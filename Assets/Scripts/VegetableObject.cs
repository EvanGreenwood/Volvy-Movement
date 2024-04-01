using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VegetableObject : MonoBehaviour
{
    [SerializeField] private CharacterAnimator.CharacterAnimation _rootedAnimation;
    [SerializeField] private CharacterAnimator.CharacterAnimation _uprootedAnimation;
    //
    private CharacterAnimator.CharacterAnimation _currentAnimation;

    private SpriteRenderer _spriteRenderer;
    //
    [SerializeField] private SpriteRenderer _shadow;
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
        if (_rooted)
        {
            RunAnimation(_rootedAnimation);
        }
        else
        {
            RunAnimation(_uprootedAnimation);
            // 
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
            _currentAnimation.Run(_spriteRenderer);
        }
        // 
    }
    public void Uproot()
    {
        Debug.Log(" Uproot! ");
        _rooted = false;
        gameObject.layer =  LayerMask.NameToLayer("Uprooted Vegetables");
        _shadow.enabled = true;
    }
}
