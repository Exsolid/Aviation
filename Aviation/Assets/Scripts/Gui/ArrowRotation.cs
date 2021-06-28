using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    [SerializeField] private float rotationForCompletion;
    [SerializeField] private float minAngle;
    [SerializeField] private float maxValueGui;
    private float currentValue;
    private float maxValue;
    public float MaxValue { get { return maxValue; } set { maxValue = value; currentValue = value; setRotation(); } }
    public float CurrentValue { get { return currentValue; } set { currentValue = value; setRotation(); } }

    private void setRotation()
    {
        float newAngle = minAngle + rotationForCompletion * (maxValue/maxValueGui)  * (CurrentValue / MaxValue) ;
        gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.eulerAngles.x, newAngle, gameObject.transform.rotation.eulerAngles.z);
    }
}
