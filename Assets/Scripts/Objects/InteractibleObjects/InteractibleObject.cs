using UnityEngine;

namespace Assets.Scripts.Objects.InteractibleObjects
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class InteractibleObject: MonoBehaviour
    {
        public int ObjectID = 1;
        [SerializeField] private GameObject Hint;
        [SerializeField] private GameObject PortalToNextLevel;

        public void ShowHint() => Hint.SetActive(true);
        public void HideHint() => Hint.SetActive(false);
        public void OnBeingPicked()
        {
            PortalToNextLevel.SetActive(true);
            Destroy(gameObject);
        }
    }
}
