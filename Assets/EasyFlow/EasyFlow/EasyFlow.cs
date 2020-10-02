using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NAsoft_EasyFlow
{
    public partial class EasyFlow : MonoBehaviour
    {
        [SerializeField]
        public List<Cover> coverList = null;

        [SerializeField]
        public SaveData saveData;

        [SerializeField]
        public List<Connector> callbackList;

        public Transform panel;
        public Camera easyflowCamera;

        public Vector3 flowPosition = Vector3.zero;
        public Vector3 beginPosition = Vector3.zero;

        [SerializeField]
        public Camera dragCamera;

        [SerializeField]
        public Collider dragCollider;

        private void Awake()
        {
            Init();
        }

        private void OnEnable()
        {
            if (saveData == null)
                return;

            switch (saveData.dragMode)
            {
                case DRAG_MODE.OnScreen:
                    {
                        Drag drag = GetComponent<Drag_onScreen>();
                        if (drag == null)
                        {
                            gameObject.AddComponent<Drag_onScreen>();
                            Drag(new Vector2(0.1f, 0.0f));
                        }
                        break;
                    }

                case DRAG_MODE.OnCollider:
                    {
                        Drag drag = GetComponent<Drag_onCollider>();
                        if (drag == null)
                        {
                            gameObject.AddComponent<Drag_onCollider>();
                            Drag(new Vector2(0.1f, 0.0f));
                        }
                        break;
                    }
            }
        }

        private void OnDisable()
        {
        }

        public void _OnMouseDown()
        {
            StopEADProcess();
        }

        public void _OnMouseDrag(Vector2 delta)
        {
            if (saveData.isInverseDrag == BOOL.Yes)
                Drag(-delta);
            else
                Drag(delta);
        }

        public void _OnMouseUp()
        {
            if (saveData.isEffectAfterDrag == BOOL.Yes)
                BeginEADProcess();
            else
            {
                if (saveData.isMoveToNearCover == BOOL.Yes)
                    MoveToNearCover();
            }
        }

        public void Init()
        {
            if (saveData == null)
            {
                Debug.Log("saveData is null");
                return;
            }

            if (callbackList == null)
                callbackList = new List<Connector>();

            InitCover(Vector3.zero);
            InitFlowPosition();
            InitPanelDepth();
        }

        protected virtual void CreateCover()
        {
        }

        public void ChangeMoveTarget()
        {
            // Move Target is [Cover -> Panel]
            if (saveData.isPanelMove == BOOL.Yes)
            {
                if (saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Far)
                    saveData.coverDistanceZMode = COVER_DISTANCE_Z_MODE.Forward;
                else if (saveData.coverDistanceZMode == COVER_DISTANCE_Z_MODE.Near)
                    saveData.coverDistanceZMode = COVER_DISTANCE_Z_MODE.Backward;

                panel.localPosition = beginPosition - flowPosition;
                InitCover(Vector3.zero);
                UpdateCover();
            }
            // Move Target is [Panel -> Cover]
            else
            {
                panel.localPosition = beginPosition;
                InitCover(Vector3.zero);
                UpdateCover();
            }
        }

        public void ChangeCoverCount(int coverCount)
        {
            int addCount = 0;
            if (coverList == null)
            {
                coverList = new List<Cover>(coverCount);
                addCount = coverCount;
            }
            else
            {
                if (coverCount == 0)
                {
                    foreach (Cover cover in coverList)
                    {
                        if (cover != null)
                            DestroyImmediate(cover.gameObject);
                    }
                    coverList.Clear();
                    addCount = 0;
                }
                else if (coverCount == coverList.Count)
                {
                    addCount = 0;
                }
                else if (coverCount > coverList.Count)
                {
                    addCount = coverCount - coverList.Count;
                }
                else if (coverCount < coverList.Count)
                {
                    for (int i = coverCount; i < coverList.Count; ++i)
                    {
                        if (coverList[i] != null)
                            DestroyImmediate(coverList[i].gameObject);
                    }

                    coverList.RemoveRange(coverCount, coverList.Count - coverCount);
                    addCount = 0;
                }
            }

            for (int i = 0; i < addCount; ++i)
                CreateCover();

            Init();
            InitTexture();
        }

        public void ChangeCoverSize(Vector2 size)
        {
            foreach (Cover cover in coverList)
                cover.SetSize(size);
        }

        private void InitCover(Vector3 originPosition)
        {
            int i = 0;
            Vector3 position = Vector3.zero;
            foreach (Cover cover in coverList)
            {
                cover.Init(string.Format("Cover : {0:0##}", i++), this, position + originPosition);
                position += saveData.coverDistance;
            }

            saveData.distanceRange = (saveData.coverDistance * (coverList.Count - 1)).sqrMagnitude;
            ChangeCoverSize(saveData.coverSize);
            UpdateCover();
        }

        private void InitFlowPosition()
        {
            // is new save, substract flow_position from previous position
            beginPosition = panel.localPosition - (-flowPosition);

            if (saveData.isPanelMove == BOOL.Yes)
            {
                flowPosition = (saveData.coverDistance * (float)saveData.beginFlowIndex);
                panel.localPosition = -flowPosition;
            }
            else
            {
                flowPosition = saveData.coverDistance * (float)saveData.beginFlowIndex;
            }
        }

        protected virtual void InitPanelDepth()
        {
        }

        public void UpdateCover()
        {
            if (saveData.isPanelMove == BOOL.Yes)
            {
                panel.localPosition = beginPosition - flowPosition;

                foreach (Cover cover in coverList)
                {
                    cover.UpdateScale(flowPosition);
                    cover.UpdateAlpha(flowPosition);
                    cover.UpdateDepth(flowPosition);
                    cover.UpdateRotate(flowPosition);
                }
            }
            else
            {
                foreach (Cover cover in coverList)
                {
                    cover.UpdatePosition(flowPosition);
                    cover.UpdateScale(flowPosition);
                    cover.UpdateAlpha(flowPosition);
                    cover.UpdateDepth(flowPosition);
                    cover.UpdateRotate(flowPosition);
                }
            }

            // Calculate index of current Cover
            currentIndex = FindNearCoverIndex(flowPosition);

            //CoverSort_inDepth();
        }

        public void MoveTo(Cover cover)
        {
            if (cover == null)
                return;

            Vector3 deltaPosition = cover.GetPosition() - flowPosition;
            dragAccuredDistance = new Vector2(deltaPosition.x, deltaPosition.y);
            BeginEADProcess();
        }

        // is UGUI mode
        // all child: remove, sort, re-attach
        /*		private void CoverSort_inDepth()
                {
                    if (saveData.coverMode == COVER_MODE.UGUI)
                    {
                        int childCount = panel.transform.childCount;
                        if (childCount > 0)
                        {
                            List<Cover> list = new List<Cover>(childCount);
                            foreach (Cover cover in panel.transform.GetComponentsInChildren<Cover>())
                                list.Add(cover);

                            list.Sort((x, y) => {return x.depth.CompareTo(y.depth);});

                            List<Vector3> posList = new List<Vector3>(childCount);
                            foreach (Cover cover in list)
                                posList.Add(cover.transform.localPosition);

                            panel.transform.DetachChildren();

                            for (int i=0; i<childCount; ++i)
                            {
                                list[i].transform.SetParent(panel);
                                list[i].transform.localPosition = posList[i];
                            }
                        }
                    }
                }
        */
    }
}