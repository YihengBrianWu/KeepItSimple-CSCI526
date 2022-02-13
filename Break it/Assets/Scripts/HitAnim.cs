using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitAnim : MonoBehaviour
{

    public Animator camAnim;

    public void HitShake(){
        camAnim.SetTrigger("hit");
    }

    public void MissShake(){
        camAnim.SetTrigger("miss");
    }
}
