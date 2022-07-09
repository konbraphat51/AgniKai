using Common;
using UnityEngine;

public class Waterbender : Player
{
    [Header("Basical ability")]
    [SerializeField] private float runningSpeed = 5.0f;
    [Tooltip("adjustment of the position for generating (going to be plused)")]
    [SerializeField] private float feetY = 50.0f;

    [Header("Animator Events/Triggers")]
    [SerializeField] private string aniCshoot = "BasicShoot";
    [SerializeField] private string aniRunning = "Running";

    [Header("Prefabs")]
    [SerializeField] private GameObject waterBeaconPrefab;
    [SerializeField] private GameObject waterGeneratorPrefab;

    public enum State
    {
        standing,
        running,
        Cshoot
    }
    //cannot be changed by outsider class
    public State state { private set; get;} = State.standing;

    private void Run(Directions direction)
    {
        //animation
        animator.SetBool(aniRunning, true);
        hasLookPreference = true;

        //move
        switch (direction)
        {
            case Directions.right:
                //animation
                spriteRenderer.flipX = false;
                
                transform.position += new Vector3(runningSpeed, 0, 0);
                break;
            case Directions.left:
                spriteRenderer.flipX = true;

                transform.position += new Vector3(-runningSpeed, 0, 0);
                break;
        }
    }

    private void StartCshoot()
    {
        animator.SetTrigger(aniCshoot);
    }

    //called from animator
    private void Cshoot(int step)
    {
        float beaconingForce = 500.0f;

        Vector3 generatingPosition
                = new Vector3(this.transform.position.x,
                    this.transform.position.y + (feetY),
                    this.transform.position.z);
        GameObject generatorObject = null;
        GameObject beaconObject = null;
        
        //actual move
        switch (step)
        {
            //first shot
            case 1:
                generatorObject = GenerateWaterGenerator(
                    generatingPosition,
                    true,
                    30,
                    1.0f);
                beaconObject = GenerateWaterBeacon(generatingPosition, beaconingForce);
                beaconObject.GetComponent
                    <ElementParentBeacon>().Animate(ElementParent.Animation.logarithm);
                break;

            //second shot
            case 2:
                generatorObject = GenerateWaterGenerator(
                    generatingPosition,
                    true,
                    30,
                    1.0f);
                beaconObject = GenerateWaterBeacon(generatingPosition, beaconingForce);
                beaconObject.GetComponent
                    <ElementParentBeacon>().Animate(ElementParent.Animation.logarithm);
                break;

            //animation ended
            case 3:
                ChangeState(State.standing);
                break;
        }

        //set the elements' parents
        if(generatorObject != null)
        {
            generatorObject.GetComponent<ElementGenerator>().parentObject
                = beaconObject;
            ElementParentBeacon beacon = beaconObject.GetComponent<ElementParentBeacon>();
            beacon.projectionXSpeed = 10.0f;
            beacon.xMutiplier = 0.2f;
            beacon.yMutiplier = 30.0f;
            beacon.toRight = true;
        }
    }

    private GameObject GenerateWaterGenerator
                                (Vector3 globalPosition,
                                bool hasLife,
                                int lifeSpanF,
                                float generatePossibility)
    {
        GameObject targetObject = Instantiate(waterGeneratorPrefab,
            Vector3.zero,
            Quaternion.identity,
            prefabsParent.transform);
        targetObject.transform.position = globalPosition;
        ElementGenerator target = targetObject.GetComponent<ElementGenerator>();
        target.hasLife = hasLife;
        target.lifeLeftF = lifeSpanF;
        target.generatePossibility = generatePossibility;

        return targetObject;
    }

    private GameObject GenerateWaterBeacon
                                (Vector3 globalPosition,
                                float force)
    {
        GameObject targetObject = Instantiate(waterBeaconPrefab,
              Vector3.zero,
              Quaternion.identity,
              prefabsParent.transform);
        targetObject.transform.position = globalPosition;
        ElementParentBeacon target = targetObject.GetComponent<ElementParentBeacon>();
        target.beaconingForce = force;

        return targetObject;
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

    private bool CanAction()
    {
        return CanMove() && !isWaitingForAnotherKey;
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
                hasLookPreference = false;
                break;
        }

        state = nextState;
    }

    protected override void OnKeyDownC()
    {
        base.OnKeyDownC();

        //Cshoot
        if (CanAction())
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
