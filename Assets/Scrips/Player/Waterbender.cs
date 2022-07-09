using Common;
using UnityEngine;

public class Waterbender : Player
{
    [SerializeField] private string aniCshoot = "BasicShoot";
    [SerializeField] private string aniRunning = "Running";

    [SerializeField] private float runningSpeed = 5.0f;

    public enum State
    {
        standing,
        running,
        Cshoot
    }
    public State state
    {
        //cannot be changed by outsider class
        private set;
        get;
    } = State.standing;

    private void Run(Directions direction)
    {
        //animation
        animator.SetBool(aniRunning, true);

        //move
        switch (direction)
        {
            case Directions.right:
                transform.position += new Vector3(runningSpeed, 0, 0);
                break;
            case Directions.left:
                transform.position += new Vector3(-runningSpeed, 0, 0);
                break;
        }
    }

    private void StartCshoot()
    {
        animator.SetTrigger(aniCshoot);
    }

    private bool CanMove()
    {
        switch (state)
        {
            case State.standing:
            case State.running:
                return true;
            default:
                return false;
        }
    }

    private void ChangeState(State nextState)
    {
        //only when different state
        if (state == nextState) return;

        //process for departing from current state
        switch (state)
        {
            case State.running:
                //animation
                animator.SetBool(aniRunning, false);
                break;
        }

        state = nextState;
    }

    protected override void OnKeyDownC()
    {
        base.OnKeyDownC();

        //Cshoot
        if (CanMove())
        {
            StartCshoot();
            ChangeState(State.Cshoot);
        }
    }

    protected override void OnKeyPushingA()
    {
        base.OnKeyPushingA();

        //Run
        if (CanMove())
        {
            Debug.Log("a");
            Run(Directions.left);
            ChangeState(State.running);
        }
    }

    protected override void OnKeyPushingD()
    {
        base.OnKeyPushingD();

        //Run
        if (CanMove())
        {
            Run(Directions.right);
            ChangeState(State.running);
        }
    }

    protected override void OnKeyUpA()
    {
        base.OnKeyUpA();

        //stop running
        if(state == State.running)
        {
            ChangeState(State.standing);
        }
    }

    protected override void OnKeyUpD()
    {
        base.OnKeyUpD();

        //stop running
        if (state == State.running)
        {
            ChangeState(State.standing);
        }
    }
}
