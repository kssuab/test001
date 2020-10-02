using System.Collections;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    public class Cover : MonoBehaviour
    {
        protected SaveData saveData;
        public Vector3 position;
        public int depth;

        [SerializeField]
        public CLICK_MODE clickMode;

        public virtual void SetTexture(Texture texture)
        {
        }

        public virtual void SetSize(Vector2 size)
        {
        }

        public void SetClickMode(EasyFlow easyflow, CLICK_MODE clickMode)
        {
            if (this.clickMode != clickMode)
            {
                this.clickMode = clickMode;

                // Remove previous ClickCover
                ClickCover cc = GetComponent<ClickCover>();
                if (cc != null)
                {
                    cc.OnBeforeDestroy();
                    DestroyImmediate(cc);
                }

                if (clickMode != CLICK_MODE.None)
                {
                    // Add new ClickCover
                    if (this is Cover_NGUI)
                        cc = gameObject.AddComponent<ClickCover_NGUI>();
                    if (this is Cover_Quad)
                        cc = gameObject.AddComponent<ClickCover_Quad>();
                    if (this is Cover_UGUI)
                        cc = gameObject.AddComponent<ClickCover_UGUI>();

                    // Initalize
                    cc.OnAfterCreated(easyflow, this);
                }
            }
        }

        public Vector3 GetPosition()
        {
            if (saveData.isPanelMove == BOOL.Yes)
                return transform.localPosition;
            else
                return position;
        }

        // near is 0.0 (Min in curve)
        // far is 1.0 (Max in curve)
        protected float CalcPercent(Vector3 flowPosition, float range, AnimationCurve curve)
        {
            if ((Mathf.Abs(saveData.coverDistance.x) <= 0.00001f && Mathf.Abs(saveData.coverDistance.y) <= 0.00001f) ||
               Mathf.Abs(range) <= 0.00001f)
                return 0.0f;

            float gapDistance = (GetPosition() - flowPosition).sqrMagnitude;
            float gapPercent = gapDistance / (float)(range * range);
            float percent = curve.Evaluate(gapPercent);
            return percent;
        }

        // (left)-1.0 ~ (near)0.0 ~ (right)1.0
        private float CalcRate(Vector3 flowPosition, float range, AnimationCurve curve)
        {
            float rate = CalcPercent(flowPosition, range, curve);
            if (IsForward(GetPosition(), flowPosition) == false)
                rate = -rate;
            return rate;
        }

        private bool IsForward(Vector3 position, Vector3 flowPosition)
        {
            Vector3 gap = position - flowPosition;
            if (Vector3.Dot(gap, saveData.coverDistance) < 0.0f)
                return false;
            return true;
        }

        public void Init(string name, EasyFlow parent, Vector3 position)
        {
            gameObject.name = name;
            this.saveData = parent.saveData;
            this.position = position;
            transform.localPosition = position;
            //isClickEnabled = false;
        }

        public void UpdatePosition(Vector3 flowPosition)
        {
            // Move Cover Mode
            if (saveData.isPanelMove == BOOL.No)
            {
                float distantRate = CalcRate(flowPosition, saveData.positionInfluenceRange, saveData.positionCurve) * saveData.positionRate;
                Vector3 distantDistance = saveData.coverDistance * distantRate;
                Vector3 nextPosition = position - flowPosition + distantDistance;
                switch (saveData.coverDistanceZMode)
                {
                    case COVER_DISTANCE_Z_MODE.Far:
                        if (IsForward(GetPosition(), flowPosition) == false)
                            nextPosition.z = -nextPosition.z;
                        break;

                    case COVER_DISTANCE_Z_MODE.Near:
                        if (IsForward(GetPosition(), flowPosition) == false)
                            nextPosition.z = -nextPosition.z;
                        break;
                }

                transform.localPosition = nextPosition;
            }
        }

        public void UpdateRotate(Vector3 flowPosition)
        {
            if (saveData.isRotateOnAxis == BOOL.Yes)
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, saveData.coverAngle);
            else
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

            if (saveData.isLookatCenter == BOOL.Yes)
            {
                // Calc angle
                float rate = CalcRate(flowPosition, saveData.rotateInfluenceRange, saveData.rotateCurve);
                float angle = rate * saveData.rotateRate;

                // Rotate - lookat center
                if (saveData.isRotateOnAxis == BOOL.Yes)
                    transform.Rotate(Vector3.up, angle, Space.Self);
                else
                {
                    float coverAngle = -saveData.coverAngle * Mathf.Deg2Rad;
                    Vector3 up = Vector3.zero;
                    up.x = Mathf.Sin(coverAngle);
                    up.y = Mathf.Cos(coverAngle);
                    transform.Rotate(up, angle, Space.Self);
                }
            }
        }

        public virtual void UpdateAlpha(Vector3 flowPosition)
        {
        }

        public virtual void UpdateScale(Vector3 flowPosition)
        {
        }

        public virtual void UpdateDepth(Vector3 flowPosition)
        {
        }

        protected float CalcAlpha(Vector3 flowPosition)
        {
            float percent = 1.0f - CalcPercent(flowPosition, saveData.alphaInfluenceRange, saveData.alphaCurve);
            float alpha = percent * saveData.alphaRate;
            return alpha;
        }

        protected float CalcScale(Vector3 flowPosition)
        {
            float percent = 1.0f - CalcPercent(flowPosition, saveData.scaleInfluenceRange, saveData.scaleCurve);
            float size = 1.0f + (percent * saveData.scaleRate);
            return size;
        }

        protected int CalcDepth(Vector3 flowPosition)
        {
            if (saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Disable)
            {
                float percent = 1.0f - CalcPercent(flowPosition, saveData.depthInfluenceRange, saveData.depthCurve);
                return (int)(saveData.coverDepth.x + ((saveData.coverDepth.y - saveData.coverDepth.x) * percent));
            }
            else
            {
                return -(int)transform.localPosition.z;
            }
        }
    }
}