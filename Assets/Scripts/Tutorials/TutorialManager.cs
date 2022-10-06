using System;
using System.Collections.Generic;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using UI;
using UnityEngine;
using Utility;

namespace Tutorials{
    public class TutorialManager : MonoBehaviour{
        private const string PrefabPathPrefix = "Prefabs/Tutorials/";

        public BallManager ballManager;
        public Shade shade;
        public TouchTracker tracker;
        public StageManager stageManager;

        private readonly HashSet<string> _completeTutorials = new();
        private float _nextDelay;
        private string _nextName;

        private void Update(){
            if (Input.GetKeyUp(KeyCode.A)) LoadTutorial("TutorialSliceBall");
        }

        public TutorialBase LoadTutorial(string tutorialName){
            if (_completeTutorials.Contains(tutorialName)) return null;
            var prefab = Resources.Load<GameObject>($"{PrefabPathPrefix}{tutorialName}");
            var inst = Instantiate(prefab, transform);
            var tutorial = inst.GetComponent<TutorialBase>();
            tutorial.Load(BuildTutorialContext(tutorial));
            tutorial.OnComplete += CompleteTutorial;
            shade.SetActive(true);
            return tutorial;
        }

        public void CompleteTutorial(TutorialBase tutorial){
            shade.SetActive(false);
            _completeTutorials.Add(tutorial.GetType().ToString());
            if (!string.IsNullOrEmpty(tutorial.nextTutorialName)){
                _nextName = tutorial.nextTutorialName;
                _nextDelay = Math.Max(0.1f, tutorial.nextDelay);
                StartCoroutine(CoroutineUtility.Delayed(_nextDelay, () => { LoadTutorial(_nextName); }));
            }
        }

        public TutorialContextBase BuildTutorialContext(TutorialBase tutorial){
            switch (tutorial){
                case TutorialSliceBall _:
                    return new TutorialContextSliceBall{
                        ballManager = ballManager,
                        tracker = tracker,
                        stageManager = stageManager
                    };
                default:
                    return null;
            }
        }
    }
}