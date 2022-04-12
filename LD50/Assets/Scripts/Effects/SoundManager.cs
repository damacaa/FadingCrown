using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] soundClip;

    public AudioSource audioCancion1;
    public AudioSource audioCancion2;
    public AudioSource audioCancion3;
    public AudioSource audioAmbiental;
    public AudioSource audioVictoryDefeat;

    public enum audioType {Victoria, RocasRompiendo, Lava, Derrota, Medusa, Golem, Slash, SlashHit,
                           Paso1, Paso2, Jump, Dash, Damage, Aterrizar, CantoVoz, CantoLofi, CantoAnimado}



    // Start is called before the first frame update
    void Start()
    {

    }

    public void AudioSelect(audioType audio, AudioSource selfAudio)
    {

        if (soundClip.Length == 0)
            return;

        switch (audio)
        {
            case audioType.Victoria:
                audioVictoryDefeat.PlayOneShot(soundClip[0]);
                break;
            case audioType.RocasRompiendo:
                selfAudio.PlayOneShot(soundClip[1]);
                break;
            case audioType.Lava:
                audioAmbiental.clip = soundClip[2];
                audioAmbiental.Play();
                audioAmbiental.loop = true;
                break;
            case audioType.Derrota:
                audioVictoryDefeat.PlayOneShot(soundClip[3]);
                break;
            case audioType.Medusa:
                selfAudio.Play();
                selfAudio.loop = true;
                break;
            case audioType.Golem:
                selfAudio.Play();
                selfAudio.loop = true;
                break;
            case audioType.Slash:
                selfAudio.PlayOneShot(soundClip[6]);
                break;
            case audioType.SlashHit:
                selfAudio.PlayOneShot(soundClip[7]);
                break;
            case audioType.Paso1:
                selfAudio.clip = soundClip[8];
                selfAudio.Play();
                selfAudio.loop = true;
                break;
            case audioType.Paso2:
                selfAudio.clip = soundClip[9];
                selfAudio.Play();
                break;
            case audioType.Jump:
                selfAudio.PlayOneShot(soundClip[10]);
                break;
            case audioType.Dash:
                selfAudio.PlayOneShot(soundClip[11]);
                break;
            case audioType.Damage:
                selfAudio.PlayOneShot(soundClip[12]);
                break;
            case audioType.Aterrizar:
                selfAudio.PlayOneShot(soundClip[13]);
                break;
            case audioType.CantoVoz:
                audioCancion1.clip = soundClip[14];
                audioCancion1.Play();
                audioCancion1.loop = true;
                break;
            case audioType.CantoLofi:
                audioCancion2.clip = soundClip[15];
                audioCancion2.Play();
                audioCancion2.loop = true;
                break;
            case audioType.CantoAnimado:
                audioCancion3.clip = soundClip[16];
                audioCancion3.Play();
                audioCancion3.loop = true;
                break;
        }
    }
}
