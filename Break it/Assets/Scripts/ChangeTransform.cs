using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTransform : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public Transform parent;
    [SerializeField]
    public bool isWhite = true;

    public
    void Awake()
    {
        transform.SetParent(parent);
    }

}
