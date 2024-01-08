using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IrregularButton : MonoBehaviour
{
    void Start()
    {
        //alphaHitTestMinimumThreshold 范围值：0 ~ 1 。
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }

    void Update()
    {

    }
}
