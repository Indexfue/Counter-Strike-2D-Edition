using UnityEngine;

namespace Player
{
    public class CameraShakeEventArgs : BaseEventArgs
    {
        public float AmplitudeGain { get; }
        public float FrequencyGain { get; }
        public float ShakeTime { get; }
        public float StopShakeTime { get; }

        public CameraShakeEventArgs(GameObject sender, float amplitudeGain, float frequencyGain, float shakeTime,
            float stopShakeTime = 1f) : base(sender)
        {
            AmplitudeGain = amplitudeGain;
            FrequencyGain = frequencyGain;
            ShakeTime = shakeTime;
            StopShakeTime = stopShakeTime;
        }
    }
}