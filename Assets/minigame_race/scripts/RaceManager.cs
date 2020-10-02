using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Race
{
    public class RaceManager : MonoBehaviour
    {
        public static RaceManager instance = null;
        public enum GameType
        {
            Normal,
            Sub
        }
        public struct itembasedata
        {
            public int sfcno; //  고유 넘버여야함 (텍스트와 이미지 한쌍일 경우에도)
            public string desc;
            public Sprite sprite;
            public string url; 
            public int groupno;
            public RaceItem.ItemType type;
        }
        //정답 정보
        private int nAnswer = 0 ;

        public static float baespeed = 3.0f;

        [SerializeField]
        private GameType gametype = GameType.Normal;
        [SerializeField]
        private CanvasGroup group;
        [SerializeField]
        private RaceCar car;
        [SerializeField]
        private RaceQst qst;
        [SerializeField]
        private Mover mover;

        private int[] arrIndex = new int[9];

        itembasedata[] arrtemp = new itembasedata[10];

        private void Awake()
        {
            SetBlock(true);
            instance = this;
            qst = GetComponent<RaceQst>();
            InitData();
        }
        private void TestCreateItemSet()
        {

            gametype = GameType.Normal;
            {
                arrtemp[0].desc = "책상";
                arrtemp[0].groupno = 1;
                arrtemp[0].sfcno = 1;
                arrtemp[0].sprite = null;
                arrtemp[0].url = "PP_1_03_OutroChair.png";
                arrtemp[0].type = RaceItem.ItemType.Text;
                //////////////
                arrtemp[1].desc = "책상";
                arrtemp[1].groupno = 1;
                arrtemp[1].sfcno = 1;
                arrtemp[1].sprite = null;
                arrtemp[1].url = "PP_1_03_OutroChair.png";
                arrtemp[1].type = RaceItem.ItemType.Image;
                //////////////
                arrtemp[2].desc = "";
                arrtemp[2].groupno = 1;
                arrtemp[2].sfcno = 1;
                arrtemp[2].sprite = null;
                arrtemp[2].url = "PP_1_03_OutroChair.png";
                arrtemp[2].type = RaceItem.ItemType.Text;
                //////////////
                arrtemp[3].desc = "책상";
                arrtemp[3].groupno = 1;
                arrtemp[3].sfcno = 1;
                arrtemp[3].sprite = null;
                arrtemp[3].url = "PP_1_03_OutroChair.png";
                arrtemp[3].type = RaceItem.ItemType.Image;


            }

            





        }
        private void InitData()
        {
            //게임타입(main 랜덤 정오답, sub 그룹으로(최대 3개) 묶인 아이템)

            for (int i = 0; i < arrIndex.Length; i++)
            {
                arrIndex[i] = i;
            }
            //정답 정보 설정
            //nAnswer = 1;

            //리소스 담아놓기
           




        }

        


        private void ShadkingIndex()
        {
            for (int i = 0; i < arrIndex.Length; i++)
            {
                int rand = Random.Range(0, arrIndex.Length);
                int val = arrIndex[i];
                arrIndex[i] = arrIndex[rand];
                arrIndex[rand] = val;
            }


        }


        private void Start()
        {
            // 배경 스크롤 시작
            mover.StartMove();
            car.ShowCar();
            

        }
        public void EndShowCar()
        {
            qst.ShowQstLarge(); 
        }

        public void GameStart()
        {
            SetBlock(false);
           
            // 아이템 리스폰 시작

        }
        public void SetBlock(bool isblock)
        {
            group.blocksRaycasts = !isblock;
        }
        

    }
}
