﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooty : MonoBehaviour
{
    [SerializeField] float timeBtwShot = 1;
    private float timer;

    private AudioSource shootySound;
    private GameManager gm;
    private ReloadTime reloader;

    void Start() 
    {
        timer = timeBtwShot;
        GameObject.Find("ReloadIndicator").GetComponent<ReloadTime>().SetMax(timeBtwShot);
        shootySound = GetComponent<AudioSource>();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        reloader = GameObject.Find("ReloadIndicator").GetComponent<ReloadTime>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timeBtwShot)
        {
            if (Input.GetMouseButtonDown(0))
            {
                // print("click");
                shootySound.Play();
                
                Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                Vector2 mouse2D = new Vector2(mousePos.x, mousePos.y);

                RaycastHit2D hit = Physics2D.Raycast(mouse2D, Vector2.zero);

                if (hit.collider != null)
                {
                    // print(hit.collider.name);
                    hit.collider.GetComponent<Enemy>().Damage();

                    // add posibility of killing multiple enemies with 1 shot
                    hit = Physics2D.Raycast(mouse2D, Vector2.zero);
                    if (hit.collider != null)
                    {
                        hit.collider.GetComponent<Enemy>().Damage();
                        gm.IncreasePoints(5);
                    }
                } 
                else
                {
                    gm.DecreasePoints(1);
                }        
                timer = 0;
                reloader.TurnOn();
            }
        }
    }
}
