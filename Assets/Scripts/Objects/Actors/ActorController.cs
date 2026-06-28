using Assets.Scripts.Business.Lists;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Actors
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class ActorController : MonoBehaviour
    {
        public ActorStateMachine StateMachine { get; private set; } = new();


        public Rigidbody2D RB { get; private set; }
        public float Speed { get; private set; }
        public float JumpForce { get; private set; }

        #region AttackData
        public Vector2 AttackInitialPoint { get; private set; }
        public int TicksToStart { get; private set; }
        public Vector2[] AttackMovementVectors { get; private set; }
        public int[] AttackMovementTicks { get; private set; }
        public float[] AttackSpeed { get; private set; }
        #endregion

        public float MaxHp { get; private set; }
        public float CurrentDamage { get; private set; }
        public float DealingDamage { get; private set; }


        private bool wasInitialised = false;
        public Vector3 SpawnPosition { get; private set; }
        public Action OnDeath { get; internal set; }

        public void Init(Vector3 position)
        {
            Debug.Log(gameObject.name + " Was Initialized");
            wasInitialised = true;
            SpawnPosition = position;

            RB = GetComponent<Rigidbody2D>();
            Speed = 7f;
            JumpForce = 10f;

            AttackInitialPoint = new Vector2(0.93f, 0.92f);
            TicksToStart = 10;
            AttackMovementVectors = new Vector2[] { new Vector2(0, -1f), new Vector2(1f, 1f) };
            AttackMovementTicks = new int[] { 10, 5 };
            AttackSpeed = new float[] { 12f, 16f };

            MaxHp = 100;
            CurrentDamage = 0;
            DealingDamage = 10;
        }

        private void Update()
        {
            if (!wasInitialised) return;

            if (StateMachine.CurrentState == ActorState.Running) ActionMachine.Move(this);
            else if (StateMachine.CurrentState == ActorState.Standing) ActionMachine.Stop(this);
            else if (StateMachine.CurrentState == ActorState.Jumping) ActionMachine.Jump(this);
            else if (StateMachine.CurrentState == ActorState.StoppingJump) ActionMachine.StopJump(this);
            else if (StateMachine.CurrentState == ActorState.StartingAttack) ActionMachine.Attack(this);
            else if (StateMachine.CurrentState == ActorState.Dieing) Destroy(gameObject);

            CheckOnFall();
        }

        private void CheckOnFall()
        {
            if (RB.linearVelocity.y <= -0.1f)
            {
                StateMachine.RegisterStateChange(ActorState.Falling);
            }
        }

        public void SetDamage(float damage)
        {
            CurrentDamage = damage;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision == null) return;
            if (collision.gameObject.layer == Layers.Ground && StateMachine.CurrentState == ActorState.Falling)
            {
                StateMachine.RegisterStateChange(ActorState.Landing);
                StateMachine.RegisterStateChange(ActorState.Standing);
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetEntityId() != transform.GetChild(0).gameObject.GetEntityId() && collision.gameObject.tag != Tags.AttackCollider)
            {
                Debug.Log(collision.gameObject.name);
                ActionMachine.TakeDamage(collision.gameObject.GetComponent<ActorController>(), DealingDamage);
            }
        }

        public void Respawn()
        {
            transform.position = SpawnPosition;
        }
    }
}
