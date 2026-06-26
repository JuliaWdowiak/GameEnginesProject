using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Spawners
{
    public class SpawnPoint: MonoBehaviour
    {
        private Action<SpawnPoint> _onActivate;
        public bool IsInitial;
        public bool IsLockable;
        public GameObject TargetRoom;
        public Vector3 Position;

        private void Start()
        {
            Position = transform.position;
        }

        public void SetActivationAction(Action<SpawnPoint> onActivate)
        {
            _onActivate = onActivate;
        }

        public void Activate()
        {
            _onActivate(this.gameObject.GetComponent<SpawnPoint>());
        }
    }
}
