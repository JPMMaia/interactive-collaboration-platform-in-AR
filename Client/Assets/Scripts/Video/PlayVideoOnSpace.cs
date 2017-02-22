using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVideoOnSpace : MonoBehaviour
{
    public MovieTexture movieTexture;

    void Update()
    {
        if (!Input.GetButtonDown("Jump")) return;

        if (movieTexture.isPlaying)
        {
            movieTexture.Pause();
        }
        else
        {
            movieTexture.Play();
        }
    }
}

