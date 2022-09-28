using UnityEngine;

namespace Code.AudioCode
{
    public class VibratingSphere : MonoBehaviour
    {
        [SerializeField, Range(0, 7)]
        private int index;

        private Transform myTransform;

        private void Awake()
        {
            myTransform = transform;

            AudioDataGrabber.ProvideSampleData += ReactToAudio;
        }

        private void OnDestroy() => AudioDataGrabber.ProvideSampleData -= ReactToAudio;

        private void ReactToAudio(AudioSampleData data) =>
            myTransform.localScale = 20f * data.normalizedBandBuffer[index] * Vector3.one;
    }
}