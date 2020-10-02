using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tester : MonoBehaviour
{
    public class TestClass
    {
        public int number;
    }

    public void OnClickButton()
    {
        for (int i = 0; i < 1000; i++)
        {
            Debug.Log("Log");
            Debug.LogWarning("Warning");
            Debug.LogError("Error");
            TestClass t = null;
            Debug.Log(t.number); 
        } 
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
    

        }
    }
}
