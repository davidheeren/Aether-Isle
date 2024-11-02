using UnityEngine;
using Game;
using System.Collections;
using System;


public class RespawnController : MonoBehaviour 
{

    Vector2 startPos;
    private Health Health;


    private void Start()
    {
       startPos = transform.position;
       Health = FindAnyObjectByType<Health>();
    }

    private bool GetIsDead()
    {
        return Health.isDead;
    }

    private void die(bool isDead)
   {
     if(isDead == true)
        {
            StartCoroutine(Respawn(0.5f));
        }
   }


    IEnumerator Respawn(float duration)
    {
        yield return new WaitForSeconds(duration);
        transform.position = startPos;

    }
}

