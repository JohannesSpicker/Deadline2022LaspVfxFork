using System.Collections;
using System.Collections.Generic;
using Code.AudioCode;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    private const            float      beatTime = 13.7f;
    [SerializeField] private GameObject spheres;

    [SerializeField] private GameObject firstParticles;
    [SerializeField] private GameObject tohuwabohuParticles;

    [SerializeField] private List<SphereRotator> sphereRotators = new List<SphereRotator>();

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        StartCoroutine(DemoRoutine());
        StartCoroutine(QuitRoutine());
    }

    private IEnumerator DemoRoutine()
    {
        audioSource.Play();

        float waitForRotation = beatTime;

        yield return new WaitForSeconds(waitForRotation);

        foreach (SphereRotator rotator in sphereRotators)
            rotator.StartRotating();

        //yield return new WaitForSeconds(41f - waitForRotation);
        yield return new WaitForSeconds(beatTime * 2f);

        spheres.SetActive(false);
        firstParticles.SetActive(true);

        while (Application.isPlaying)
        {
            yield return new WaitForSeconds(beatTime * 2f);

            firstParticles.SetActive(false);
            tohuwabohuParticles.SetActive(true);

            yield return new WaitForSeconds(beatTime * 2f);

            firstParticles.SetActive(true);
            tohuwabohuParticles.SetActive(false);
        }
    }

    private IEnumerator QuitRoutine()
    {
        yield return new WaitUntil(() => !audioSource.isPlaying);

        QuitHelper.Quit();
    }
}