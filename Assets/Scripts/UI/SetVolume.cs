using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void SetLevel(float sliderValue) { 

        // Converts to log because of dB
        mixer.SetFloat("SoundVol", Mathf.Log10(sliderValue) * 20);
    }
}
