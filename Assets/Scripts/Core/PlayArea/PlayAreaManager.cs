using System;
using System.Collections.Generic;
using Core.PlayArea.Balls;
using Core.PlayArea.BlackHoles;
using Core.PlayArea.Blocks;
using Core.PlayArea.Mines;
using Model.Mechanics;
using Model.Mechanics.PlayableObjects;
using Model.Mechanics.PlayableObjects.MineEffects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.PlayArea{
    public class PlayAreaManager: MonoBehaviour{
        public Model.Mechanics.PlayArea model;
        public BallManager ballManager;
        public BlockManager blockManager;
        public MineManager mineManager;
        public BlackHoleManager blackHoleManager;

        private readonly List<PlayableObjectViewBase> _views = new();

        public IEnumerable<IPlayableViewManager> AllManagers => new IPlayableViewManager[] {
            ballManager,
            blockManager,
            mineManager,
            blackHoleManager
        };

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

        public void SetPlayableViewPosition<T>(T view, IPlayableObject objectModel) 
        where T: PlayableObjectViewBase{
            var rect = GridRectToRect(objectModel.InitGridPosition);
            var rectTrans = view.transform;
            ((RectTransform)rectTrans).anchoredPosition = rect.position;
            ((RectTransform)rectTrans).sizeDelta = rect.size;
        }


        public BlockView AddBlock(BlockLevel level){
            var block = model.MakeAndPlaceBlock(level);
            return blockManager.PlaceBlock(block);
        }

        private BlockView _test;
        private void Update(){
            if (Input.GetKeyUp(KeyCode.A)){
                mineManager.PlaceMine(model.MakeAndPlaceMine(2, 1, new MineEffectLoseLife(1)));
            }

            if (Input.GetKeyUp(KeyCode.S)){
                blackHoleManager.PlaceBlackHole(model.MakeAndPlaceBlackHole(0.2f, 3f));
            }
        }

        public IEnumerable<T> GetAllViewsOfProperty<T>() where T: IPlayableObjectViewProperty {
            foreach (var playableViewManager in AllManagers) {
                foreach (var playableObjectViewBase in playableViewManager.GetAllViews()) {
                    if (playableObjectViewBase is T typed) yield return typed;
                }
            }
        }
    }
}