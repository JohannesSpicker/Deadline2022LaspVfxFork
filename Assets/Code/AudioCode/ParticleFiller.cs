using UnityEngine;
using UnityEngine.VFX;

namespace Code.AudioCode
{
    public class ParticleFiller : MonoBehaviour
    {
        [SerializeField] private VisualEffect visualEffect;

        [SerializeField, Range(0, 7)]
        private int index;

        private void Awake()      => AudioDataGrabber.ProvideSampleData += ReactToAudio;
        private void OnDestroy()  => AudioDataGrabber.ProvideSampleData -= ReactToAudio;
        private void OnValidate() => visualEffect = GetComponent<VisualEffect>();

        private void ReactToAudio(AudioSampleData data)
        {
            visualEffect.SetFloat("Amplitude", data.normalizedBandBuffer[index]);
            visualEffect.SetFloat("Radius", data.normalizedBandBuffer[index]);
            
        }
    }
}