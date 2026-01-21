using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXPlay : MonoBehaviour
{
    public AudioSource myAudioSource;
    public AudioClip myAudioClip;

    private void OnCollisionEnter(Collision collision)
    {
        myAudioSource.PlayOneShot(myAudioClip);
    }
}
