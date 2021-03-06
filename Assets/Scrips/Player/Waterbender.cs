using Common;
using UnityEngine;

namespace Game
{
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
        [SerializeField] private GameObject waterGeneratorPrefab;

        public enum State
        {
            standing,
            running,
            Cshoot,
            jumping1
        }
        //cannot be changed by outsider class
        public State state { private set; get; } = State.standing;

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
            gameObject.GetComponent<Rigidbody2D>().AddForce(jumpingVector);

            //effect water element
            ElementGeneratorSettings gs = new ElementGeneratorSettings()
            {
                elementSettings = new ElementSettings()
                {
                    rigid = true,
                    option = Element.Option.spreadRandom,
                    optionA = 1.5f,
                    lifeLeft = 150,
                    isTrigger = true
                },
                hasLife = false,
                generatePossibility = 0.5f
            };
            generatorAttached = MakeGenerator(
                                        waterGeneratorPrefab,
                                        new Vector3(0, 0, 0),
                                        gs,
                                        true);

            //come along
            generatorAttached.transform.parent = transform;
            generatorAttached.transform.localPosition = new Vector3(0, feetY, 0);
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
            float vy = GetComponent<Rigidbody2D>().velocity.y;
            if (detailState == 1 && vy < 0)
            {
                detailState = 2;
                animator.SetInteger(aniJumping1State, detailState);
                Destroy(generatorAttached);
            }
            else if (detailState == 2 && vy == 0)
            {
                EndJumping1();
            }
            //horizontal
            if (direction != Directions.none)
            {
                Rigidbody2D rigid = GetComponent<Rigidbody2D>();
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
            GetComponent<Rigidbody2D>().velocity
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
            int generatorLife = 30;
            float generatingRate = 2.0f;

            //where generator set
            Vector3 generatingPosition
                    = new Vector3(transform.position.x,
                        transform.position.y + feetY,
                        transform.position.z);

            ElementSettings es = new ElementSettings()
            {
                bullet = true,
                rigid = true,
                collidable = true,
                hasLife = false,
                toRight = lookingAtRight,
                orbit = Element.Orbit.waterCshoot,
                isTrigger = false,
                collisionIgnored = new GameObject[1] {
                    this.gameObject
                }
            };
            ElementGeneratorSettings gs = new ElementGeneratorSettings()
            {
                elementSettings = es,
                hasLife = true,
                lifeLeftF = generatorLife,
                generatePossibility = generatingRate
            };

            //actual move
            switch (step)
            {
                //first shot
                case 1:

                    MakeGenerator(waterGeneratorPrefab,
                                    generatingPosition,
                                    gs,
                                    false);
                    break;

                //second shot
                case 2:
                    MakeGenerator(waterGeneratorPrefab,
                                    generatingPosition,
                                    gs,
                                    false); break;

                //animation ended
                case 3:
                    ChangeState(State.standing);
                    hasLookPreference = false;
                    break;
            }
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

        private GameObject MakeGenerator(GameObject prefab,
                                         Vector3 position,
                                         ElementGeneratorSettings settings,
                                         bool isLocalPosition = true)
        {
            GameObject generatorObj = Instantiate(prefab, transform.parent);

            //setting position
            if (isLocalPosition)
            {
                generatorObj.transform.localPosition = position;
            }
            else
            {
                generatorObj.transform.position = position;
            }

            //settings
            if (settings.parentObject == null)
            {
                settings.parentObject = gameObject.transform.parent.gameObject;
            }
            settings.elementSettings.playerN = number;
            generatorObj.GetComponent<ElementGenerator>().settings = settings;

            return generatorObj;
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

            if (CanMove())
            {
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
            if (state == State.running)
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
}