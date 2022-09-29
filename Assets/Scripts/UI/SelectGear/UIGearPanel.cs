using System.Collections;
using System.Collections.Generic;
using Model;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI{
    public delegate void ClickEvent(Game game,int id);

    public class UIGearPanel : MonoBehaviour
    {
        public ClickEvent OnClick;
        // Start is called before the first frame update
        public TextMeshProUGUI text;
        public Image image;
        public int id;
        public GameObject highLight;
        void Start()
        {
                            
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void Click()
        {
            this.OnClick?.Invoke(GameManager.shared.game,this.id);
        }
    }

}
