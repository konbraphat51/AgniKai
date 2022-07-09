using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //consts (not making serializefield because to changed by code)
    public bool toRight;
    public float projectionXSpeed = 10.0f;
    public float xMutiplier = 0.2f;
    public float yMutiplier = 1.0f;

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
}
