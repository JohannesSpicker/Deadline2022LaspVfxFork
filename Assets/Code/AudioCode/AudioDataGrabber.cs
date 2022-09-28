using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioDataGrabber : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float audioProfileStartValue = 5f;

    private float amplitude, amplitudeBuffer, amplitudeHighest;

    private float[] bandBuffer     = new float[8];
    private float[] bufferDecrease = new float[8];
    private float[] frequencyBand  = new float[8];

    private float[] frequencyBandHighest    = new float[8];
    private float[] normalizedBandBuffer    = new float[8];
    private float[] normalizedFrequencyBand = new float[8];

    private float[] samples = new float[512];

    private void Start() => SetAudioProfile();

    private void Update()
    {
        GetSpectrumAudioSource();
        MakeFrequencyBands();
        MakeBandBuffer();
        MakeNormalizedBands();
        GetAmplitude();

        BroadcastData();
    }

    private void OnValidate() => audioSource = GetComponent<AudioSource>();

    private void SetAudioProfile()
    {
        for (int i = 0; i < 8; i++)
            frequencyBandHighest[i] = audioProfileStartValue;
    }

    public static event Action<AudioSampleData> ProvideSampleData;

    private void GetSpectrumAudioSource() => audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

    private void BroadcastData() => ProvideSampleData?.Invoke(new AudioSampleData
    {
        bandBuffer              = bandBuffer,
        bufferDecrease          = bufferDecrease,
        frequencyBand           = frequencyBand,
        frequencyBandHighest    = frequencyBandHighest,
        normalizedBandBuffer    = normalizedBandBuffer,
        normalizedFrequencyBand = normalizedFrequencyBand,
        samples                 = samples
    });

    private void MakeFrequencyBands()
    {
        int count = 0;

        for (int i = 0; i < 8; i++)
        {
            int   sampleCount = (int)Mathf.Pow(2, i + 1);
            float average     = 0f;

            for (int j = 0; j < sampleCount; j++)
                average += samples[count] * ++count;

            average /= count;

            frequencyBand[i] = average * 10;
        }
    }

    private void MakeBandBuffer()
    {
        for (int i = 0; i < 8; i++)
        {
            if (bandBuffer[i] < frequencyBand[i])
            {
                bandBuffer[i]     = frequencyBand[i];
                bufferDecrease[i] = 0.005f;
            }

            if (frequencyBand[i] < bandBuffer[i])
            {
                bandBuffer[i]     -= bufferDecrease[i];
                bufferDecrease[i] *= 1.05f;
            }
        }
    }

    private void MakeNormalizedBands()
    {
        for (int i = 0; i < 8; i++)
        {
            if (frequencyBandHighest[i] < frequencyBand[i])
                frequencyBandHighest[i] = frequencyBand[i];

            normalizedFrequencyBand[i] = frequencyBand[i] / frequencyBandHighest[i]; //careful with 0
            normalizedBandBuffer[i]    = bandBuffer[i]    / frequencyBandHighest[i]; //careful with 0
        }
    }

    private void GetAmplitude()
    {
        float currentAmplitude       = 0f;
        float currentAmplitudeBuffer = 0f;

        for (int i = 0; i < 8; i++)
        {
            currentAmplitude       += normalizedFrequencyBand[i];
            currentAmplitudeBuffer += normalizedBandBuffer[i];
        }

        if (amplitudeHighest < currentAmplitude)
            amplitudeHighest = currentAmplitude;

        amplitude       = currentAmplitude       / amplitudeHighest;
        amplitudeBuffer = currentAmplitudeBuffer / amplitudeHighest;
    }
}