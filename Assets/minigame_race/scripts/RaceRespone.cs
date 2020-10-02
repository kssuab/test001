using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Race
{
    public class RaceRespone : MonoBehaviour
    {
        public static RaceRespone instance;

        //9개의 격자 위치
        [SerializeField]
        private Vector2[] arrCreatePosition;
        //아이템 프리팹
        [SerializeField]
        private RaceItem itemori;
        [SerializeField]
        private RectTransform rtParent;
        private void Awake()
        {
            instance = this;
        }

        
        public void StartMoveItem(RaceItem item)
        {
            RectTransform rt = item.transform as RectTransform;
            rt.DOAnchorPosY(RaceManager.baespeed, -960).SetEase(Ease.Linear).OnComplete(()=> {
                Destroy(this);
            });

        }


        //아이템 생성
        public RaceItem CreateTextItem(int index,int sfcno ,  string  desc , bool iscorrect  )
        {
            GameObject obj = Instantiate(itemori.gameObject, rtParent);
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).anchoredPosition = arrCreatePosition[index];
            RaceItem item = obj.GetComponent<RaceItem>();

            item.SetTextTypeData(sfcno, desc, iscorrect);


            return item;

        }
        public RaceItem CreateImageItem(int index, int sfcno,Sprite sprite, bool iscorrect)
        {
            GameObject obj = Instantiate(itemori.gameObject, rtParent);
            obj.transform.localScale = Vector3.one;
            (obj.transform as RectTransform).anchoredPosition = arrCreatePosition[index];
            RaceItem item = obj.GetComponent<RaceItem>();
            item.SetImageTypeData(sfcno, sprite, iscorrect);
            return item;
        }

        public void ShakePosition()
        {
            Vector2 _pos = Vector2.zero;
            for (int i = 1; i < arrCreatePosition.Length; i++)
            {
                int rand = Random.Range(0, arrCreatePosition.Length);
                _pos = arrCreatePosition[i];
                arrCreatePosition[i] = arrCreatePosition[rand];
                arrCreatePosition[rand] = _pos;
            }

        }








    }
}
