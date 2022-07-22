using System;
using Common;
using UnityEngine;
using UnityEngine.Events;

namespace Game
{

    //sprites always must look right
    public class Player : MonoBehaviour
    {
        [SerializeField] private int __number = 1;
        public int number { get; private set; }

        public float hp { get; protected set; } = 1000;

        public bool shouldLookRight = true;
        protected bool hasLookPreference = false;
        protected bool lookingAtRight = false;

        //w,a,d,s, c,v,b,f,g
        private int KeyN = 9;
        protected bool[] keyPressed = new bool[9];
        protected enum Keys
        {
            W = 0,
            A = 1,
            D = 2,
            S = 3,
            C = 4,
            V = 5,
            B = 6,
            F = 7,
            G = 8
        }
        //frames waited for another key until move conducted
        [SerializeField] private int waitForAnotherKeyF = 7;
        private UnityAction actionIfNothing;
        private UnityAction<Keys> keyReception;     //called when another key
        private int frameCountDown = 0;
        //if true, don't conduct another action independently
        protected bool isWaitingForAnotherKey { get; private set; } = false;

        //parent for objects this player instantiate (ex.element generator/parent)
        [SerializeField] protected GameObject prefabsParent;

        protected Animator animator;
        protected SpriteRenderer spriteRenderer;

        protected Directions direction;

