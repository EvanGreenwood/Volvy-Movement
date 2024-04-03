using System;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;


public class CharacterAnimator : MonoBehaviour
{
    [Serializable]
    public class CharacterAnimation
    {
        public  Sprite[] frames;
        public float frameCounter =0;
        public float frameRate = 0.0334f;
        public int frame = 0;
        public bool loop = true;
        public int loopFrame = -1;
        //
        public void Reset(SpriteRenderer renderer)
        {
            frameCounter = 0;
            frame = 0;
            renderer.sprite = frames[frame];
        }
        //
        public void Run(SpriteRenderer renderer)
        {
            frameCounter += Time.deltaTime;
            if (frameCounter >= frameRate)
            {
                frame++;
                frameCounter -= frameRate;
                if (loop)
                {
                    if (loopFrame >= 0 && frame >= frames.Length)
                    {
                        frame = loopFrame; 
                    }
                    renderer.sprite = frames[frame % frames.Length]; 
                }
                else
                {
                    renderer.sprite = frames[Mathf.Clamp(frame, 0, frames.Length - 1)];
                }
            }
        }
        public bool IsFinished => frame >= frames.Length && !loop;
    }
    private CharacterInput _input;
    private CharacterMover _mover; 
    private Rigidbody _rigidbody;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    //
    [SerializeField] private CharacterAnimation _idleAnimation;
    [SerializeField] private CharacterAnimation _runAnimation;
    [SerializeField] private CharacterAnimation _burrowAnimation;
    [SerializeField] private CharacterAnimation _exitburrowAnimation;
    [SerializeField] private CharacterAnimation _stunnedAnimation;
    [SerializeField] private CharacterAnimation _deathAnimation;
    //
    private CharacterAnimation _currentAnimation;
    void Start()
    {
        _input = GetComponent<CharacterInput>(); 
        _rigidbody = GetComponent<Rigidbody>(); 
        _mover = GetComponent<CharacterMover>();
    }

    // 
    void LateUpdate()
    {
        if (_mover.movementState != MovementState.Burrow && _mover.movementState != MovementState.ExitBurrow && _mover.movementState != MovementState.Stunned && _mover.movementState != MovementState.Dead)
        {
            if (_input.HoldingLeft && _input.HoldingRight)
            {
                // ***  BORK  ***
            }
            else if (_input.HoldingLeft)
            {
                _spriteRenderer.flipX = true;
            }
            else if (_input.HoldingRight)
            {
                _spriteRenderer.flipX = false;
            }
            else
            {
                // *** NADA ***
            }
        }
        //
        if (_mover.movementState == MovementState.Dead)
        {
            RunAnimation(_deathAnimation);
        }
        else if (_mover.movementState == MovementState.Idle)
        { 
            RunAnimation(_idleAnimation);
        }
        else if (_mover.movementState == MovementState.Stunned)
        {
            RunAnimation(_stunnedAnimation);
        }
        else if (_mover.movementState == MovementState.Run)
        {
            RunAnimation(_runAnimation);
        }
        else if (_mover.movementState == MovementState.Burrow)
        {
            RunAnimation(_burrowAnimation);
        }
        else if (_mover.movementState == MovementState.ExitBurrow)
        {
            RunAnimation(_exitburrowAnimation);
        }
    }
    public void RunAnimation(CharacterAnimation animation)
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
    public bool ReachedEndOfAnimation()
    {
        return _currentAnimation.frame >= _currentAnimation.frames.Length -1;
    }
}
