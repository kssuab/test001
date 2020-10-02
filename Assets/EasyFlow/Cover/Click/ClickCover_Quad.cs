using UnityEngine;

namespace NAsoft_EasyFlow
{
    public class ClickCover_Quad : ClickCover
    {
        private Vector3 mousePos;

        public override void OnAfterCreated(EasyFlow easyflow, Cover cover)
        {
            base.OnAfterCreated(easyflow, cover);

            var v = gameObject.GetComponent<Collider>();
            if (v == null)
                v = gameObject.AddComponent<BoxCollider>();
            v.isTrigger = true;
        }

        public override void OnBeforeDestroy()
        {
            base.OnBeforeDestroy();

            var v = gameObject.GetComponent<Collider>();
            if (v != null)
                DestroyImmediate(v);
        }

        private void OnGUI()
        {
            if (easyflow == null ||
                easyflow.easyflowCamera == null)
                return;

            var ray = easyflow.easyflowCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            var collider = GetComponent<Collider>();
            if (collider.Raycast(ray, out hit, float.MaxValue/* transform.position.z + 1.0f*/))
            {
                switch (Event.current.type)
                {
                    case EventType.MouseDown:
                        _OnMouseDown();
                        break;

                    case EventType.MouseUp:
                        _OnMouseUp();
                        break;
                }
            }
        }

        private void _OnMouseDown()
        {
            mousePos = Input.mousePosition;
        }

        private void _OnMouseUp()
        {
            if (Vector3Comparer(mousePos, Input.mousePosition, 5.0f))
            {
                if (Vector3Comparer(transform.localPosition, Vector3.zero, 0.1f))
                    OnClickCenter();
                else
                    OnClickOther();
            }
        }
    }
}