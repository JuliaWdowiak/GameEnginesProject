using UnityEngine;

namespace Assets.Scripts.Objects.Actors
{
    public enum ActorState { 
        None, Running, Standing, Dashing,
        Jumping, Accelerating, StoppingJump, Decellerating, Falling, Landing,
        StartingAttack, Attacking, FinishedAttacking, TakingDamage, TookDamage,
        Interacting, FinishedInteracting, Dieing
    }
    public enum LookDirection { Left=-1, None=0, Right=1 }

    public class ActorStateMachine
    {
        private ActorState _registeredState;
        private LookDirection _registeredDirection;

        public ActorState CurrentState { get; private set; }
        public ActorState LastState { get; private set; }

        public LookDirection CurrentDirection { get; private set; }
        public LookDirection LastDirection { get; private set; }

        public string name = "";


        public void RegisterStateChange(ActorState state)
        {            
            if (_registeredState == state || CurrentState == state) return;
            _registeredState = state;
            UpdateState();
        }
        public void RegisterStateChange(LookDirection lookDirection)
        {
            if (lookDirection == LookDirection.None) return;
            _registeredDirection = lookDirection;
            UpdateState();
        }
        public void UpdateState()
        {
            if (CanBeApplied(_registeredState)) ChangeState(_registeredState);
            if (CanBeApplied(_registeredDirection)) ChangeDirection(_registeredDirection);
        }

        private bool CanBeApplied(ActorState actorState)
        {
            if (actorState == ActorState.None) return false;
            if (CurrentState == ActorState.Dieing) return false;
            if (actorState == ActorState.Dieing) return true;

            if (actorState == ActorState.StoppingJump) Debug.Log(actorState);
            if (CurrentState == ActorState.Jumping && actorState != ActorState.Accelerating) return false;
            if (CurrentState == ActorState.Accelerating && actorState != ActorState.StoppingJump
                && actorState != ActorState.Decellerating && actorState != ActorState.Falling) return false;
            if (CurrentState != ActorState.Accelerating && actorState == ActorState.StoppingJump) return false;

            if (CurrentState == ActorState.Falling && actorState != ActorState.Landing) return false;
            if (CurrentState == ActorState.Landing && actorState != ActorState.Standing && actorState != ActorState.Running) return false;
            if (CurrentState == ActorState.StoppingJump && actorState != ActorState.Decellerating) return false;
            if (CurrentState == ActorState.Decellerating && actorState != ActorState.Falling) return false;

            if (CurrentState == ActorState.StartingAttack && actorState != ActorState.Attacking) return false;
            if (CurrentState == ActorState.Attacking && actorState != ActorState.FinishedAttacking) return false;
            if (CurrentState == ActorState.FinishedAttacking && actorState != ActorState.Standing) return false;

            if (CurrentState == ActorState.TakingDamage && actorState != ActorState.TookDamage) return false;
            if (CurrentState == ActorState.TookDamage && actorState != ActorState.Standing) return false;

            if (CurrentState == ActorState.Interacting && actorState != ActorState.FinishedInteracting) return false;
            if (CurrentState == ActorState.FinishedInteracting && actorState != ActorState.Standing) return false;

                Debug.Log(name + " State: " + actorState);
            return true;
        }
        private bool CanBeApplied(LookDirection direction)
        {
            if (CurrentState == ActorState.Dashing) return false;
            if (direction == LookDirection.None) return false;
            return true;
        }

        private void ChangeState(ActorState state)
        {
            LastState = CurrentState;
            CurrentState = state;
            _registeredState = ActorState.None;
        }
        private void ChangeDirection(LookDirection lookDirection)
        {
            LastDirection = lookDirection;
            CurrentDirection = lookDirection;
            _registeredDirection = LookDirection.None;
        }
    }
}
