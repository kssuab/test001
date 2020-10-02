using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace Race
{
    public class RaceItem : MonoBehaviour
    {
        public enum ItemType
        {
            None,
            Text,
            Image,
            Trap


        }

        ItemType _itemType = ItemType.None;
        ItemType itemType {
            get { return _itemType; }
        }

        [SerializeField]
        private int sfcNo;
        [SerializeField]
        private int groupno;
        [SerializeField]
        private GameObject gbjTextBox;
        [SerializeField]
        private Text txtSfc;
        [SerializeField]
        private GameObject gbjImageBox;
        [SerializeField]
        private Image imgSfc;
        [SerializeField]
        private BoxCollider2D collider;

        private string txt;
        private Sprite sprite;
      

        /// <summary>
        /// 텍스트 타입의 아이템
        /// </summary>
        /// <param name="sfcno"></param>
        /// <param name="desc"></param>
        /// <param name="iscorrect"></param>
        public void SetTextTypeData(int sfcno, string desc, bool iscorrect)
        {



            _itemType = ItemType.Text;
            sfcNo = sfcno;
            txt = desc;
            txtSfc.text = txt;
            gbjTextBox.SetActive(true);
        }
        /// <summary>
        /// 이미지 타입의 아이템
        /// </summary>
        /// <param name="sfcno"></param>
        /// <param name="sprite"></param>
        /// <param name="iscorrect"></param>
        public void SetImageTypeData(int sfcno, Sprite sprite, bool iscorrect)
        {

            _itemType = ItemType.Image;
            sfcNo = sfcno;
            imgSfc.sprite = sprite;
            gbjImageBox.SetActive(true);


        }

        public void SetTrapData(ItemType type)
        {
            _itemType = type;
        }

        public void SetColliderSize(float x  , float y)
        {
            collider.size = new Vector2(x, y); 
        }





    }


}
