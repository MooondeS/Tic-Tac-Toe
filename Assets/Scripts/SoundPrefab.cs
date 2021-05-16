using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPrefab : MonoBehaviour
{
    public AudioSource soundSource;

    void Start()
    {
        Destroy(gameObject, soundSource.clip.length);
    }
}
