using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Player.GUI
{
    public class CameraShake : MonoBehaviour
    {
        private IEnumerator _coroutine;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private void Awake()
        {
            _cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        }

        public void ShakeCamera(float amplitudeGain, float frequencyGain, float shakeTime, float stopShakeTime = 1f)
        {
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = amplitudeGain;
            cinemachineBasicMultiChannelPerlin.m_FrequencyGain = frequencyGain;

            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StopShakingAfterTime(stopShakeTime);
            StartCoroutine(StopShakingAfterTime(stopShakeTime));
        }

        private void SubscribeEvent()
        {
            
        }

        private IEnumerator StopShakingAfterTime(float stopShakeTime = 1f)
        {
            float smoothStepCount = 100f;
            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = _cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            float currentAmplitude = cinemachineBasicMultiChannelPerlin.m_AmplitudeGain;
            float currentFrequency = cinemachineBasicMultiChannelPerlin.m_FrequencyGain;

            for (int i = 0; i < smoothStepCount; i++)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(currentAmplitude, 0, Mathf.Pow(i / smoothStepCount, stopShakeTime));
                cinemachineBasicMultiChannelPerlin.m_FrequencyGain = Mathf.Lerp(currentFrequency, 0, Mathf.Pow(i / smoothStepCount, stopShakeTime));
                yield return new WaitForSeconds(stopShakeTime / smoothStepCount);
            }
        }
    }
}