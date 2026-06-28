using Assets.Scripts.Business.Lists;
using System;
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEditor.U2D.Animation;
using UnityEngine;

namespace Assets.Scripts.Objects.Actors
{
    public static class ActionMachine
    {
        public static void Move(ActorController actor)
        {
            actor.RB.linearVelocity = new Vector2 ((int)actor.StateMachine.CurrentDirection * actor.Speed, actor.RB.linearVelocity.y);
        }

        public static void Stop(ActorController actor)
        {
            actor.RB.linearVelocity = Vector2.zero;
        }

        public static void Jump(ActorController actor)
        {
            actor.StateMachine.RegisterStateChange(ActorState.Accelerating);

            int sleepTime = (int)(Time.fixedDeltaTime * 1000);
            actor.RB.linearVelocity = new Vector2(actor.RB.linearVelocity.x, actor.JumpForce);
        }

        public static void StopJump(ActorController actor)
        {
            actor.StateMachine.RegisterStateChange(ActorState.Decellerating);
            actor.RB.linearVelocity = new Vector2(actor.RB.linearVelocity.x, actor.RB.linearVelocity.y * 0.3f);
        }

        public static void Attack(ActorController actor)
        {
            actor.StateMachine.RegisterStateChange(ActorState.Attacking);
            GameObject attackCollider = actor.gameObject.transform.GetChild(0).gameObject;
            actor.StartCoroutine(AttackMovement(
                actor.AttackInitialPoint, actor.TicksToStart,
                actor.AttackMovementVectors, actor.AttackMovementTicks,
                actor.AttackSpeed, attackCollider,
                () => { actor.StateMachine.RegisterStateChange(ActorState.FinishedAttacking); }
                ));
        }
        private static IEnumerator AttackMovement(
            Vector2 startPoint, int ticksToStart,
            Vector2[] movementVectors, int[] ticks,
            float[] attackSpeed, GameObject attackCollider, 
            Action onEnd
            )
        {
            yield return new WaitForFixedUpdate();
            while (ticksToStart > 0)
            {
                ticksToStart--;
                yield return new WaitForFixedUpdate();
            }

            attackCollider.transform.localPosition = startPoint;
            attackCollider.SetActive(true);

            for (int i = 0; i < ticks.Length; i++)
            {
                int tick = ticks[i];
                Vector3 distance = (Vector3)(movementVectors[i] * attackSpeed[i] * Time.fixedDeltaTime);

                while (tick > 0)
                {
                    tick--;
                    attackCollider.transform.position += distance;
                    yield return new WaitForFixedUpdate();
                }
            }

            attackCollider.SetActive(false);
            onEnd.Invoke();
            yield return null;
        }

        public static async void TakeDamage(ActorController actor, float damage)
        {
            actor.StateMachine.RegisterStateChange(ActorState.TakingDamage);
            actor.SetDamage(actor.CurrentDamage + damage);

            Debug.Log(actor.name + ": " + actor.CurrentDamage + ", state: " + actor.StateMachine.CurrentState);

            if (actor.CurrentDamage < actor.MaxHp)
            {

                await Task.Delay(1000);

                actor.StateMachine.RegisterStateChange(ActorState.TookDamage);
                actor.StateMachine.RegisterStateChange(ActorState.Standing);
            }
            else
            {
                if (actor.gameObject.tag == Tags.Player) actor.OnDeath();
                else actor.StateMachine.RegisterStateChange(ActorState.Dieing);
            }
        }
    }
}
