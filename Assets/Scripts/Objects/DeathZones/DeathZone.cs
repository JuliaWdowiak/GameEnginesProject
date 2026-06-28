using Assets.Scripts.Objects.Actors;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var entity = collision.GetComponent<ActorController>();
        if (entity == null) return;
        entity.StateMachine.RegisterStateChange(ActorState.Dieing);
    }
}
