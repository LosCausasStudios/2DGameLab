using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public int life;
    public int speed;
    public int atackDamage;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void reciveDamage(int d)
    {
        life = life - d;
    }
    public int atack()
    {
        return atackDamage;
    }
    public int getSpeed()
    {
        return speed;
    }
}
