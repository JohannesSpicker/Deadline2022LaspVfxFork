using UnityEngine;

public class TunnelRotator : MonoBehaviour
{
    [SerializeField] private float     speed = 2f;
    private                  Transform myTransform;

    private void Awake()  => myTransform = transform;
    private void Update() => myTransform.rotation = Quaternion.AngleAxis(speed, Vector3.left) * myTransform.rotation;
}