using System;
using Core.PlayArea.Balls;
using Core.PlayArea.Block;
using Core.PlayArea.Mine;
using Core.PlayArea.TouchTracking;
using Model;
using Model.Mechanics.PlayableObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea{
    public class PlayAreaManager: MonoBehaviour{
        public Model.Mechanics.PlayArea model;
        public BallManager ballManager;
        public BlockManager blockManager;
        public MineManager mineManager;
        public TouchTracker touchTracker;

        private void Start(){
            model = GameManager.shared.game.playArea;
            model.gameObject = gameObject;
        }
        
        public Rect GridRectToRect(RectInt gridRect){
            var gridCellSize = ((RectTransform)transform).rect.size / Model.Mechanics.PlayArea.GridSize;
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

        private BlockView _test;
        private void Update(){
            // var values = Enum.GetValues(typeof(BlockLevel));
            // if (Input.GetKeyUp(KeyCode.A)){
            //     var level = (BlockLevel)values.GetValue(Random.Range(0, values.Length));
            //     var gridSize = Model.Obstacles.Block.GenerateGridSize(level);
            //     var rect = model.RandomRectInt(gridSize);
            //     if (rect != null){
            //         var block = new Model.Obstacles.Block(null, rect.Value, level);
            //         _test = blockManager.PlaceBlock(block);
            //     }
            // }
            //
            // if (Input.GetKeyUp(KeyCode.S)){
            //     blockManager.RemoveBlock(_test);
            // }
        }
    }
}