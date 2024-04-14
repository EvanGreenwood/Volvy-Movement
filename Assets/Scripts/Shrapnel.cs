using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrapnel : MonoBehaviour
{
    [SerializeField] private float _gravity = -40;

    private float _yOffsetSpeed = 0;
    private float _yOffset = 0;
    private Vector2 _velocity = Vector2.zero;
   
    private SpriteRenderer _spriteRenderer;
    private int _flyingFrame = 0;
    private float _frameCounter = 0.0334f;
    [SerializeField] private Sprite[] _flyingSprites;
    [SerializeField] private CharacterAnimator.CharacterAnimation _landAnim;
    [SerializeField] private CharacterAnimator.CharacterAnimation _fadeAnim; 
    private float _groundTime = 0;
    [SerializeField] private GameObject _shadow;
    //
    [SerializeField] private VegetableType _seedType ;

    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    //  
    void Update()
    {
        transform.position += _velocity.ToVector3() * Time.deltaTime; 
        transform.position = transform.position.WithZ(transform.position.y * 0.02f); 
        //
        if (_yOffsetSpeed > 0 || _yOffset > 0)
        {
           
            _yOffsetSpeed += _gravity * Time.deltaTime;
            _yOffset += _yOffsetSpeed * Time.deltaTime;
            if (_yOffset <= 0 && _yOffsetSpeed < 0)
            {
                //Debug.Log(" Hit ground "); 
                //
                _yOffset = 0;
                _yOffsetSpeed *= -0.4f;
                if (Mathf.Abs(_yOffsetSpeed) < 2)
                {
                    _yOffsetSpeed = 0;
                    _velocity = Vector2.zero;
                    _shadow.SetActive(false);
                    // **********  LAND  **************
                }
                else
                {
                    _velocity *= 0.3f;
                }
            }
            //
            _spriteRenderer.transform.localPosition = new Vector3(0, _yOffset, 0);
            //
           
            //
            _frameCounter += Time.deltaTime;
            if (_frameCounter > 0.0334f)
            {
                _frameCounter -= 0.0334f;
                //
                int targetFrame = (int)Mathf.Clamp(Mathf.Abs(_yOffsetSpeed) / 3, 0, 3);
                if (_flyingFrame < targetFrame) _flyingFrame++;
                if (_flyingFrame > targetFrame) _flyingFrame--;
                //
                _spriteRenderer.sprite = _flyingSprites[_flyingFrame];
            }
        }
        else
        {
            _groundTime += Time.deltaTime;
            if (_groundTime < 0.3f)
            {
                _landAnim.Run(_spriteRenderer, Time.time);
            }
            else
            {
                _fadeAnim.Run(_spriteRenderer, Time.time);
                if (_fadeAnim.IsFinished)
                {
                    //
                    if (_seedType != null)
                    {
                        VegetablesSpawner.Instance.SpawnVegetable(  _seedType, transform.position);
                    }
                    Destroy(gameObject);
                    //
                    
                }
            }
        }
        //
       // _spriteRenderer.transform.localPosition = new Vector3(0, 2, 0);
       // Debug.Log(" _yOffset  " + _yOffset + "   + _yOffsetSpeed " + _yOffsetSpeed + " " + _spriteRenderer.transform.localPosition);
    }
    public void Launch(float upwardForce, Vector2 force)
    {
        _velocity = force;
        _yOffsetSpeed = upwardForce;
    }
}
