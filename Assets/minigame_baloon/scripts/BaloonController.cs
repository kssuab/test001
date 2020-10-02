using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Baloon
{
    public class BaloonController : MonoBehaviour
    {
       
        [SerializeField]
        private CanvasGroup group;

        [SerializeField]
        private Text txtScore;
        private int  nScore;

        private bool _isComplete = false;
        public bool bIsCoplete
        {
            get { return _isComplete; }
        }
        [SerializeField]
        private int correctCount = 0;
        private int targetCount = 10;
        List<Baloon> lstBaloon;
        [SerializeField]
        private BaloonRespone respone;
        /// <summary>
        ///  true = 아이템을 생성하는 순간 카운트가 줄어듬 , false = 아이템 풍선을 터트리지 않으면 카운트가 줄지 않음
        /// </summary>
        [SerializeField]
        private bool isCountItems;
        [SerializeField]
        private Text txtTime;
        [SerializeField]
        private float fGameTimer = 10;
        Tween Timer;


        int totalscore = 0;
        int addscore = 100;
        bool bIsClear = false;

        // 스테이지 정보
        //


        /// <summary>
        /// items 1 (몇초동안 보너스 점수) 
        /// </summary>
        Tween AddScoreTime;
        float fAddScoreTimeTime = 5f;
        public bool bisAddScoreTime = false;
        /// <summary>
        /// items 2 (몇초동안 풍선 올라가는 속도 감속)
        /// </summary>
        Tween SlowTime;
        float fSlowTime = 2f;
        public bool bisSlowTime = false;
        /// <summary>
        /// items 3 (몇초동안 풍선 이동 정지)
        /// </summary>
        Tween StopTime;
        float fStopTime = 1f;
        public bool bisStopTime = false;
        /// <summary>
        /// item 4 (피버 타임, 모든 풍선을 (아이템포함??) 점수화 시킨다.)
        /// </summary>
        Tween FeverTime;
       // float fFeverTime = 5f;
       public bool bisFeverTime = false;
    
        #region Items

        /// <summary>
        /// 정해진 시간동안 정답 풍선을 터치하면 보너스 점수 획득
        /// </summary>
        private void StartAddScoreTime()
        {
            if (AddScoreTime != null)
                AddScoreTime.Kill();

            AddScoreTime = null;
            bisAddScoreTime = true;
            AddScoreTime = DOVirtual.DelayedCall(fAddScoreTimeTime, () =>
            {
                bisAddScoreTime = false;

            });
        }
        /// <summary>
        /// 정해진 시간동안 풍선의 이동 속도 감속
        /// </summary>
        private void StartSlowTime()
        {
            // StopTime중 감속 아이템 터치시 무시
            if (bisStopTime) return;
            if (SlowTime != null)
                SlowTime.Kill();

            SlowTime = null;

            // 풍선 이동속도 감속
            List<Baloon> _lst = respone.GetBaloons();
            bisSlowTime = true;
            respone.SetSlowTime(bisSlowTime);
            AllSpeedDown();
            SlowTime = DOVirtual.DelayedCall(fSlowTime, () =>
            {
                bisSlowTime = false;
                respone.SetSlowTime(bisSlowTime);
                AllSpeedReturn();
            });

        }
        /// <summary>
        /// 정해진 시간동안 풍선 정 
        /// </summary>
        private void StartStopTime()
        {
            if (StopTime != null)
                StopTime.Kill();

            PauseTimer();
            StopTime = null;
            bisStopTime = true;
            List<Baloon> _lst = respone.GetBaloons();
            respone.SetIgnoreRespone(true);
            //풍선 이동 정지
            AllPause();
            StopTime = DOVirtual.DelayedCall(fStopTime, () =>
            {
                bisStopTime = false;
                AllResume();
                respone.SetIgnoreRespone(false);
                ResumeTimer();
            }); 
        }
        /// <summary>
        /// 모든 풍선을 점수화(현재 아이템 풍선 및 오답 풍선도  동일한 점수로 판별한다(2020.0921 김인웅))
        /// </summary>
        private void StartFeverTime()
        {
            //클릭금지( 게임내에서만)
            SetBlock(true);

            //시간정지 아이템 동작중이면 정지
            if (StopTime != null)
                StopTime.Kill();

            PauseTimer();
            //리스폰 정지
            respone.SetIgnoreRespone(true); 
            //마지막 풍선 애니메이션 대기 시간
            float _timer = 0.0f;
            //풍선 이동 정지
            List<Baloon> _lst = respone.GetBaloons();
            for (int i = 0; i < _lst.Count; i++)
            {
                //각 풍선 점수화시켜서 터트리기(풍선 종류 상관 없는지 ?? 지금은 전부타  터트리고 정담 풍선만 점수 계산 할것 )
                if (_lst[i].curState == Baloon.State.Move)
                {
                    _lst[i].PauseBaloon();
                }
                int _index = i;
              
                DOVirtual.DelayedCall(_index * 0.3f, () =>
                {
                    if (_lst[_index].curState == Baloon.State.Move)
                    {
                        _lst[_index].TEMP_PlayBaloonAni("Baloon_Ani");
                    }
                    correctCount++;

                    if (bisAddScoreTime)
                        nScore += 200;
                    else
                        nScore += 100;
                    txtScore.text = nScore.ToString(); 
                });
            }
            _timer = (_lst.Count * 0.3f) + 1.0f  ;
            DOVirtual.DelayedCall(_timer, () =>
            {
                if (checkComplete())
                {
                    _isComplete = true;
                    bIsClear = true;
                    SetBlock(false);
                    EndGame();
                }
                else
                {
                    SetBlock(false);
                    AllResume();
                    respone.SetIgnoreRespone(false);
                    ResumeTimer();
                }
            });

           
           
        }
        private void AllSpeedDown()
        {
            List<Baloon> _lst = respone.GetBaloons();
            for (int i = 0; i < _lst.Count; i++)
            {
                if (_lst[i].curState == Baloon.State.Move)
                {
                    _lst[i].SpeedDown();
                }
            }
        }
        private void AllSpeedReturn()
        {
            List<Baloon> _lst = respone.GetBaloons();
            for (int i = 0; i < _lst.Count; i++)
            {
                if (_lst[i].curState == Baloon.State.Move)
                {
                    _lst[i].SpeedReturn();
                }
            }
        }
        private void AllPause()
        {
            List<Baloon> _lst = respone.GetBaloons();
            for (int i = 0; i < _lst.Count; i++)
            {
                if (_lst[i].curState == Baloon.State.Move)
                {
                    _lst[i].PauseBaloon();
                }
            }
        }
        private void AllResume()
        {
            List<Baloon> _lst = respone.GetBaloons();
            for (int i = 0; i < _lst.Count; i++)
            {
                if (_lst[i].curState == Baloon.State.Move)
                {
                    _lst[i].ResumeBaloon();
                }
            }
        }

        #endregion

        #region Timer
        private void ResetTimer()
        {
            
        }
        private void PauseTimer()
        {
            if (Timer != null)
                Timer.Pause();
        }
        private void ResumeTimer()
        {
            if (Timer != null)
                Timer.Play();
        }
        private void KillTimer()
        {
            if (Timer != null)
                Timer.Kill();

            Timer = null;
        }
        private void StartTimer()
        {
            if (Timer != null)
                Timer.Kill();

            Timer = null;

            Timer = DOVirtual.Float(fGameTimer, 0, fGameTimer, (value) =>
            {
                txtTime.text = ((int)value).ToString();

            }).SetEase(Ease.Linear)
              .OnComplete(EndGame);
        }


        #endregion


        private void Start()
        {
#if UNITY_EDITOR
            GameStart();
#endif
        }
        public void Awake()
        {


        }
        private void GameStart()
        {
            Reset();
            respone.startBaloonUp();
            StartTimer();
        }
        private void Update()
        {
#if UNITY_EDITOR
            if (Input.GetKeyDown(KeyCode.F))
            {

                GameStart();
            }
            if (Input.GetKeyDown(KeyCode.G))
            {

                EndGame();
            }


            if (Input.GetKeyDown(KeyCode.Y))
            {

                StartAddScoreTime();
            }
            if (Input.GetKeyDown(KeyCode.U))
            {
                StartSlowTime();
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                StartStopTime();
            }
            if (Input.GetKeyDown(KeyCode.O))
            {
                StartFeverTime();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {

            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                Time.timeScale = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                Time.timeScale = 1;
            }
#endif



        }
      
        private void CheckClickedBaloon(Baloon _baloon)
        {
            if (_isComplete)
            {
                Debug.Log("게임이 종료되었습니다.");
                return;
            }
            Debug.Log(_baloon.correctInfo);
            //아이템인지 판별
            if (_baloon.type != Baloon.BaloonType.Correct && _baloon.type != Baloon.BaloonType.InCorrect)
            {
                //아이템 처리
                OnClickItem(_baloon.type);
            }
            else
            {
                if (_baloon.correctInfo)
                {
                    if (bisAddScoreTime)
                        nScore += 200;
                    else
                        nScore += 100;
                    txtScore.text = nScore.ToString();

                    correctCount++;
                    if (checkComplete())
                    {
                        //결과 데이터 저장
                        _isComplete = true;
                        bIsClear = true;
                        DOVirtual.DelayedCall(1.5f, EndGame);
                    }
                }
            }

        }
        private void OnClickItem(Baloon.BaloonType _type)
        {
            switch (_type)
            {
                case Baloon.BaloonType.Item_1:
                    StartAddScoreTime();
                    break;
                case Baloon.BaloonType.Item_2:
                    StartSlowTime();
                    break;
                case Baloon.BaloonType.Item_3:
                    StartStopTime();
                    break;
                case Baloon.BaloonType.Item_4:
                    StartFeverTime();
                    break;
                case Baloon.BaloonType.Item_5:
                    break;
            }



        }


        /// <summary>
        /// 동작 중인 생성트윈 정지
        /// </summary>
        private void OnDisable()
        {
            respone.StopRespone();
        }
        private bool checkComplete()
        {
            if (correctCount >= targetCount)
            {
                return true;
            }
            return false;
        }
      


        public void EndGame()
        {
            Debug.Log("EndGame");
            //ㄹㅣ스폰 정지
            respone.StopRespone();
            _isComplete = true;
            KillTimer();
        }  
        public void Reset()
        {


            //게임 스테이지 관련 정보 받아서 설정,

            //Respone
            respone.SetIgnoreRespone(false);
            bIsClear = false;
            respone.SetData(this, 0.7f, 3, 3.7f,4,3);
            correctCount = 0;
            _isComplete = false;
            respone.Reset();
            respone.SetBaloonClickListener(CheckClickedBaloon);
            //UI
            nScore = 0;
            txtScore.text = "";
        } 
        /// <summary>
        /// 게임 내에 클릭 금지
        /// </summary>
        /// <param name="_isblock"></param>
        private void SetBlock(bool _isblock)
        {
            group.blocksRaycasts = !_isblock;
        }
        private void OnApplicationFocus(bool focus)
        {
            Debug.Log("Focus   " + focus.ToString());

            if (focus)
            {
                //정지 팝업이 띄워저 있는지 확인 후
                //없으면 게임 정지 후 팝업 띄우ㅁ
            }

        }
    }
}
