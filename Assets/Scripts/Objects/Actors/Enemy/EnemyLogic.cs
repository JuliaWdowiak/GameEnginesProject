using Assets.Scripts.Business.Lists;
using System;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Objects.Actors.Enemy
{
    [RequireComponent(typeof(ActorController))]
    public class EnemyLogic : MonoBehaviour
    {
        private ActorController _self;
        private bool _isSeingEnemy = false;
        private bool _isEnemyInAttackZone = false;
        private Transform _enemy;
        public int ID { get; private set; }

        [SerializeField] private GameObject _leftmostPatrolPoint;
        [SerializeField] private GameObject _rightmostPatrolPoint;

        private void Start()
        {
            _self = GetComponent<ActorController>();

            transform.GetChild(1).GetComponent<SensorZoneLogic>().Init(OnSensorzoneEnter, OnSensorzoneLeave);
            transform.GetChild(2).GetComponent<SensorZoneLogic>().Init(OnSensorzoneEnter, OnSensorzoneLeave);
            transform.GetChild(3).GetComponent<SensorZoneLogic>().Init(OnAttackzoneEnter, OnAttackzoneExit);
        }

        private void Update()
        {
            if (_self.StateMachine.CurrentState != ActorState.Standing && _self.StateMachine.CurrentState != ActorState.Running)
                _self.StateMachine.RegisterStateChange(ActorState.Standing);
            if (_self.StateMachine.CurrentState == ActorState.Standing && !_isSeingEnemy) OnEnemyLost();
            else if (_self.StateMachine.CurrentState == ActorState.Standing && !_isEnemyInAttackZone) GoToEnemy();
            else if (_self.StateMachine.CurrentState == ActorState.Standing) AttackEnemy();
        }

        private void OnEnemyLost()
        {
            _isSeingEnemy = false;
            _enemy = null;

            if ((_leftmostPatrolPoint.transform.position - transform.position).x < 0) _self.StateMachine.RegisterStateChange(LookDirection.Left);
            else _self.StateMachine.RegisterStateChange(LookDirection.Right);

            _self.StateMachine.RegisterStateChange(ActorState.Running);
        }

        private void OnSensorzoneEnter(Collider2D collision)
        {
            if (collision.gameObject.tag != Tags.Player) return;

            _isSeingEnemy = true;
            _enemy = collision.transform;
            GoToEnemy();
        }
        private void GoToEnemy()
        {
            if ((_enemy.position - transform.position).x < 0) _self.StateMachine.RegisterStateChange(LookDirection.Left);
            else _self.StateMachine.RegisterStateChange(LookDirection.Right);
            _self.StateMachine.RegisterStateChange(ActorState.Running);
        }
        private void OnSensorzoneLeave(Collider2D collision)
        {
            if (collision.gameObject.tag != Tags.Player) return;
            OnEnemyLost();
        }

        private void OnAttackzoneEnter(Collider2D collision)
        {
            if (collision.gameObject.tag != Tags.Player) return;
            _isEnemyInAttackZone = true;
            AttackEnemy();
        }
        private void OnAttackzoneExit(Collider2D collision)
        {
            if (collision.gameObject.tag != Tags.Player) return;
            _isEnemyInAttackZone = false;
        }

        private void AttackEnemy()
        {
            _self.StateMachine.RegisterStateChange(ActorState.StartingAttack);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision == null ) return;

            if (collision.tag == Tags.LeftmostPoint && !_isSeingEnemy) _self.StateMachine.RegisterStateChange(LookDirection.Right);
            else if (collision.tag == Tags.RightmostPoint && !_isSeingEnemy) _self.StateMachine.RegisterStateChange(LookDirection.Left);
        }

        public void SetID(int id)
        {
            ID = id;
        }
    }
}