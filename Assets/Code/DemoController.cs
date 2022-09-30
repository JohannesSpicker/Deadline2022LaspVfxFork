using System.Collections;
using System.Collections.Generic;
using Code.AudioCode;
using UnityEngine;

public class DemoController : MonoBehaviour
{
    private const            float      beatTime = 13.7f;
    [SerializeField] private GameObject spheres;

    [SerializeField] private GameObject firstParticles;

    [SerializeField] private List<SphereRotator> sphereRotators = new List<SphereRotator>();

    private void Start() => StartCoroutine(DemoRoutine());

    private IEnumerator DemoRoutine()
    {
        float waitForRotation = beatTime;

        yield return new WaitForSeconds(waitForRotation);

        foreach (SphereRotator rotator in sphereRotators)
            rotator.StartRotating();

        //yield return new WaitForSeconds(41f - waitForRotation);
        yield return new WaitForSeconds(beatTime * 2f);

        spheres.SetActive(false);
        firstParticles.SetActive(true);
    }
}