using UnityEngine;

namespace Code.AudioCode
{
    public class VibratingSphere : MonoBehaviour
    {
        [SerializeField, Range(0, 7)]
        private int index;

        private Transform myTransform;

        private void Awake() => myTransform = transform;

        private void OnEnable()  => AudioDataGrabber.ProvideSampleData += ReactToAudio;
        private void OnDisable() => AudioDataGrabber.ProvideSampleData -= ReactToAudio;

        private void ReactToAudio(AudioSampleData data) => myTransform.localScale =
            Mathf.Min(10f * data.normalizedBandBuffer[index], 1.3f) * Vector3.one;
    }
}