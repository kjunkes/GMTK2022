using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioClip introClip;
    public AudioClip loopClip;

    private bool loop = false;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = introClip;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(!loop && !audioSource.isPlaying)
        {
            audioSource.clip = loopClip;
            audioSource.Play();
        }
    }
}
