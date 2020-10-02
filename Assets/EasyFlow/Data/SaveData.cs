using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    public enum COVER_MODE
    {
        Disabled,
        NGUI,
        Quad,
        UGUI
    }

    public enum CLICK_MODE
    {
        None = 0,
        MoveTo,
        ClickCenter,
        ClickAll,
        MoveToAndClickCenter,
        MoveToAndClickAll,
        Custom
    }

    public enum TEXTURE_MODE
    {
        Once,
        Loop,
        Random
    }

    public enum DRAG_MODE
    {
        OnScreen,
        OnCollider
    }

    public enum BOOL
    {
        No = 0,
        Yes = 1
    }

    public enum COVER_DISTANCE_Z_MODE
    {
        Disable = 0,
        Forward,
        Backward,
        Far,
        Near
    }

    [Serializable]
    public class SaveData : ScriptableObject
    {
        #region Cover

        public int coverCount;
        public int coverCountLimit;
        public float m_coverDistanceF;

        public float coverDistanceF
        {
            get
            {
                return m_coverDistanceF;
            }
            set
            {
                m_coverDistanceF = value;
                UpdateCoverDistanceXY();
            }
        }

        public float coverDistanceLimit;
        public float distanceRange;
        public Vector3 coverDistance;
        public float m_coverAngle;

        public float coverAngle
        {
            get
            {
                return m_coverAngle;
            }
            set
            {
                m_coverAngle = value;
                UpdateCoverDistanceXY();
            }
        }

        public Vector2 coverSize;
        public Vector2 coverSizeLimit;
        public float coverDistanceZ;
        public float coverDistanceZLimit;
        public COVER_DISTANCE_Z_MODE m_coverDistanceZMode;

        public COVER_DISTANCE_Z_MODE coverDistanceZMode
        {
            get
            {
                return m_coverDistanceZMode;
            }
            set
            {
                m_coverDistanceZMode = value;
                switch (m_coverDistanceZMode)
                {
                    case COVER_DISTANCE_Z_MODE.Disable: coverDistance.z = 0.0f; break;
                    case COVER_DISTANCE_Z_MODE.Forward: coverDistance.z = coverDistanceZ; break;
                    case COVER_DISTANCE_Z_MODE.Backward: coverDistance.z = -coverDistanceZ; break;
                    case COVER_DISTANCE_Z_MODE.Far: coverDistance.z = coverDistanceZ; break;
                    case COVER_DISTANCE_Z_MODE.Near: coverDistance.z = -coverDistanceZ; break;
                }
            }
        }

        #endregion Cover

        #region Click

        public CLICK_MODE clickMode;

        #endregion Click

        #region Texture

        public List<Texture> textureList;
        public TEXTURE_MODE textureMode;

        #endregion Texture

        #region Drag

        public float dragPower;
        public float dragPowerLimit;

        public DRAG_MODE dragMode;
        public Rect dragRect;

        public BOOL isInverseDrag;
        public BOOL isDragOnAxis;
        public BOOL isEffectAfterDrag;
        public BOOL isMoveToNearCover;

        #endregion Drag

        #region Effect After Drag

        public float effectAfterDragTime;
        public float effectAfterDragTimeLimit;
        public AnimationCurve effectAfterDragCurve;

        #endregion Effect After Drag

        #region Position

        public float positionRate;
        public float positionRateLimit;

        public float positionInfluenceRange;
        public float positionInfluenceRangeLimit;

        public AnimationCurve positionCurve;

        #endregion Position

        #region Rotate

        public BOOL isLookatCenter;
        public BOOL isRotateOnAxis;

        public float rotateRate;
        public float rotateRateLimit;

        public float rotateInfluenceRange;
        public float rotateInfluenceRangeLimit;

        public AnimationCurve rotateCurve;

        #endregion Rotate

        #region Scale

        public float scaleRate;
        public Vector2 scaleRateLimit;  // min ~ max

        public float scaleInfluenceRange;
        public float scaleInfluenceRangeLimit;

        public AnimationCurve scaleCurve;

        #endregion Scale

        #region Alpha

        public float alphaRate;
        public float alphaRateLimit;

        public float alphaInfluenceRange;
        public float alphaInfluenceRangeLimit;

        public AnimationCurve alphaCurve;

        #endregion Alpha

        #region Depth

        public float panelDepth;
        public Vector2 panelDepthLimit; // min ~ max

        public Vector2 coverDepth;  // min ~ max
        public Vector2 coverDepthLimit; // min ~ max

        public float depthInfluenceRange;
        public float depthInfluenceRangeLimit;

        public AnimationCurve depthCurve;

        #endregion Depth

        #region Other

        public BOOL isPanelMove;
        public float beginFlowIndex;
        public COVER_MODE coverMode;

        #endregion Other

        private void UpdateCoverDistanceXY()
        {
            coverDistance.x = Mathf.Cos(coverAngle * Mathf.Deg2Rad) * coverDistanceF;
            coverDistance.y = Mathf.Sin(coverAngle * Mathf.Deg2Rad) * coverDistanceF;
            if (Mathf.Abs(coverDistance.x) < 0.0001f)
                coverDistance.x = 0.0f;
            if (Mathf.Abs(coverDistance.y) < 0.0001f)
                coverDistance.y = 0.0f;
        }

        public SaveData()
        {
            Init();
        }

        private void Init()
        {
            #region Cover

            coverCount = 0;
            coverCountLimit = 100;

            coverSize = new Vector2(120.0f, 90.0f);
            coverSizeLimit = new Vector2(200.0f, 200.0f);

            coverAngle = 0.0f;
            coverDistanceF = 100.0f;
            coverDistanceLimit = 200.0f;
            distanceRange = 0.0f;

            coverDistanceZ = 0.0f;
            coverDistanceZLimit = 200.0f;
            coverDistanceZMode = COVER_DISTANCE_Z_MODE.Disable;

            #endregion Cover

            #region Click

            clickMode = CLICK_MODE.None;

            #endregion Click

            #region Texture

            textureList = new List<Texture>();
            textureMode = TEXTURE_MODE.Loop;

            #endregion Texture

            #region Drag

            dragPower = 1.0f;
            dragPowerLimit = 3.0f;

            dragMode = DRAG_MODE.OnScreen;
            dragRect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);

            isInverseDrag = BOOL.No;
            isDragOnAxis = BOOL.Yes;

            #endregion Drag

            #region Effect After Drag

            isEffectAfterDrag = BOOL.Yes;
            isMoveToNearCover = BOOL.Yes;

            effectAfterDragTime = 1.0f;
            effectAfterDragTimeLimit = 3.0f;

            effectAfterDragCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f, 0.0f, 3.0f), new Keyframe(1.0f, 1.0f, 0.0f, 0.0f) });

            #endregion Effect After Drag

            #region Position

            positionRate = 1.0f;
            positionRateLimit = 10.0f;

            positionInfluenceRange = 200.0f;
            positionInfluenceRangeLimit = 1000.0f;

            positionCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f, 0.0f, 3.0f), new Keyframe(1.0f, 1.0f, 0.0f, 0.0f) });

            #endregion Position

            #region Rotate

            isLookatCenter = BOOL.Yes;
            isRotateOnAxis = BOOL.Yes;

            rotateRate = 60.0f;
            rotateRateLimit = 120.0f;

            rotateInfluenceRange = 100.0f;
            rotateInfluenceRangeLimit = 200.0f;

            rotateCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f, 0.0f, 3.0f), new Keyframe(1.0f, 1.0f, 0.0f, 0.0f) });

            #endregion Rotate

            #region Scale

            scaleRate = 0.5f;
            scaleRateLimit = new Vector2(-1.0f, 2.0f);

            scaleInfluenceRange = 100.0f;
            scaleInfluenceRangeLimit = 200.0f;

            scaleCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

            #endregion Scale

            #region Alpha

            alphaRate = 1.0f;
            alphaRateLimit = 2.0f;

            alphaInfluenceRange = 300.0f;
            alphaInfluenceRangeLimit = 600.0f;

            alphaCurve = AnimationCurve.EaseInOut(0.0f, 0.0f, 1.0f, 1.0f);

            #endregion Alpha

            #region Depth

            panelDepth = 0.0f;
            panelDepthLimit = new Vector2(0.0f, 100.0f);

            coverDepth = new Vector2(0.0f, 1000.0f);
            coverDepthLimit = new Vector2(-1000.0f, 1000.0f);

            depthInfluenceRange = 500.0f;
            depthInfluenceRangeLimit = 1000.0f;

            depthCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 0.0f, 0.0f, 3.0f), new Keyframe(1.0f, 1.0f, 0.0f, 0.0f) });

            #endregion Depth

            #region Other

            isPanelMove = BOOL.No;
            beginFlowIndex = 0.0f;
            coverMode = COVER_MODE.Disabled;

            #endregion Other
        }
    }
}