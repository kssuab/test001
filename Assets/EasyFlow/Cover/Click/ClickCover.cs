using UnityEngine;

namespace NAsoft_EasyFlow
{
    public class ClickCover : MonoBehaviour
    {
        public EasyFlow easyflow;
        public Cover cover;

        public virtual void OnAfterCreated(EasyFlow easyflow, Cover cover)
        {
            this.easyflow = easyflow;
            this.cover = cover;
        }

        public virtual void OnBeforeDestroy()
        {
        }

        protected virtual void OnClickCenter()
        {
            //Debug.Log("OnClick (Center) - " + cover.name + "   Pos: " + cover.GetPosition());

            if (easyflow == null ||
                easyflow.callbackList == null)
                return;

            if (easyflow.saveData.clickMode == CLICK_MODE.ClickCenter ||
                easyflow.saveData.clickMode == CLICK_MODE.ClickAll ||
                easyflow.saveData.clickMode == CLICK_MODE.MoveToAndClickCenter ||
                easyflow.saveData.clickMode == CLICK_MODE.MoveToAndClickAll)
                CallList();
        }

        protected virtual void OnClickOther()
        {
            //Debug.Log("OnClick (Other) - " + cover.name + "   Pos: " + cover.GetPosition() + "   BeginPos: " + easyflow.beginPosition + "   FlowPos: " + easyflow.flowPosition);

            if (easyflow == null)
                return;

            if (easyflow.saveData.clickMode == CLICK_MODE.MoveTo ||
                easyflow.saveData.clickMode == CLICK_MODE.MoveToAndClickCenter ||
                easyflow.saveData.clickMode == CLICK_MODE.MoveToAndClickAll)
                easyflow.MoveTo(cover);

            if (easyflow.saveData.clickMode == CLICK_MODE.ClickAll ||
                easyflow.saveData.clickMode == CLICK_MODE.MoveToAndClickAll)
                CallList();
        }

        private void CallList()
        {
            foreach (var v in easyflow.callbackList)
            {
                var p = v.GetFirstParameterType();
                if (p == typeof(Cover))
                    v.Invoke(cover);
                else if (p == typeof(int))
                    v.Invoke(easyflow.currentIndex);
                else if (p == typeof(string))
                    v.Invoke(easyflow.currentIndex.ToString());
            }
        }

        protected static bool Vector3Comparer(Vector3 v1, Vector3 v2, float tolerance)
        {
            if (Mathf.Abs(v1.x - v2.x) < tolerance &&
                Mathf.Abs(v1.y - v2.y) < tolerance &&
                Mathf.Abs(v1.z - v2.z) < tolerance)
                return true;
            else
                return false;
        }
    }
}