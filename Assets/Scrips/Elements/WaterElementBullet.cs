using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class WaterElementBullet : ElementBullet
{
    void Update()
    {
        lifeLeft--;
        if(lifeLeft <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public override void Damage(int damage)
    {
        base.Damage(damage);

        lifeLeft -= damage;
    }

}
