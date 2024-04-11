using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedEffect : MonoBehaviour
{
   [SerializeField] CharacterAnimator.CharacterAnimation _animation;
    [SerializeField] private AnimationCurve _scaleCurve;
    private float _counter = 0;
    [SerializeField] private float _scaleTime = 0.4f;
    [SerializeField] private bool _randomAngle = true;
    [SerializeField] private float _rotaitonSpeed = 2;
    private Vector3 _initialScale;
    private SpriteRenderer _spriteRenderer;
    //
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialScale = transform.localScale; 
        if (_randomAngle)
        {
            transform.rotation = Quaternion.Euler(0, 0, Random.value * 360);
        }
        _animation.Reset(_spriteRenderer);
    }

    //  
    void Update()
    {
        _counter += Time.deltaTime;
        transform.localScale = _initialScale * _scaleCurve.Evaluate(_counter/ _scaleTime);
        transform.Rotate(0, 0, Time.deltaTime * _rotaitonSpeed);
        //
        if (_animation.IsFinished)
        {
            Destroy(gameObject);
        }
        else
        {
            _animation.Run(_spriteRenderer, _counter);
        }
    }
}
