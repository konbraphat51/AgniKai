using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Common;

public class ElementParent : MonoBehaviour
{
    public enum State
    {
        nothing,
        logarithm
    }
    private State state = State.nothing;

    public enum Animation
    {
        logarithm
    }
    
    private Vector3 rootLocalPosition;

    public bool hasLife = false;
    public int lifeLeft = 100;
    public bool destroyHittingWall = false;

    //consts (not making serializefield because to changed by code)
    public bool toRight;
    public float projectionXSpeed = 10.0f;
    public float xMutiplier = 0.2f;
    public float yMutiplier = 1.0f;

    public int playerN = 1;

    private Animator animator;

    private void Start()
    {
        animator = this.GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        switch (state)
        {
            case State.logarithm:
                AnimateLogarithm();
                break;
        }

        lifeLeft--;
        if(hasLife && lifeLeft <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    //Use for trigger animation
    public void Animate(Animation animation)
    {
        rootLocalPosition = this.transform.localPosition;

        switch (animation)
        {
            case Animation.logarithm:
                state = State.logarithm;
                break;
        }
    }

    private void AnimateLogarithm()
    {
        float calculatedX;
        if (toRight)
        {
            calculatedX = this.transform.localPosition.x + projectionXSpeed;
        }
        else
        {
            calculatedX = this.transform.localPosition.x - projectionXSpeed;
        }

        float movedX = Mathf.Abs(calculatedX - this.rootLocalPosition.x);

        float calculatedY = Mathf.Log(movedX * xMutiplier) * yMutiplier + this.rootLocalPosition.y;

        //apply the position
        this.transform.localPosition
            = new Vector3(calculatedX, calculatedY, this.transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        string colliderTag = collision.gameObject.tag;

        if(destroyHittingWall && TagCommon.Contains(colliderTag, "Wall"))
        {
            Destroy(this.gameObject);
        }
    }
}
