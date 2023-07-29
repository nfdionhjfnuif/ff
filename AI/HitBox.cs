using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public Target target;

    public void OnRaycastHit(GunSystem gun, Vector3 direction)
    {
        if (target != null)
        {
            target.TakeDamage(gun.damage, direction);
        }
    }
}