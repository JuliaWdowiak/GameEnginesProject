using System;
using UnityEngine;

namespace Assets.Scripts.Objects.Fader
{
    public class Fader : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        private const string FADER_PATH = "Fader/Fader";

        private Action _OnFadedInCallback;
        private Action _OnFadedOutCallback;

        private static Fader _instance;

        public bool IsFading { get; private set; }

        public static Fader instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<Fader>(FADER_PATH);
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance);
                }

                return _instance;
            }
        }

        public void FadeIn(Action onFadedInCallback)
        {
            Debug.Log("IsFading: " + IsFading);
            if (IsFading) return;

            IsFading = true;
            _OnFadedInCallback = onFadedInCallback;
            animator.SetBool("IsFaded", false);
        }

        public void FadeOut(Action onFadedOutCallback)
        {
            if (IsFading) return;

            IsFading = true;
            _OnFadedOutCallback = onFadedOutCallback;
            animator.SetBool("IsFaded", true);
        }

        private void Handle_FadeInEnded()
        {
            _OnFadedInCallback?.Invoke();
            _OnFadedInCallback = null;
            IsFading = false;
        }
        private void Handle_FadeOutEnded()
        {
            _OnFadedOutCallback?.Invoke();
            _OnFadedOutCallback = null;
            IsFading = false;
        }
    }
}
