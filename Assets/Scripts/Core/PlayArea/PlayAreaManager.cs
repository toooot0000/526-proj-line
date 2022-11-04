using System;
using Core.PlayArea.Balls;
using Core.PlayArea.Block;
using Core.PlayArea.Mine;
using Model;
using Model.Obstacles;
using Model.Obstacles.MineEffects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea{
    public class PlayAreaManager: MonoBehaviour{
        public Model.PlayArea model;
        public BallManager ballManager;
        public BlockManager blockManager;
        public MineManager mineManager;

        private void Start(){
            model = GameManager.shared.game.playArea;
            model.gameObject = gameObject;
        }
        
        public Rect GridRectToRect(RectInt gridRect){
            var gridCellSize = ((RectTransform)transform).rect.size / Model.PlayArea.GridSize;
            var ret = new Rect(){
                position = gridRect.position * gridCellSize,
                size = gridRect.size * gridCellSize
            };
            ret.position += ret.size / 2;
            return ret;
        }
        
        
        public BlockView AddBlock(BlockLevel level){
            var block = model.MakeAndPlaceBlock(level);
            return blockManager.PlaceBlock(block);
        }

        public MineView AddMine(MineEffect effect) {
            var mine = model.MakeAndPlaceMine(new MineEffectLoseLife());
            return mineManager.PlaceMine(mine);
        }

        private BlockView _testBlock;
        private MineView _mineView;
        private void Update(){
            if (Input.GetKeyUp(KeyCode.A)) {
                _mineView = AddMine(new MineEffectLoseLife(1));
            }

            if (Input.GetKeyUp(KeyCode.S)) {
                mineManager.RemoveMine(_mineView);
            }
        }
    }
}