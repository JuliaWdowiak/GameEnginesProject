using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Actors
{
    public class AttackColliderLogic : MonoBehaviour
    {
        private Action<Collider2D> _onCollision;

        public void Init(Action<Collider2D> onCollision)
        {
            _onCollision = onCollision;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            _onCollision(collision);
        }
    }
}
