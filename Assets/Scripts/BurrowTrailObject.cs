using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurrowTrailObject : MonoBehaviour
{
    [SerializeField] private CharacterAnimator.CharacterAnimation _grow;
    [SerializeField] private CharacterAnimator.CharacterAnimation _shrink;
    //
    private CharacterAnimator.CharacterAnimation _currentAnimation;

    private SpriteRenderer _spriteRenderer;
    //
    private bool _growing = true;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // 
    void Update()
    {
        if (_growing)
        {
            RunAnimation(_grow);
        }
        else
        {
            RunAnimation(_shrink);
            //
            if (_shrink.IsFinished)
            {
                Destroy(gameObject);
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
    public void StartShrinking()
    {
        _growing = false;
    }
}
