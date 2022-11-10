using System;
using System.Linq;
using Model;
using Model.Mechanics.PlayableObjects;
using UnityEngine;

namespace Core.PlayArea.Crystals{
    public class CrystalView: PlayableObjectViewWithModel<Crystal>, ISliceableView, IOnPlayerFinishDrawing{
        [Serializable]
        public struct Pair{
            public CrystalType type;
            public Sprite spr;
        }
        
        private Crystal _model;
        public SpriteRenderer spriteRenderer;
        public Pair[] typeToSprite;
        public bool isTriggered = false;

        public override Crystal Model{
            set{
                _model = value;
                spriteRenderer.sprite = typeToSprite.First(p => p.type == value.type).spr;
            }
            get => _model;
        }

        public void OnSliced(){
            if (isTriggered) return;
            Model.OnSliced().Execute();
            isTriggered = true;
        }

        public void OnPlayerFinishDrawing(){
            isTriggered = false;
        }
    }
}