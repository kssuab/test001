using System.Collections;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    [RequireComponent(typeof(EasyFlow))]
    public class Drag_onCollider : Drag
    {
        public void Awake()
        {
            easyflow = GetComponent<EasyFlow>();
        }

        public void OnGUI()
        {
            if (Event.current == null)
                return;

            if (easyflow == null ||
                easyflow.saveData == null ||
                easyflow.dragCamera == null ||
                easyflow.dragCollider == null)
                return;

            var ray = easyflow.dragCamera.ScreenPointToRay(Event.current.mousePosition);
            RaycastHit hit;
            if (isBeginDrag || easyflow.dragCollider.Raycast(ray, out hit, float.MaxValue))
            {
                switch (Event.current.type)
                {
                    case EventType.MouseDown:
                        _OnMouseDown(Event.current.mousePosition);
                        Event.current.Use();
                        break;

                    case EventType.MouseDrag:
                        _OnMouseDrag(Event.current.mousePosition);
                        Event.current.Use();
                        break;

                    case EventType.MouseUp:
                        _OnMouseUp(Event.current.mousePosition);
                        Event.current.Use();
                        break;
                }
            }
        }
    }
}