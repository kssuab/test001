using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
namespace Race
{
    public class RaceCar : MonoBehaviour,  IBeginDragHandler , IDragHandler , IEndDragHandler 
    {
        public enum AniState
        {
            Foward,
            Left,
            Right,
            Crash,

        }
        public enum MoveDir
        {
            Foward,
            Left,
            Right,
        }
        [SerializeField]
        private Canvas canvas;
        [SerializeField]
        private AniState curAniState = AniState.Foward;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private RectTransform rtCar;
        [SerializeField]
        private RectTransform rtDragBoard;
        [SerializeField]
        private float startMoveTime = 2.0f;
        float Height = 800;
        Vector3 prevPos = Vector3.zero;
        Vector2 localpos = Vector2.zero;
        
        float minX = -400;
        float maxX = 400;

        float ffixheight = -300;

        public float fchangeDis = 3.0f;
        public float returntime = 0.0f;
        public float returnwaittime = 1.0f;
        private Vector3 waitpos = Vector3.zero;
       
        public void ShowCar()
        {
            this.gameObject.SetActive(true);
            rtCar.anchoredPosition = new Vector2(0, -(Height  ) - (rtCar.sizeDelta.y / 2));

            rtCar.DOAnchorPosY(-300, 1.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                RaceManager.instance.EndShowCar();
            });


        }
        
        private void Start()
        {
          
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            Debug.Log("test");
        }
        public void OnBeginDrag(PointerEventData eventData)
        {
            Debug.Log("begin");
            prevPos = Input.mousePosition;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rtDragBoard, eventData.position, eventData.pressEventCamera, out localpos))
            {
               
                MoveCar();
            }
          
        }
        private MoveDir GetDir() 
            {
            if (prevPos.x < Input.mousePosition.x)
            {
                return MoveDir.Right;
                
            }
            else if (prevPos.x > Input.mousePosition.x)
            {
                return MoveDir.Left;            }
            else
            {
                return MoveDir.Foward;
            }

        }
        public void MoveCar(   )
        {
            //최대 최하치를 넘어가는 이동값일 경우
            if (localpos.x <= minX)
            {
                localpos.x = minX;
                ChangeAniState(AniState.Foward);
            }
            //최대 최하치를 넘어가는 이동값일 경우
            else if (localpos.x >= maxX)
            {
                localpos.x = maxX;
                ChangeAniState(AniState.Foward);
            }
            else
            {
                float absprev = Mathf.Abs(prevPos.x);
                float absnow = Mathf.Abs(Input.mousePosition.x);

                switch (curAniState)
                {
                    case AniState.Left:
                        if (GetDir() == MoveDir.Right    )
                        {
                            if (Mathf.Abs(absprev - absnow) < fchangeDis)
                            {
                                ChangeAniState(AniState.Foward);
                            }
                            else
                            {
                                ChangeAniState(AniState.Right);
                            } 
                        } 
                        break;
                    case AniState.Right:
                        if (GetDir() == MoveDir.Left)
                        {
                            if (Mathf.Abs(absprev - absnow) < fchangeDis)
                            {
                                ChangeAniState(AniState.Foward);
                            }
                            else
                            {
                                ChangeAniState(AniState.Left);
                            }

                        }
                        break;
                    case AniState.Foward:
                        if (Mathf.Abs(absprev - absnow) < fchangeDis)
                        {
                            ChangeAniState(AniState.Foward);
                        }
                        else if (Input.mousePosition.x > prevPos.x)
                        {
                            ChangeAniState(AniState.Right);
                        }
                        else if (Input.mousePosition.x < prevPos.x)
                        {
                            ChangeAniState(AniState.Left);

                        }
                        break;
                } 
                prevPos = Input.mousePosition;
            }

            localpos.y = ffixheight;
            rtCar.anchoredPosition = localpos; 
        }
        private void WaitReturn()
        {
            if (waitpos.x == Input.mousePosition.x)
            {
                returntime += Time.deltaTime;

                if (returntime > returnwaittime)
                {
                    ChangeAniState(AniState.Foward);
                    returntime = 0.0f;
                }
            }
            waitpos.x = Input.mousePosition.x; 
        }
        private void Update()
        {
            WaitReturn(); 
        } 
        public void OnEndDrag(PointerEventData eventData)
        {
            Debug.Log("end");
            if (curAniState == AniState.Crash)
            {
                return;
            }

            ChangeAniState(AniState.Foward);
        } 
        private void ChangeAniState(AniState state)
        {
            curAniState = state;
            switch (curAniState)
            {
                case AniState.Foward:
                    animator.Play("Foward");
                    break;
                case AniState.Left:
                    
                    animator.Play("Left");
                    break;
                case AniState.Right:
                    
                    animator.Play("Right");
                    break;
                case AniState.Crash:
                    animator.Play("Crash");
                    break;
            }
        }
    }
}