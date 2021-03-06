using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Assignables
    [SerializeField] ParticleSystem explosionFX;
    [SerializeField] float speed = 1f;
    [SerializeField] float threshold = 1f;
    [SerializeField] int hyperPointValue;
    [SerializeField] bool randomize = true;
    [SerializeField] protected float lifeTime;
    [SerializeField] int screenGap;
    [SerializeField] int pointValue;

    //References
    protected float timer = 0;
    private bool stopped = true;
    private Vector3 targetLocation;
    private GameManager gm;
    protected EnemySpawner spawner;
    protected Animation anim;

    void Start()
    {
        if (randomize)
            transform.position = GetRandomPos();
        gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        spawner = GameObject.Find("EnemySpawner").GetComponent<EnemySpawner>();
        anim = GetComponent<Animation>();
    }

    void Update() 
    {
        // lifetimer
        timer += Time.deltaTime;
        if (timer >= lifeTime)
        {
            Destroy(gameObject);
            spawner.Killed();
        }
        // animation flashes when 5s of life left
        if (lifeTime-timer < 5)
            anim.Play("Enemy_Sayonara");

    }

    void FixedUpdate() 
    {
        // finding position and moving towards it 
        if (stopped)
        {
            targetLocation = GetRandomPos();
            // print("Moving to " + targetLocation.ToString());
            stopped = false;
        } 
        else
        {
            Vector3 dir = targetLocation - transform.position;
            if (dir.magnitude < threshold)
                stopped = true; // we have arrived
            else
                transform.position += dir.normalized * speed * Time.deltaTime; // vroom vroom
        }
    }

    virtual public void Damage()
    {
        gm.IncreasePoints(pointValue);
        Die();
    }

    public void Die()
    {
        Instantiate(explosionFX, new Vector3(transform.position.x, transform.position.y, -1), Quaternion.identity);
        Destroy(gameObject);
        // gm.IncreasePoints(pointValue);
        spawner.Killed();
    }

    private Vector3 GetRandomPos()
    {
        float spawnX = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(screenGap, 0)).x, Camera.main.ScreenToWorldPoint(new Vector2(Screen.width-screenGap, 0)).x);
        float spawnY = Random.Range(Camera.main.ScreenToWorldPoint(new Vector2(0, screenGap + 50)).y, Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height-screenGap)).y);
        return new Vector3(spawnX, spawnY, 0);
    }

    public void setSpeed(float _speed)
    {
        speed = _speed;
    }

}
