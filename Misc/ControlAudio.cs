using Kitchen.Components;
using UnityEngine;

namespace Yipee.Misc
{
    public class ControlAudio : MonoBehaviour
    {
        public Animator animator;
        public SoundSource soundSource;

        private bool done = false;
        private void Update()
        {
            if (!animator.GetBool("ShouldSit") || done) return;
            soundSource.Play();
            done = true;
        }
    }
}