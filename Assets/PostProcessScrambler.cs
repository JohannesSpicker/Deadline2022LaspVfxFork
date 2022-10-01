using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

[RequireComponent(typeof(Volume))]
public class PostProcessScrambler : MonoBehaviour
{
    private const            float  beatTime = 13.7f;
    [SerializeField] private Volume volume;

    private LensDistortion lensDistortion;

    private void Start()
    {
        volume.profile.TryGet(out lensDistortion);
        StartCoroutine(ScramblingRoutine());
    }

    private void OnValidate() => volume = GetComponent<Volume>();

    private IEnumerator ScramblingRoutine()
    {
        VolumeParameter<float> volumeParameter = new VolumeParameter<float>();
        volumeParameter.value = 0f;

        lensDistortion.intensity.SetValue(volumeParameter);

        yield return new WaitForSeconds(3f * beatTime);

        yield return new WaitForSeconds(beatTime * 1.5f);

        Ticker ticker = new Ticker(.5f * beatTime);

        while (!ticker.Tick(Time.deltaTime))
        {
            volumeParameter.value = Mathf.Clamp(-1 + ticker.Remaining / (.5f * beatTime), -1f, 0f);
            lensDistortion.intensity.SetValue(volumeParameter);

            yield return null;
        }

        ticker.Reset();

        while (!ticker.Tick(Time.deltaTime))
        {
            volumeParameter.value = Mathf.Clamp(-ticker.Remaining / (.5f * beatTime), -1f, 0f);
            lensDistortion.intensity.SetValue(volumeParameter);

            yield return null;
        }

        volumeParameter.value = 0f;
        lensDistortion.intensity.SetValue(volumeParameter);
    }

    public class Ticker
    {
        protected float counter;
        private   float duration;

        public Ticker(float duration) { this.duration = duration; }

        public float Remaining => duration - counter;

        public virtual bool Tick(float delta)
        {
            counter += delta;

            return duration <= counter;
        }

        public void Reset() => counter = 0;

        public void ChangeDuration(float newDuration) => duration = newDuration;
    }
}