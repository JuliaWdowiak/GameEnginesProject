using Assets.Scripts.Business.Lists;
using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Portals
{
    [RequireComponent(typeof(CapsuleCollider2D))]
    public class Portal : MonoBehaviour
    {
        private Action _onPlayerEnterPortal;

        public void Init(Action onPlayerEnterPortal)
        {
            _onPlayerEnterPortal = onPlayerEnterPortal;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("Portal::::" + collision);
            if (collision == null) return;

            if (collision.gameObject.tag == Tags.Player)
            {
                Debug.Log("Go To The Next Level");
                _onPlayerEnterPortal.Invoke();
            }
        }
    }
}
