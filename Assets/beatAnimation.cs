using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing; 
public class beatAnimation : MonoBehaviour
{
    public float beatValue;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;

    public BeatDetection beatDetection;
    public BeatDetection beatDetection2;
    public BeatDetection beatDetection3;

    private Bloom bloom;

    private void Start()
    {
        PostProcessVolume volume = GetComponent<PostProcessVolume>();
        volume.profile.TryGetSettings(out bloom);
    }

    private void Update()
    {
        // Modify the bloom intensity based on the beat value
        if (beatDetection.beatValue > beatDetection2.beatValue && beatDetection.beatValue > beatDetection3.beatValue)
        {
            beatValue = beatDetection.beatValue;
        }
        else if (beatDetection2.beatValue > beatDetection.beatValue && beatDetection2.beatValue > beatDetection3.beatValue)
        {
            beatValue = beatDetection2.beatValue;
        }
        else if (beatDetection3.beatValue > beatDetection.beatValue && beatDetection3.beatValue > beatDetection2.beatValue)
        {
            beatValue = beatDetection3.beatValue;
        }
        else
        {
            beatValue = beatDetection.beatValue;
        }
        float intensity = Mathf.Lerp(minIntensity, maxIntensity, beatValue);
        bloom.intensity.value = intensity;
    }
}
