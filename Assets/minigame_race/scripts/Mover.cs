using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Race
{
    public class Mover : MonoBehaviour
    {
        public enum MoveState
        {
            Wait,
            Scroll
        }
        [SerializeField]
        private MoveState curMoveState = MoveState.Wait;

        [SerializeField]
        private RectTransform rtBgBottom;
        [SerializeField]
        private RectTransform rtBgTop;

        RectTransform mainBg;
        RectTransform subBg;
        public float fSpeed = 3;
        private float fHeight = 0;

        private Vector2 rePos = Vector2.zero;
        private Coroutine comove;
        private void Awake()
        {
            fHeight = rtBgBottom.sizeDelta.y;
            rePos.y = fHeight;
            Application.targetFrameRate = 60; ;
            Debug.Log(Application.targetFrameRate);
        }

        private void SwitchMainBg()
        {
            if (mainBg == rtBgBottom)
            {
                subBg = rtBgBottom;
                mainBg = rtBgTop;
            }
            else
            {
                subBg = rtBgTop;
                mainBg = rtBgBottom;
            }
        }

        IEnumerator MoveBg()
        {
            float timer = Time.time;
            while (true)
            {
                switch (curMoveState)
                {
                    case MoveState.Wait:
                        break;
                }
                float moveval = (800.0f / 3.0f) * Time.deltaTime;
                timer += Time.deltaTime;
                rtBgTop.anchoredPosition -= new Vector2(0, moveval);
                rtBgBottom.anchoredPosition -= new Vector2(0, moveval);
                if (rtBgTop.anchoredPosition.y <= -fHeight)
                {
                    Debug.Log(Time.time - timer);
                    timer = Time.time;
                    rtBgTop.anchoredPosition = rtBgBottom.anchoredPosition + rePos;
                }
                if (rtBgBottom.anchoredPosition.y <= -fHeight)
                {
                    Debug.Log(Time.time - timer);
                    timer = Time.time;
                    rtBgBottom.anchoredPosition = rtBgTop.anchoredPosition + rePos;
                }
                yield return null;
            }
        }
        public void StartMove()
        {
            if (comove != null)
                StopCoroutine(comove);

            comove = null;
            comove = StartCoroutine(MoveBg());
        }
        // Start is called before the first frame update
        void Start()
        {

        }
        private void OnDestroy()
        {
            if (comove != null)
            {
                StopCoroutine(comove);

            }
            comove = null;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
