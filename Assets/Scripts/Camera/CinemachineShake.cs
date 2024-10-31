using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set; }
    private CinemachineVirtualCamera vc;
    private float shakeTimer;
    private float startingIntensity;
    private float shakeTimerTotal;
    private void Awake()
    {
        Instance = this;
        vc = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin bmcp = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        bmcp.m_AmplitudeGain = intensity;
        shakeTimer = time;
        shakeTimerTotal = time;
        startingIntensity = intensity;
    }

    private void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            CinemachineBasicMultiChannelPerlin bmcp = vc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            bmcp.m_AmplitudeGain = Mathf.Lerp(startingIntensity, 0f, (1 - (shakeTimer / shakeTimerTotal)));
        }
    }
}
