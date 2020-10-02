using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    public partial class EasyFlow : MonoBehaviour
    {
        public int currentIndex;

        public void MoveNext(int count = 1)
        {
            MoveTo(currentIndex + count);
        }

        public void MovePrev(int count = 1)
        {
            MoveTo(currentIndex - count);
        }

        public void MoveToFirst()
        {
            MoveTo(0);
        }

        public void MoveToLast()
        {
            if (coverList != null)
                MoveTo(coverList.Count - 1);
        }

        public void MoveTo(int index)
        {
            if (coverList == null ||
                coverList.Count <= index)
                return;

            index = Mathf.Clamp(index, 0, coverList.Count - 1);

            Vector3 deltaPosition = coverList[index].position - coverList[currentIndex].position;
            dragAccuredDistance = deltaPosition;
            Debug.Log(deltaPosition);
            BeginEADProcess();
        }
    }
}