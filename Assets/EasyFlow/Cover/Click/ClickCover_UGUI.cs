using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NAsoft_EasyFlow
{
    public class ClickCover_UGUI : ClickCover, IPointerClickHandler
    {
        public override void OnAfterCreated(EasyFlow easyflow, Cover cover)
        {
            base.OnAfterCreated(easyflow, cover);

            gameObject.AddComponent<GraphicRaycaster>();
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();

            var graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
            if (graphicRaycaster != null)
                DestroyImmediate(graphicRaycaster);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (Vector3Comparer(transform.localPosition, Vector3.zero, 0.1f))
                OnClickCenter();
            else
                OnClickOther();
        }
    }
}