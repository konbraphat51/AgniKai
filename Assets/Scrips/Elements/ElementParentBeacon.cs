using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

//adding force towards this beacon
public class ElementParentBeacon : ElementParent
{
    public List<GameObject> elementsObject;

    //no SerializeField because to change by code
    public float beaconingForce = 200.0f;

    protected override void Update()
    {
        base.Update();

        Beacon();
    }

    void Beacon()
    {
        foreach(GameObject obj in elementsObject)
        {
            Vector3 direction = (this.transform.position 
                -obj.transform.position).normalized;

            obj.GetComponent<Rigidbody2D>().AddForce(direction * beaconingForce);
        }
    }

    

}
