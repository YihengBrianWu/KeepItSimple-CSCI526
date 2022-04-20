using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardCircle : MonoBehaviour
{
    [SerializeField] 
    private ParticleSystem circleParticle;

    private BoxCollider2D circleCollider;
    private SpriteRenderer sp;

    //sound
    private AudioSource music;
    private AudioClip getReward;

    void Start()
    {
        circleCollider = GetComponent<BoxCollider2D>();
        sp = GetComponent<SpriteRenderer>();

        music = gameObject.GetComponent<AudioSource>();
        getReward = Resources.Load<AudioClip>("sound/getReward");
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
            int tempLevel = PlayerPrefs.GetInt("level"+levelParam, 0);
            PlayerPrefs.SetInt("level"+levelParam, tempLevel + 1);
            
            int tempTotal = PlayerPrefs.GetInt("total", 0);
            PlayerPrefs.SetInt("total", tempTotal + 1);

            circleParticle.Play();
            Destroy(gameObject, 2f);

            music.clip = getReward;
            music.Play();
        }
    }

}
