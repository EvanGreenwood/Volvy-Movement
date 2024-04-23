using Cinemachine;
using Framework;
using System.Collections;
using UnityEngine;

public class PlayerCameraManager : SingletonBehaviour<PlayerCameraManager>
{
    [SerializeField] private CinemachineVirtualCamera _vcam;
    private CinemachineBasicMultiChannelPerlin _noise;

    protected override void Awake()
    {
        base.Awake();

        _noise = _vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public IEnumerator ShakeRoutine(float shakeIntensity = 25f, float shakeTime = 0.5f)
    {
        _noise.ReSeed();

        SetNoise(1, shakeIntensity);
        yield return new WaitForSeconds(shakeTime);
        SetNoise(0, 0);
    }

    private void SetNoise(float amplitudeGain, float frequencyGain)
    {
        _noise.m_AmplitudeGain = amplitudeGain;
        _noise.m_FrequencyGain = frequencyGain;
    }
}