using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogRotation : MonoBehaviour
{
    
    [System.Serializable]
    private class RotationElement
    {
        #pragma warning disable 0649
        public float Speed;
        public float Duration;
        #pragma warning restore 0649
    }

    [SerializeField]
    private RotationElement[] rotationPattern;

    private WheelJoint2D wheelJoint;
    private JointMotor2D motor;

    // private GameObject logObj;

    private void Awake()
    {
        // logObj = GameObject.Find("Log");

        // Texture2D texture = new Texture2D(128, 128);
        // logObj.GetComponent<SpriteRenderer>().material.mainTexture = texture;
        // for (int y = 0; y < texture.height; y++)
        // {
        //     for (int x = 0; x < texture.width; x++)
        //     {

        //         texture.SetPixel(x, y, Color.black);
        //     }
        // }
        // texture.Apply();
    
        wheelJoint = GetComponent<WheelJoint2D>();
        motor = new JointMotor2D();
        StartCoroutine("PlayRotationPattern");
    }

    private IEnumerator PlayRotationPattern()
    {
        int rotationIndex = 0;
        while (true)
        {
            yield return new WaitForFixedUpdate();

            motor.motorSpeed = rotationPattern[rotationIndex].Speed;
            motor.maxMotorTorque = 10000;
            wheelJoint.motor = motor;

            yield return new WaitForSecondsRealtime(rotationPattern[rotationIndex].Duration);

            rotationIndex++;
            rotationIndex = rotationIndex < rotationPattern.Length ? rotationIndex : 0;
        }
    }
}
