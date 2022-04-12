using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    AudioSource self;
    float defaultVolume = 0;

    private void Awake()
    {
        self = GetComponent<AudioSource>();
        if (defaultVolume == 0)
            defaultVolume = 0.2f;
        else
            defaultVolume = self.volume;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            self.volume = defaultVolume;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent<Player>(out Player player))
        {
            self.volume = 0f;
        }
    }
}
