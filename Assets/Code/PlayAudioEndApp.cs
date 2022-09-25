using System.Collections;
using TeppichsTools.Behavior;
using UnityEngine;

public class PlayAudioEndApp : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private void Awake() => audioSource.Play();
    private void Start() => StartCoroutine(QuitRoutine());

    private IEnumerator QuitRoutine()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);

        QuitHelper.Quit();
    }
}