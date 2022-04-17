using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstaclesDestory : MonoBehaviour
{

    private float fade = 1.2f;
    private Material m1;
    private bool startDissolve = false;    
    void Start()
    {
        m1 = this.GetComponent<SpriteRenderer>().material;
    }
    void Update()
    {
        if(startDissolve)
        {
            fade -= Time.deltaTime;
            if (fade <= 0f)
            {
                startDissolve = false;
            }

            m1.SetFloat("_Fade", fade);
        }
    }

    public void selfDestory()
    {   
        this.GetComponent<BoxCollider2D>().enabled = false;  
        startDissolve = true;
        StartCoroutine("waitEliminate");
    }

    IEnumerator waitEliminate() {
        yield return new WaitForSecondsRealtime(1.2f);
        Destroy(this.gameObject);
    }
}
