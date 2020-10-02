using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Baloon
{
    public class BaloonRespone : MonoBehaviour
    {

        [SerializeField]
        private BaloonController controller;

        System.Action<Baloon> clicklistener;

        [SerializeField]
        private Baloon baloonori;
        [SerializeField]
        private RectTransform parent;

        //전체 풍선 리스트
        List<Baloon> lstBaloon = new List<Baloon>();

        //리스폰 타이머 (정답)
        [SerializeField]
        private float fRes_Cotime = 2;

        //리스톤 타이머 (오답)
        [SerializeField]
        private float fRes_Incotime = 3;
        //리스톤 타이머 (아이템)
        [SerializeField]
        private float fRes_Itemtime = 3; 
        /// <summary>
        /// 사용 가능한 아이템 종류 개수
        /// </summary>
        private int canMaxItemCount = 3;

        /// <summary>
        /// 사용 가능한 각 아이템 당 개수
        /// </summary>
        private int useMaxItemCount = 3;

        /// <summary>
        /// 아이템 개수 저장 
        /// </summary>
        List<int> lstItem = new List<int>();
        List<Baloon.BaloonType> lstBaloonType = new List<Baloon.BaloonType>();


        Tween dt_correct;
        Tween dt_incorrect;
        Tween dt_item;

        private bool bIsSlowTime = false;
        private bool bIsPauseRespone = false;

        /// <summary>
        ///  
        /// </summary>
        public void SetData(BaloonController _controller , float correcttime , float incorrecttime , float itemtime , int _canMaxItemCount , int _useMaxItemCount)
        {
            fRes_Cotime = correcttime;
            fRes_Incotime = incorrecttime;
            fRes_Itemtime = itemtime;
            controller = _controller;
            canMaxItemCount = _canMaxItemCount;
            useMaxItemCount = _useMaxItemCount; 
        }
        public void SetData(float   correcttime , float incorrecttime, float itemtime, int _canMaxItemCount, int _useMaxItemCount)
        {
            
            fRes_Cotime = correcttime;
            fRes_Incotime = incorrecttime;
            fRes_Itemtime = itemtime;
                canMaxItemCount = _canMaxItemCount;
                useMaxItemCount = _useMaxItemCount;
        }

        public void SetSlowTime(bool _isslowtime)
        {
            bIsSlowTime = _isslowtime;
        }

        public void Reset( )
        {
            lstItem.Clear();
            lstBaloonType.Clear();
            lstItem.Add(useMaxItemCount);
            lstItem.Add(useMaxItemCount);
            lstItem.Add(useMaxItemCount);
            lstItem.Add(useMaxItemCount);
            lstItem.Add(useMaxItemCount);
           
            for (int i = 0; i < canMaxItemCount; i++)
            {
                lstBaloonType.Add((Baloon.BaloonType)i);
            }

        }
        /// <summary>
        /// 풍서 터치시 실행해줄 리스터 등 
        /// </summary>
        /// <param name="listener"></param>
        public void SetBaloonClickListener(System.Action<Baloon> listener)
        {
            clicklistener = listener;
        }
        public List<Baloon> GetBaloons()
        {
            return lstBaloon;
        }
        /// <summary>
        /// 벌룬 생성 타이머 시작
        /// </summary>
        public void startBaloonUp()
        {
             
            respone_Correct();
            respone_InCorrect();
            respone_Item();
        }
        private void respone_Correct()
        {
            dt_correct = DOVirtual.DelayedCall(fRes_Cotime, () =>
            {
                if(!bIsPauseRespone)
                AddBaloon(true);
                respone_Correct();
            });
        }
        private void respone_InCorrect()
        {
            dt_incorrect = DOVirtual.DelayedCall(fRes_Incotime, () =>
            {
                if (!bIsPauseRespone)
                    AddBaloon(false);
                respone_InCorrect();
            });
        }
        private void respone_Item()
        {

            dt_item = DOVirtual.DelayedCall(fRes_Itemtime, () =>
            {
                Baloon.BaloonType _type = GetItem();
                // Debug.Log("respone_Item");
                if (_type != Baloon.BaloonType.None)
                {
                    if (!bIsPauseRespone)
                        AddBaloonItem(_type);
                    respone_Item();
                }
                else
                {
                    Debug.Log("item zero ");
                    dt_item.Kill();
                }
            });
        } 
        public void StopRespone()
        {
            if (dt_correct != null) { dt_correct.Kill(); }
            if (dt_incorrect != null)  {dt_incorrect.Kill(); }
            if (dt_item != null) { dt_item.Kill();  }
            for (int i = 0; i < lstBaloon.Count; i++)
            {
                if (lstBaloon[i].curState == Baloon.State.Move)
                {
                    lstBaloon[i].StopBaloon();
                }
            }
        }
        #region Baloon Create
        /// <summary>
        /// 풍선 생성(기존에 대기중인 풍선이 존재한다면 대기풍선 리턴)
        /// </summary>
        /// <returns></returns>
        private Baloon createBaloon()
        {
            Baloon _bal = null;
            //현재 대기중인 풍선 검색
            _bal = findWaitBaloon();

            if (_bal != null)
            {
                return _bal;
            }
            GameObject _obj = Instantiate(baloonori.gameObject, parent);
            _obj.transform.localScale = Vector3.one;
            _bal = _obj.GetComponent<Baloon>();
            _bal.setController(controller);
            lstBaloon.Add(_bal);
            return _bal;
        }

        /// <summary>
        /// 풍서 등록
        /// </summary>
        /// <param name="_iscorrect"></param>
        private void AddBaloon(bool _iscorrect)
        {
            Baloon _bal = createBaloon();
            if (_iscorrect)
            {
                _bal.SetData(clicklistener, Baloon.BaloonType.Correct, 3, bIsSlowTime, Baloon.State.Move  );
            }
            else
            {
                _bal.SetData(clicklistener, Baloon.BaloonType.InCorrect, 3, bIsSlowTime,Baloon.State.Move   );
            }
            _bal.StartBaloon();
        }
        /// <summary>
        /// 풍서 등록
        /// </summary>
        /// <param name="_iscorrect"></param>
        private void AddBaloonItem(Baloon.BaloonType _type)
        {
            Baloon _bal = createBaloon();
            _bal.SetData(clicklistener, _type, 3, bIsSlowTime, Baloon.State.Move);
            _bal.StartBaloon();
        }

        private Baloon.BaloonType GetItem()
        {
            if (lstBaloonType.Count == 0)
                return Baloon.BaloonType.None;
            int _maxIndex = 0;
            _maxIndex = lstBaloonType.Count;
            int _rand = Random.Range(0, _maxIndex);
            Baloon.BaloonType _type = lstBaloonType[_rand];
            lstItem[(int)_type]--;
            if (lstItem[(int)_type] == 0)
            {

                lstBaloonType.RemoveAt(_rand);
            }
            return _type;
        }
        /// <summary>
        /// 대기중인 풍선 찾기 (없으면 널 리턴)
        /// </summary>
        /// <returns></returns>
        private Baloon findWaitBaloon()
        {
            for (int i = 0; i < lstBaloon.Count; i++)
            {
                if (lstBaloon[i].curState == Baloon.State.Wait && lstBaloon[i].gameObject.activeSelf == false)
                {
                    return lstBaloon[i];
                }
            }
            Debug.Log("can not found baloon");
            return null;
        }
        public void SetIgnoreRespone(bool _isignore)
        {
            bIsPauseRespone = _isignore;
        }
        #endregion
    }
}