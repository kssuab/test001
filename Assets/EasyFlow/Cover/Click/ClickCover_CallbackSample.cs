using System;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    [Serializable]
    public class ClickCover_CallbackSample : MonoBehaviour
    {
        public void Callback(Cover cover)
        {
            if (cover == null)
                Debug.Log("Callback - Click Cover : Cover is null");
            else
                Debug.Log("Callback - Click Cover : " + cover.name);
        }

        public void Callback(int index)
        {
            Debug.Log("Callback - Click Cover : " + index);
        }

        public void Callback(string str)
        {
            Debug.Log("Callback - Click Cover : " + str);
        }
    }
}