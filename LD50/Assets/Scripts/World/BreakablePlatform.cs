using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class BreakablePlatform : MonoBehaviour
{
    bool breaking = false, shaking = false;
    bool breaked;
    Vector3 iniPos;
    SpriteRenderer sprite;
    Color color;
    Color baseColor;
    public float speedDrop = 2f;
    public float speedShake = 2f;
    public float secondsToDrop = 1f;
    float step;
    Vector3 disaPos;
    public int timeRegenerate = 4;
    public float slideMove = 2f;
    int repeticiones = 4;
    public float offset;

    public SoundManager audios;
    public AudioSource selfAudio;

    // El alpha en .5 para que en el momento en que empieza a caer haya feedback
    // Si no queda raro
    float alpha = .5f;

    float timeSinceLastShake;



    //public float timeShake = 0.3f;

    private void Start()
    {
        audios = GameObject.FindObjectOfType<SoundManager>().GetComponent<SoundManager>();
        selfAudio = GetComponent<AudioSource>();
        iniPos = transform.position;
        baseColor = GetComponent<SpriteRenderer>().color;
        sprite = this.GetComponent<SpriteRenderer>();
        disaPos = new Vector3(transform.position.x, transform.position.y - 1000, transform.position.z);
        color = gameObject.GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {

        if (breaking)
        {
            alpha -= Time.deltaTime;
            color.a = alpha;
            GetComponent<SpriteRenderer>().color = color;
            sprite.transform.position -= new Vector3(0, Time.deltaTime * 10, 0);
        }
        else if(shaking)
        {
            if (Time.realtimeSinceStartup - timeSinceLastShake > .05f)
            {
                //Debug.Log("SHAKE");
                timeSinceLastShake = Time.realtimeSinceStartup;
                sprite.transform.position = iniPos + new Vector3(UnityEngine.Random.Range(-0.15f, 0.15f), 0, 0);
            }
        }
    }

    public void Drop()
    {        
        GetComponent<Collider2D>().isTrigger = true;
    }


    public async void Cooldown()
    {
        shaking = true;
        audios.AudioSelect(SoundManager.audioType.RocasRompiendo, selfAudio);
        await Task.Delay((int)(secondsToDrop * 1000));

        breaking = true;
        Drop();
        await Task.Delay(1000);

        await Task.Delay(timeRegenerate * 1000);
        Reset();
    }

    private void Reset()
    {
        transform.position = iniPos;
        GetComponent<Collider2D>().isTrigger = false;
        GetComponent<SpriteRenderer>().color = baseColor;
        sprite.transform.position = iniPos;
        alpha = .5f;
        breaked = false;
        breaking = false;
        shaking = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.transform.position.y > transform.position.y + offset)
        {
            //Debug.Log("entro al cooldown");
            //Invoke("Shake", 1f);
            Cooldown();             
        }
    }
}
