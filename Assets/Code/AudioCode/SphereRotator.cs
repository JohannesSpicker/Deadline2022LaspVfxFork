using UnityEngine;

namespace Code.AudioCode
{
    public class SphereRotator : MonoBehaviour
    {
        [SerializeField, Range(0, 7)]
        private int index;

        [SerializeField] private bool goesRight;

        [SerializeField, Min(0f)]
        private float intensity = 1f;

        private float intensityFactor;

        private bool isRotating;

        private Transform myTransform;

        private void Awake()
        {
            myTransform     = transform;
            intensityFactor = (goesRight ? 1f : -1f) * intensity;
        }

        private void OnEnable()  => AudioDataGrabber.ProvideSampleData += ReactToAudio;
        private void OnDisable() => AudioDataGrabber.ProvideSampleData -= ReactToAudio;

        private void ReactToAudio(AudioSampleData data)
        {
            if (isRotating)
                myTransform.localRotation =
                    Quaternion.AngleAxis(intensityFactor * (data.normalizedBandBuffer[index] + 0.1f), Vector3.forward)
                    * myTransform.localRotation;
        }

        public void StartRotating() => isRotating = true;
    }
}