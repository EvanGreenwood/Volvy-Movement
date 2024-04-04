using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private AnimatedSprite _animatedSprite;
   [SerializeField] private AnimatedSprite _explosionPrefab;
    [SerializeField] private float _knockForce = 5;
    private float _yOffsetSpeed = 0;
    private Vector2 _velocity;
    private float _yOffset = 0;
    void Start()
    { 
        //
        transform.position = transform.position.WithZ(transform.position.y * 0.02f);
    }

    // 
    void Update()
    {
        if (!_animatedSprite.enabled)
        {
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
            //
            Collider[] enemyColliders = Physics.OverlapSphere(transform.position, 3.1f, 1 << LayerMask.NameToLayer("Enemies"));
            for (int i = 0; i < enemyColliders.Length; i++)
            {
                if (enemyColliders[i].TryGetComponent<CharacterMover>(out CharacterMover mover))
                {
                    Vector3 diff = (mover.transform.position - transform.position).WithZ(0);
                    if (diff.magnitude < 1.7f)
                    {
                        mover.Damage(1);
                    }
                    else
                    {
                        mover.Stun(0.5f);
                        mover.Knock(diff.normalized * _knockForce);
                    }
                    // mover.Stun(1f);
                }
            }
            //
            Destroy(gameObject);
            //
        }
        else
        {
            if (_yOffset > 0 || _yOffsetSpeed > 0)
            {
                _yOffset += _yOffsetSpeed * Time.deltaTime;
                _yOffsetSpeed -= 50 * Time.deltaTime;
                if (_yOffset <= 0)
                {
                    _yOffset = 0;
                    _yOffsetSpeed *= -0.4f;
                    _velocity *= 0.6f;
                    if (Mathf.Abs(_yOffsetSpeed) < 1)
                    {
                        _yOffsetSpeed = 0;
                        _velocity = Vector2.zero;
                    }
                }
            }
            _animatedSprite.transform.localPosition = new Vector3(0, _yOffset, 0); // ** OFFSET **
            //
            transform.position += _velocity.ToVector3() * Time.deltaTime;
            transform.position = transform.position.WithZ(transform.position.y * 0.02f);
        }
    }
    public void Launch(Vector2 velocity, float ySpeed)
    {
        this._velocity = velocity;
        _yOffsetSpeed = ySpeed;

    }
}
