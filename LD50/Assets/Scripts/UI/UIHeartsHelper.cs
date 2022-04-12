using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHeartsHelper : MonoBehaviour
{
    public List<HeartComponent> hearts;
    public Player player;
    public GameObject heart;
    public int maxhealth;
    int health;
    int downLifes;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        maxhealth = player.maxHealth;
        AddHeart(maxhealth);

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(player.health);

        downLifes = player.health;

        //health = player.health = downLifes;
        if (player.maxHealth > maxhealth)
        {
            maxhealth = player.maxHealth;
            AddHeart(player.maxHealth - maxhealth);
        }

        if (hearts[0] != null)
        {
            for (var n = 0; n < hearts.Count; n++)
            {
                if (n < downLifes)
                {
                    hearts[n].SetActivo(true);
                }
                else
                {
                    hearts[n].SetActivo(false);
                }
            }
        }

    }

    public void AddHeart(int num)
    {
        for(int n = 0; n< num; n++)
        {
            var corazon = Instantiate(heart) as GameObject;
            corazon.transform.parent = this.transform;
            hearts.Add(corazon.GetComponent<HeartComponent>());
        }
    }

    public void DeleteHeart()
    {
        Destroy(hearts[hearts.Count]);
    }
}