        protected virtual void Start()
        {
            number = __number;

            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        protected virtual void Update()
        {
            direction = Directions.none;

            CheckKeys();
            Look();
            WaitForAnotherKey();
        }

        public void Damage(float damage)
        {
            hp -= damage;
        }

        private void CheckKeys()
        {
            foreach (Keys k in Enum.GetValues(typeof(Keys)))
            {
                bool former = keyPressed[(int)k];
                bool current = Input.GetKey(GetKeyCode(k));
                if (former == false && current == true)
                {
                    //The moment down
                    OnKeyDown(k);
                    keyPressed[(int)k] = current;
                }
                else if (former == true && current == true)
                {
                    //keep pushing
                    OnKeyPushing(k);
                }
                else if (former == true && current == false)
                {
                    //The moment up
                    OnKeyUp(k);
                    keyPressed[(int)k] = current;
                }
            }
        }

        //look at the opponent (this.shouldLookRight decided by BattleManager)
        private void Look()
        {
            if (!hasLookPreference)
            {
                lookingAtRight = shouldLookRight;
            }
            spriteRenderer.flipX = !lookingAtRight;
        }

        //wait before taking actions for another key input
        protected void StartWaitForAnotherKey(UnityAction actionIfNothing, UnityAction<Keys> keyReception)
        {
            //remember the action when no key input else
            this.actionIfNothing = actionIfNothing;

            this.keyReception = keyReception;

            //count down until action start
            frameCountDown = waitForAnotherKeyF;

            //don't conduct another action independently
            isWaitingForAnotherKey = true;
        }

        private void WaitForAnotherKey()
        {
            if (!isWaitingForAnotherKey) return;

            frameCountDown--;

            if (frameCountDown == 0)
            {
                isWaitingForAnotherKey = false;
            }
        }

        private void OnAnotherKey(Keys k)
        {
            keyReception.Invoke(k);
        }

        private void OnKeyDown(Keys k)
        {
            switch (k)
            {
                case Keys.W:
                    OnKeyDownW();
                    break;
                case Keys.A:
                    OnKeyDownA();
                    break;
                case Keys.D:
                    OnKeyDownD();
                    break;
                case Keys.S:
                    OnKeyDownS();
                    break;
                case Keys.C:
                    OnKeyDownC();
                    break;
                case Keys.V:
                    OnKeyDownV();
                    break;
                case Keys.B:
                    OnKeyDownB();
                    break;
                case Keys.F:
                    OnKeyDownF();
                    break;
                case Keys.G:
                    OnKeyDownG();
                    break;
            }
        }
        private void OnKeyPushing(Keys k)
        {
            if (isWaitingForAnotherKey)
            {
                OnAnotherKey(k);
            }

            switch (k)
            {
                case Keys.W:
                    OnKeyPushingW();
                    break;
                case Keys.A:
                    OnKeyPushingA();
                    break;
                case Keys.D:
                    OnKeyPushingD();
                    break;
                case Keys.S:
                    OnKeyPushingS();
                    break;
                case Keys.C:
                    OnKeyPushingC();
                    break;
                case Keys.V:
                    OnKeyPushingV();
                    break;
                case Keys.B:
                    OnKeyPushingB();
                    break;
                case Keys.F:
                    OnKeyPushingF();
                    break;
                case Keys.G:
                    OnKeyPushingG();
                    break;
            }
        }
        private void OnKeyUp(Keys k)
        {
            switch (k)
            {
                case Keys.W:
                    OnKeyUpW();
                    break;
                case Keys.A:
                    OnKeyUpA();
                    break;
                case Keys.D:
                    OnKeyUpD();
                    break;
                case Keys.S:
                    OnKeyUpS();
                    break;
                case Keys.C:
                    OnKeyUpC();
                    break;
                case Keys.V:
                    OnKeyUpV();
                    break;
                case Keys.B:
                    OnKeyUpB();
                    break;
                case Keys.F:
                    OnKeyUpF();
                    break;
                case Keys.G:
                    OnKeyUpG();
                    break;
            }
        }

        protected KeyCode GetKeyCode(Keys k)
        {
            switch (number)
            {
                case 1:
                    switch (k)
                    {
                        case Keys.W:
                            return KeyCode.W;
                        case Keys.A:
                            return KeyCode.A;
                        case Keys.D:
                            return KeyCode.D;
                        case Keys.S:
                            return KeyCode.S;
                        case Keys.C:
                            return KeyCode.C;
                        case Keys.V:
                            return KeyCode.V;
                        case Keys.B:
                            return KeyCode.B;
                        case Keys.F:
                            return KeyCode.F;
                        case Keys.G:
                            return KeyCode.G;
                    }
                    break;
                case 2:
                    switch (k)
                    {
                        case Keys.W:
                            return KeyCode.O;
                        case Keys.A:
                            return KeyCode.K;
                        case Keys.D:
                            return KeyCode.Equals;
                        case Keys.S:
                            return KeyCode.L;
                        case Keys.C:
                            return KeyCode.Slash;
                        case Keys.V:
                            return KeyCode.Backslash;
                        case Keys.B:
                            return KeyCode.RightShift;
                        case Keys.F:
                            return KeyCode.Semicolon;
                        case Keys.G:
                            return KeyCode.RightBracket;
                    }
                    break;
            }

            Debug.LogWarning("Unwanted keycode");
            return 0;
        }

        private void CheckKey(bool[] keys)
        {

        }

        protected virtual void OnKeyDownW() { }
        protected virtual void OnKeyDownA() { }
        protected virtual void OnKeyDownD() { }
        protected virtual void OnKeyDownS() { }
        protected virtual void OnKeyDownC() { }
        protected virtual void OnKeyDownV() { }
        protected virtual void OnKeyDownB() { }
        protected virtual void OnKeyDownF() { }
        protected virtual void OnKeyDownG() { }

        protected virtual void OnKeyPushingW() { }
        protected virtual void OnKeyPushingA()
        {
            direction = Directions.left;
        }
        protected virtual void OnKeyPushingD()
        {
            direction = Directions.right;
        }
        protected virtual void OnKeyPushingS() { }
        protected virtual void OnKeyPushingC() { }
        protected virtual void OnKeyPushingV() { }
        protected virtual void OnKeyPushingB() { }
        protected virtual void OnKeyPushingF() { }
        protected virtual void OnKeyPushingG() { }

        protected virtual void OnKeyUpW() { }
        protected virtual void OnKeyUpA() { }
        protected virtual void OnKeyUpD() { }
        protected virtual void OnKeyUpS() { }
        protected virtual void OnKeyUpC() { }
        protected virtual void OnKeyUpV() { }
        protected virtual void OnKeyUpB() { }
        protected virtual void OnKeyUpF() { }
        protected virtual void OnKeyUpG() { }
    }
}