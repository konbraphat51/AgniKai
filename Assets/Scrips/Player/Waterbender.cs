using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waterbender : Player
{
    [SerializeField] private string aniCshoot = "BasicShoot";

    public enum State
    {
        waiting,
        Cshoot
    }
    public State state
    {
        //cannot be changed by outsider class
        private set;
        get;
    } = State.waiting;

    private void StartCshoot()
    {
        Debug.Log("Cshoot");
        animator.SetTrigger(aniCshoot);
    }

    protected override void OnKeyDownC()
    {
        base.OnKeyDownC();

        switch (state)
        {
            case State.waiting:
                StartCshoot();
                break;
        }
    }
}
