using System;
using System.Collections.Generic;
using Core.DisplayArea;
using Core.DisplayArea.Stage;
using Core.PlayArea.Balls;
using Core.PlayArea.TouchTracking;
using Tutorials.Display;
using Tutorials.SliceBall;
using UI;
using UI.Common;
using UnityEngine;
using Utility;

namespace Tutorials{
    public class TutorialManager : MonoBehaviour{
        private const string PrefabPathPrefix = "Prefabs/Tutorials/";

        public BallManager ballManager;
        public Shade shade;
        public TouchTracker tracker;
        public StageManager stageManager;
        public ActionDetailDisplayer actionDetailDisplayer;

        private readonly HashSet<string> _completeTutorials = new();
        private float _nextDelay;
        private string _nextName;
        private string _curName;

        private void Update(){
            if (Input.GetKeyUp(KeyCode.A)){
                LoadTutorial("TutorialCharge");
            }
        }

        public TutorialBase LoadTutorial(string tutorialName){
            if (_completeTutorials.Contains(tutorialName)) return null;
            _curName = tutorialName;
            var prefab = Resources.Load<GameObject>($"{PrefabPathPrefix}{tutorialName}");
            var inst = Instantiate(prefab, transform);
            var tutorial = inst.GetComponent<TutorialBase>();
            tutorial.Load(this);
            tutorial.OnComplete += CompleteTutorial;
            shade.SetActive(true);
            return tutorial;
        }

        public void CompleteTutorial(TutorialBase tutorial){
            shade.SetActive(false);
            _completeTutorials.Add(_curName);
            if (!string.IsNullOrEmpty(tutorial.nextTutorialName)){
                _nextName = tutorial.nextTutorialName;
                _nextDelay = Math.Max(0.1f, tutorial.nextDelay);
                StartCoroutine(CoroutineUtility.Delayed(_nextDelay, () => { LoadTutorial(_nextName); }));
            }
        }
    }
}