using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingBlocks : MonoBehaviour
{

    public Vector2 speedMinMax;
    float speed;
    public Vector2[] vecArray;
    int vl1 ;
    int vl2, vl3;
    // private Vector2 temp,temp1,temp2;
    public GameObject o1, o2;

    float visibleHeightThreshold;

    void Start()
    {
        vl1 = Random.Range(0, 2);
        valuepos();
        vecArray = new Vector2[]
        {
   transform.position,o1.transform.position,o2.transform.position
    };
        speed = Mathf.Lerp(speedMinMax.x, speedMinMax.y, Mathf.Clamp01(Time.timeSinceLevelLoad / 60));

        visibleHeightThreshold = -Camera.main.orthographicSize - transform.localScale.y;
       
    }
    void valuerand() {

        int startNumber = 0;
        int endNumber = 2;
        List<int> numberPot = new List<int>();
        for (int i = startNumber; i < endNumber + 1; i++)
        {
            numberPot.Add(i);
        }

        while (numberPot.Count > 0)
        {
            int index = Random.Range(0, numberPot.Count);
            int randomNumber = numberPot[index];
            numberPot.RemoveAt(index);
            Debug.Log(randomNumber);
            PlayerPrefs.SetInt("randompos", randomNumber);
        }
       
       // return randomNumber;
    }
    int valuepos() {
        
        if (vl1 == 1) {
            vl2 = 0;
            vl3 = 2;
            return vl1;
        }
        else if (vl1 == 2)
        {
            vl2 = 0;
            vl3 = 1;
            return vl1;
        }
        else {
            vl2 = 2;
            vl3 = 1;
            return vl1;
        }
    }
    public void resetposition()
    {
       
        transform.Translate(Vector3.down * speed * Time.deltaTime, Space.Self);
       
        if (transform.position.y < visibleHeightThreshold)
        {
            valuerand();
            transform.position = vecArray[PlayerPrefs.GetInt("randompos")];
            //  gameObject.SetActive(true);
        }
      /*  if (PlayerPrefs.GetInt("correct", 1) == 1)
        {
            transform.position = vecArray[vl1];
            o1.transform.position = vecArray[vl2];
            o2.transform.position = vecArray[vl3];
            //  gameObject.SetActive(true);
            PlayerPrefs.SetInt("correct", 0);
        }
        else if (PlayerPrefs.GetInt("missed", 1) == 1)
        {
            transform.position = vecArray[vl1];
            o1.transform.position = vecArray[vl2];
            o2.transform.position = vecArray[vl3];
            //  gameObject.SetActive(true);
            PlayerPrefs.SetInt("missed", 0);
        } */
    }
    void Update()
    {

       // if (transform.position.y < visibleHeightThreshold)
            resetposition();

    }

}
