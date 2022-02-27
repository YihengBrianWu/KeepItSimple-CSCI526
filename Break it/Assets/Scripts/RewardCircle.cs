using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCircle : MonoBehaviour
{
    [SerializeField] 
    private ParticleSystem circleParticle;

    private BoxCollider2D circleCollider;
    private SpriteRenderer sp;

    void Start()
    {
        circleCollider = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();    
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Knife"))
        {

            circleCollider.enabled = false;
            sp.enabled = false;
            transform.parent = null;

            RewardCount.CircleCount ++;

            int levelParam = GameObject.FindGameObjectWithTag("LevelControl").GetComponent<GameController>().difficulty;
            PlayerPrefs.SetInt("level"+levelParam, RewardCount.CircleCount);

            circleParticle.Play();
            Destroy(gameObject, 2f);
        }
    }

}
