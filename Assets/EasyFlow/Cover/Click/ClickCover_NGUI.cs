using UnityEngine;

namespace NAsoft_EasyFlow
{
    public class ClickCover_NGUI : ClickCover
    {
        public override void OnAfterCreated(EasyFlow easyflow, Cover cover)
        {
            base.OnAfterCreated(easyflow, cover);
#if NGUI_USED
            UIEventTrigger eventTrigger = gameObject.AddComponent<UIEventTrigger>();
            eventTrigger.onClick.Add(new EventDelegate(_OnClick));

            UITexture texture = GetComponent<UITexture>();

            BoxCollider collider = gameObject.AddComponent<BoxCollider>();
            collider.size = texture.localSize;
#endif
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();

#if NGUI_USED
            UIEventTrigger eventTrigger = GetComponent<UIEventTrigger>();
            if (eventTrigger != null)
                DestroyImmediate(eventTrigger);

            BoxCollider collider = GetComponent<BoxCollider>();
            if (collider != null)
                DestroyImmediate(collider);
#endif
        }

        public void _OnClick()
        {
            if (Vector3Comparer(transform.localPosition, Vector3.zero, 0.1f))
                OnClickCenter();
            else
                OnClickOther();
        }
    }
}