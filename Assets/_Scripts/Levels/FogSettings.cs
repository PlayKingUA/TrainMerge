using UnityEngine;

public class FogSettings : MonoBehaviour
{
    [SerializeField] private Color fogColor;
    private void OnEnable()
    {
        RenderSettings.fogColor = fogColor;
    }
}
