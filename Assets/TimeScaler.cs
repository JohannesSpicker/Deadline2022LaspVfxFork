using UnityEngine;

public class TimeScaler : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField, Range(.01f, 20f)]
    private float timeScale = 1;

    private void Update() => Time.timeScale = timeScale;
#endif
}