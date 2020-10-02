using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
/// <summary>
/// 풍선
/// 재사용 가능하도록 설계해야함
/// </summary>
namespace Baloon
{
    public class Baloon : MonoBehaviour
    {
        static int HALFWIDTH  = 640;
        static int HALFHEIGHT = 400;
        //ㅌㅏㅇㅣㅂ
        public enum BaloonType
        {
            None = -1,
            Item_1,
            Item_2,
            Item_3,
            Item_4,
            Item_5,
            Correct,
            InCorrect,

        }
        public enum State
        {
            Wait,
            Move,
            TouchAni
        }
        //type
        [SerializeField]
        public BaloonType type = BaloonType.None;
        //정답 정보
        private bool _iscorrect = false;

        public bool correctInfo
        {
            get { return _iscorrect; }
        }

        //현재 상태(액티브 , 웨이트)
        public State curState = State.Wait;
        //이미지
        [SerializeField]
        private Image img;
        // btn
        [SerializeField]
        private Button btn;
        // 풍선 속돌
        [SerializeField]
        private float speed = 5;
        // 풍선 이미지(정오답 2개)
        [SerializeField]
        private Sprite[] arrSprite;
        // 풍선 이미지(정오답 2개)
        [SerializeField]
        private Animator animator;

        private BaloonController controller;
        private System.Action<Baloon> touchevent;
        private Tweener descr;
        private RectTransform rt;
       
        private float targetYpos;
        private float minX;
        private float maxX;
        private Vector2 setpos = Vector2.zero;
        float moveTime;
        bool bIsSlowTime;
        // Start is called before the first frame update
        void Awake()
        {
            rt = this.GetComponent<RectTransform>();
            targetYpos = 400 + (img.rectTransform.sizeDelta.y * 0.5f);

            minX = -640 + rt.sizeDelta.x * 0.5f;
            maxX = 640 - rt.sizeDelta.x * 0.5f;
        }

        // Update is called once per frame
        void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.Q))
            {
                this.gameObject.SetActive(true);
                StartBaloon();
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                StopBaloon();
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
               

                //SetSpeedB = 10;// (10);// = 10;
            }

#endif

        }

        public void setController(BaloonController _controller)
        {
            controller = _controller;
        }
        /// <summary>
        ///  풍선 오브젝트를 생성할때 
        /// </summary>
        /// <param name="_event"> 버튼 터치 이벤트  </param>
        /// <param name="_type"> 풍선 타입(정오답)</param>
        /// <param name="_state">풍선의 현재 상태(동작중인지 대기상태인지)</param>
        
        public void SetData(System.Action<Baloon> _event, BaloonType _type, float _movespeed, bool _isslow, State _state = State.Move)
        {
            touchevent = _event;
            curState = _state;
            SetType(_type);
            SetSprite(_type);
            speed = _movespeed;
            bIsSlowTime = _isslow;
        }
        /// <summary>
        /// 풍서 타입 설정(정답 풍선인지 아닌지)
        /// </summary>
        /// <param name="_type"></param>
        public void SetType(BaloonType _type)
        {
            if (_type == BaloonType.None)
            {
                Debug.LogError("풍선설정은 none이 될 수 없습니다.");
                return;
            }
            type = _type;
            if (type == BaloonType.Correct)
            {
                _iscorrect = true;
            }
            //오답 또는 아이템
            else
            {
                _iscorrect = false;
            }
             
        }

        /// <summary>
        /// 풍섡 무빙 시작
        /// </summary>
        public void StartBaloon()
        {
            Debug.Log("start move");
            this.gameObject.SetActive(true);
            //화면 아래로 위치 이동 , 화면 크기 이내의 랜덤 x 값 설정
            rt.SetAsLastSibling();
            rt.anchoredPosition = GetRandomPos();
            //이동 시작
            moveTime = Random.Range(speed, speed + 2);
           
            //Random.Range(speed, speed)
            descr = rt.DOAnchorPosY(targetYpos, moveTime).OnComplete(()=> {
                StopBaloon();

            }).SetEase(Ease.Linear);
            if (bIsSlowTime)
                descr.timeScale = 0.5f;
            else
                descr.timeScale = 1f;


            //상태 변경 
            curState = State.Move;
            img.raycastTarget = true;
            TEMP_PlayBaloonAni("Baloon_idle");
        }
        private Vector2 GetRandomPos()
        {
            setpos.x = Random.Range(minX, maxX);
            setpos.y = -400 - rt.sizeDelta.y;
            return setpos;
        }
        /// <summary>
        /// 이동만 정지
        /// </summary>
        public void PauseBaloon()
        {
            if (descr != null)
            {
                descr = descr.Pause();
            }
            else
            {
                Debug.Log("tweener is null");
            }
            
        }
        public void ResumeBaloon()
        {
            if (descr != null)
            {
                descr.Play();
            }
            else
            {
                Debug.Log("tweener is null");
            }
        }
        public void StopBaloon()
        {
//            Debug.Log("StopBaloon");
            if (descr != null)
            {
                descr.Kill();
            }
            descr = null;
            curState = State.Wait;
            this.gameObject.SetActive(false);
           // img.raycastTarget = false;
        }
        /// <summary>
        /// 풍선 터치
        /// </summary>
        public void OnClickBaloon()
        {
            //해당 타입에 맞는 사운드 재



            //목표 달성 후에 클릭 
            if (controller.bIsCoplete) return;
             
            img.raycastTarget = false;
            if (descr != null)
            {
                descr.Kill();
                
            }
            descr = null;
            //animation
            if (_iscorrect)
            {
                TEMP_PlayBaloonAni("Baloon_Ani");
            }
            else
            {
                TEMP_PlayBaloonAni("Baloon_Ani2");
            }
            if (touchevent != null)
            {
                Debug.Log("touchevent");
                touchevent(this);
            }
            else
            {
                Debug.LogError("touch event is null");
            }
        }

        private void endTouchAnimation()
        {
            StopBaloon();
          
        }    


        /// <summary>
        /// 풍선 이미지 설정
        /// </summary>
        /// <param name="_type"></param>
        public void SetSprite(BaloonType _type)
        {

            //test code
            switch (_type)
            {
                case BaloonType.Correct:
                    img.color = Color.yellow;
                    break;
                case BaloonType.InCorrect:
                    img.color = Color.blue;
                    break;
                case BaloonType.Item_1:
                    img.color = Color.green;
                    break;
                case BaloonType.Item_2:
                    img.color = Color.cyan;
                    break;
                case BaloonType.Item_3:
                    img.color = Color.black;
                    break;
                case BaloonType.Item_4:
                    img.color = Color.red;
                    break;
                case BaloonType.Item_5:
                    img.color = Color.magenta;
                    break; 
            }
            

            return;
            // 
            if (arrSprite.Length != 2)
                return;

            img.sprite = arrSprite[(int)_type];
            img.SetNativeSize();
            

        }


        public void TEMP_PlayBaloonAni(string _name)
        {
            animator.Play(_name);
        }

        public void setSpeedTime(float _time)
        {
            descr.Kill();
            
            descr = rt.DOAnchorPosY(targetYpos, _time * 100f);
        }
        public void SpeedDown(   )
        {
            if (descr == null) return;

            descr.timeScale = 0.5f; 
            //descr.Kill(); 
            //descr = rt.DOAnchorPosY(targetYpos, moveTime * 2  );
        }
        public void SpeedReturn()
        {
            descr.timeScale = 1f; 
        }

    }

}
