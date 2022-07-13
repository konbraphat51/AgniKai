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
    [SerializeField] private string aniJumping1 = "Jumping1";
    [SerializeField] private string aniJumping1State = "Jumping1State";

    [Header("Prefabs")]
    [SerializeField] private GameObject waterBeaconPrefab;
    [SerializeField] private GameObject waterGeneratorPrefab;
    [SerializeField] private GameObject waterBulletGeneratorPrefab;

    public enum State
    {
        standing,
        running,
        Cshoot,
        jumping1
    }
    //cannot be changed by outsider class
    public State state { private set; get;} = State.standing;

    private GameObject generatorAttached;

    private int detailState = 0;

    protected override void Update()
    {
        base.Update();

        switch (state)
        {
            case State.jumping1:
                UpdateJumping1();
                break;
        }
    }

    private void Run()
    {
        //animation
        animator.SetBool(aniRunning, true);
        hasLookPreference = true;

        //move
        switch (direction)
        {
            case Directions.right:
                //animation
                lookingAtRight = true;
                
                transform.position += new Vector3(runningSpeed, 0, 0);
                break;
            case Directions.left:
                lookingAtRight = false;

                transform.position += new Vector3(-runningSpeed, 0, 0);
                break;
        }
    }

    private void StartJumping1()
    {
        //consts
        float jumpingForce = 15f;
        
        //don't be front
        ChangeState(State.jumping1);

        //animation
        detailState = 1;
        animator.SetTrigger(aniJumping1);
        animator.SetInteger(aniJumping1State, detailState);

        //add force
        Vector3 jumpingVector = new Vector3(0, 1, 0) * jumpingForce;
        this.gameObject.GetComponent<Rigidbody2D>().AddForce(jumpingVector);

        //effect water element
        generatorAttached = GenerateWaterGenerator(
            this.transform.position,
            false,
            1,
            0.5f,
            WaterElementType.normal);
            //comew along
        generatorAttached.transform.parent = this.transform;
        generatorAttached.transform.localPosition
            = new Vector3(0, feetY, 0);
        ElementGenerator generator = generatorAttached.GetComponent<ElementGenerator>();
        generator.option = ElementGenerator.Option.spreadRandom;
        generator.parentObject = this.transform.parent.gameObject;
        generator.isElementHeavy = true;
    }

    private void Jumping1(int step)
    {
        switch (step)
        {
            case 1:
                //rising animation ended
                break;
        }
    }

    private void UpdateJumping1()
    {
        float horizontalForce = 0.05f;

        //animation
        float vy = this.GetComponent<Rigidbody2D>().velocity.y;
        if (detailState == 1 && vy < 0)
        {
            detailState = 2;
            animator.SetInteger(aniJumping1State, detailState);
            Destroy(generatorAttached);
        }
        else if(detailState == 2 && vy == 0)
        {
            EndJumping1();
        }
        //horizontal
        if (direction != Directions.none)
        {
            Rigidbody2D rigid = this.GetComponent<Rigidbody2D>();
            switch (direction)
            {
                case Directions.right:
                    rigid.AddForce(new Vector3(horizontalForce, 0, 0));
                    break;
                case Directions.left:
                    rigid.AddForce(new Vector3(-horizontalForce, 0, 0));
                    break;
            }
        }

    }

    private void EndJumping1()
    {
        //state
        ChangeState(State.standing);

        //animation
        animator.SetInteger(aniJumping1State, 0);

        //movement: stop the horizontal movement during jumping
        this.GetComponent<Rigidbody2D>().velocity
            = new Vector3(0, 0, 0);
    }

    private void StartCshoot()
    {
        animator.SetTrigger(aniCshoot);
        ChangeState(State.Cshoot);

        hasLookPreference = true;
    }

    //called from animator
    private void Cshoot(int step)
    {
        //consts
        float beaconingForce = 100.0f;
        int generatorLife = 30;
        float generatingRate = 2.0f;
        float projectionXSpeed = 7.0f;
        float xMutiplier = 0.2f;
        float yMutiplier = 30.0f;
        bool toRight = lookingAtRight;
        bool hasLife = true;
        int elementLifeLeft = 400;


        //where generator set
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
                    generatorLife,
                    generatingRate,
                    WaterElementType.bullet);
                beaconObject = GenerateWaterBeacon(generatingPosition, beaconingForce);
                beaconObject.GetComponent
                    <ElementParentBeacon>().Animate(ElementParent.Animation.logarithm);
                break;

            //second shot
            case 2:
                generatorObject = GenerateWaterGenerator(
                    generatingPosition,
                    true,
                    generatorLife,
                    generatingRate,
                    WaterElementType.bullet);
                beaconObject = GenerateWaterBeacon(generatingPosition, beaconingForce);
                beaconObject.GetComponent
                    <ElementParentBeacon>().Animate(ElementParent.Animation.logarithm);
                break;

            //animation ended
            case 3:
                ChangeState(State.standing);
                hasLookPreference = false;
                break;
        }

        //set the elements' parents
        if(generatorObject != null)
        {
            ElementGenerator generator = generatorObject.GetComponent<ElementGenerator>();
            generator.parentObject = beaconObject;
            ElementParentBeacon beacon = beaconObject.GetComponent<ElementParentBeacon>();
            beacon.projectionXSpeed = projectionXSpeed;
            beacon.xMutiplier = xMutiplier;
            beacon.yMutiplier = yMutiplier;
            beacon.toRight = toRight;
            beacon.hasLife = hasLife;
            beacon.lifeLeft = elementLifeLeft;
        }
    }

    private GameObject GenerateWaterGenerator
                                (Vector3 globalPosition,
                                bool hasLife,
                                int lifeSpanF,
                                float generatePossibility,
                                WaterElementType type)
    {
        GameObject targetObject = null;
        switch (type)
        {
            case WaterElementType.normal:
                targetObject = Instantiate(waterGeneratorPrefab,
                    Vector3.zero,
                    Quaternion.identity,
                    prefabsParent.transform);
                break;

            case WaterElementType.bullet:
                targetObject = Instantiate(waterBulletGeneratorPrefab,
                    Vector3.zero,
                    Quaternion.identity,
                    prefabsParent.transform);
                break;
        }
        
        targetObject.transform.position = globalPosition;
        ElementGenerator target = targetObject.GetComponent<ElementGenerator>();
        target.hasLife = hasLife;
        target.lifeLeftF = lifeSpanF;
        target.generatePossibility = generatePossibility;
        target.playerN = this.number;

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
        target.playerN = this.number;

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

    //for new actions/attack
    //(for not triggering isWaitingForAnotherKey
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
        }
    }

    protected override void OnKeyDownW()
    {
        base.OnKeyDownW();

        if (CanMove()) {
            StartJumping1();
        }
    }

    protected override void OnKeyPushingA()
    {
        base.OnKeyPushingA();

        //Run
        if (CanMove())
        {
            Run();
            ChangeState(State.running);
        }
    }

    protected override void OnKeyPushingD()
    {
        base.OnKeyPushingD();

        //Run
        if (CanMove())
        {
            Run();
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
