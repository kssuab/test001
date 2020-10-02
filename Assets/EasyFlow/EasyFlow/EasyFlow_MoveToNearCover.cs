using System.Collections;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    public partial class EasyFlow : MonoBehaviour
    {
        public void MoveToNearCover()
        {
            flowPosition = FindNearPosition(flowPosition);
            UpdateCover();
        }

        private int FindNearCoverIndex(Vector3 targetPosition)
        {
            int nearIndex = 0;
            float nearDistance = float.MaxValue;
            float gapDistance = 0.0f;

            for (int i = 0; i < coverList.Count; ++i)
            {
                gapDistance = (coverList[i].GetPosition() - targetPosition).sqrMagnitude;
                if (gapDistance <= nearDistance)
                {
                    nearDistance = gapDistance;
                    nearIndex = i;
                }
            }

            return nearIndex;
        }

        private Cover FindNearCover(Vector3 targetPosition)
        {
            int nearIndex = FindNearCoverIndex(targetPosition);
            return coverList[nearIndex];
        }

        private Vector3 FindNearPosition(Vector3 targetPosition)
        {
            Cover nearCover = FindNearCover(targetPosition);
            Vector3 nearPosition = nearCover.position;
            return nearPosition;
        }
    }
}