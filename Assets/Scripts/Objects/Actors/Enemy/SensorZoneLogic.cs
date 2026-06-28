using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Actors.Enemy
{
    internal class SensorZoneLogic: MonoBehaviour
    {
        private Action<Collider2D> _onCollision;
        private Action<Collider2D> _onZoneLeft;

        public void Init(Action<Collider2D> onCollision, Action<Collider2D> onZoneLeft)
        {
            _onCollision = onCollision;
            _onZoneLeft = onZoneLeft;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _onCollision(collision);
        }
        private void OnTriggerExit2D(Collider2D collision)
        {
            _onZoneLeft(collision);
        }
    }
}
