using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class BeatDetection : MonoBehaviour
{
    // Start is called before the first frame update
    public float sensitivity = 1f;  // Adjust this value to control the beat detection sensitivity
    public float beatValue;        // The beat value to be used for the glowing effect

    private AudioSource audioSource;
    private float[] spectrumData;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        // Set the audio source to use the music audio clip
        // Replace "audioClip" with your actual music audio clip
        // AudioClip audioClip = Resources.Load<AudioClip>("Music/AudioClip");
        // audioSource.clip = audioClip;

        // // Play the audio clip
        // audioSource.Play();
    }

    private void Update()
    {
        // Get the spectrum data from the audio source
        spectrumData = new float[256];
        audioSource.GetSpectrumData(spectrumData, 0, FFTWindow.BlackmanHarris);

        // Perform beat detection based on spectrum data
        float currentBeatValue = 0f;
        for (int i = 0; i < spectrumData.Length; i++)
        {
            currentBeatValue += spectrumData[i];
        }
        currentBeatValue *= sensitivity;

        // Update the beat value for the glowing effect
        beatValue = currentBeatValue;
    }
}
