using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBullet : Bullet
{
    public override void Move()
    {
        Vector2 offset = MoveDirection.normalized * Speed * Time.fixedDeltaTime;
        _rigidbody2d.MovePosition(_rigidbody2d.position + offset);
    }
}
