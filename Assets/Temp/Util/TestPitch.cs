using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioListener))]

public class TestPitch : MonoBehaviour
{
    public AudioClip testclip;
    public AudioSource _source;
    // Start is called before the first frame update
    void Start()
    {
      
         



    }



    void Update()
    {
        //float[] spectrum = new float[256];

        //AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);

        //for (int i = 1; i < spectrum.Length - 1; i++)
        //{
        //    Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
        //    Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2), new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
        //    Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1), new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
        //    Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3), new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        //}

        if (Input.GetKeyDown(KeyCode.B))
        {
            float[] spectrum = new float[256];
            _source.clip.GetData(spectrum, _source.timeSamples);//.GetOutputData(spectrum, 0);
           
            float _ret = 0;
            for (int i = 0; i < spectrum.Length; i++)
            {
                _ret += Mathf.Abs( spectrum[i]);
            }
            Debug.Log("평균 볼류  " + _ret/1024);
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            float[] spectrum = new float[256];
            AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
            var maxIndex = 0;
            var maxValue = 0.0f;
            for (int i = 0; i < spectrum.Length; i++)
            {
                var val = spectrum[i];
                if (val > maxValue)
                {
                    maxValue = val;
                    maxIndex = i;
                }
            }
            var freq = maxIndex * AudioSettings.outputSampleRate / 2 / spectrum.Length;
            Debug.Log("주파수  " +freq);
            Debug.Log(CalculateNoteNumberFromFrequency(freq));
        }

    }
    public   int CalculateNoteNumberFromFrequency(float freq)
    {
        return Mathf.FloorToInt(69 + 12 * Mathf.Log(freq / 440, 2));
    }
}
 