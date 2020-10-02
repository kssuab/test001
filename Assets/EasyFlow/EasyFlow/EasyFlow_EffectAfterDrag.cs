using System.Collections;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    public partial class EasyFlow : MonoBehaviour
    {
        private static Vector2 dragAccuredDistance = Vector2.zero;

        public void AccureDragDistance(Vector2 deltaPos)
        {
            dragAccuredDistance += deltaPos;

            if (IsInvoking("EADReduceProcess") == false)
                InvokeRepeating("EADReduceProcess", 0.02f, 0.02f);
        }

        public void BeginEADProcess()
        {
            StopCoroutine("EADProcess");

            StartCoroutine("EADProcess");
        }

        public void StopEADProcess()
        {
            StopCoroutine("EADProcess");

            if (IsInvoking("EADReduceProcess"))
                CancelInvoke("EADReduceProcess");

            dragAccuredDistance = Vector2.zero;
        }

        private void EADReduceProcess()
        {
            dragAccuredDistance *= 0.975f;
            if (dragAccuredDistance.sqrMagnitude < 3000.0f)
            {
                dragAccuredDistance = Vector2.zero;
                CancelInvoke("EADReduceProcess");
            }
        }

        private IEnumerator EADProcess()
        {
            if (IsInvoking("EADReduceProcess"))
                CancelInvoke("EADReduceProcess");

            float accuredPlayTime = 0.0f;
            Vector3 beginPosition = flowPosition;
            Vector3 deltaGoalPosition = Vector3.zero;

            Vector3 normalDragAccuredDistance = (Vector3)dragAccuredDistance.normalized;
            Vector3 normalCoverDistance = saveData.coverDistance.normalized;
            float dir = Vector3.Dot(normalDragAccuredDistance, normalCoverDistance);
            float power = dir * (Mathf.Abs(dragAccuredDistance.x) + Mathf.Abs(dragAccuredDistance.y));
            Vector3 deltaPosition = saveData.coverDistance.normalized * power;

            // Calculate Goal Position
            // is [MoveToNearCover] Mode
            if (saveData.isMoveToNearCover == BOOL.Yes)
            {
                // Calculate NearCover Position
                Cover nearCover = FindNearCover(flowPosition + deltaPosition);
                if (nearCover != null)
                    deltaGoalPosition = nearCover.position - flowPosition;
            }
            else
            {
                deltaGoalPosition = deltaPosition;
            }

            float percent = 0.0f;
            while (true)
            {
                // Check end time
                accuredPlayTime += Time.smoothDeltaTime;
                if (accuredPlayTime >= saveData.effectAfterDragTime)
                {
                    flowPosition = beginPosition + deltaGoalPosition;
                    UpdateCover();
                    break;
                }

                // Calc next position
                percent = accuredPlayTime / saveData.effectAfterDragTime;
                deltaPosition = deltaGoalPosition * saveData.effectAfterDragCurve.Evaluate(percent);

                // Move
                flowPosition = beginPosition + deltaPosition;
                UpdateCover();

                // Next
                yield return new WaitForEndOfFrame();
            }
        }
    }
}