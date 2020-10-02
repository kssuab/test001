using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
namespace Race
{
    public class RaceQst : MonoBehaviour
    {
        [SerializeField]
        private Image imgQstLarge;
        [SerializeField]
        private Image imgQstSmall;


        [SerializeField]
        private float waitTime = 3.0f;

        private RaceManager manager {
            get
            {
                return RaceManager.instance;
            }
        }
        private void Awake()
        {
            
        }
        public void SetImageLarge(Sprite sprite)
        {
            imgQstLarge.sprite = sprite;
        }
        public void SetImageSmall(Sprite sprite)
        {
            imgQstSmall.sprite = sprite;
        }
        public     void ShowQstImage(Sprite _sprite)
        {
            imgQstLarge.sprite = _sprite;
            imgQstSmall.sprite = _sprite;

        }

        public void ShowQstLarge()
        {
            imgQstLarge.rectTransform.anchoredPosition = new Vector2(0, 800);
            imgQstLarge.gameObject.SetActive(true);
            imgQstLarge.rectTransform.DOAnchorPosY(0, 1.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                HideQstLarge();
            }); 
        }
        public void HideQstLarge()
        {
            imgQstLarge.rectTransform.DOAnchorPosY(800, 1.5f).SetEase(Ease.InOutBack).SetDelay(waitTime)
                .OnComplete(() =>
                {
                    ShowQstSmall();
                }); ;

        }
        public void ShowQstSmall()
        {
            imgQstSmall.rectTransform.anchoredPosition = new Vector2(50, 800);
            imgQstSmall.gameObject.SetActive(true);
            imgQstSmall.rectTransform.DOAnchorPosY( -50, 1.5f).SetEase(Ease.InOutBack).OnComplete(() =>
            {
                manager.GameStart();
            });
        }
        public void HideQstSmall(System.Action listener)
        {
            imgQstSmall.rectTransform.DOAnchorPosY(800, 1.5f).SetEase(Ease.InOutBack).SetDelay(waitTime)
               .OnComplete(() =>
               {
                   if (listener != null)
                       listener();
               }); ;
        }


        



    }
}