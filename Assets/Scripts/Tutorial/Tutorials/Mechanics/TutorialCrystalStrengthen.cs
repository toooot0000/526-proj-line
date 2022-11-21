using System.Linq;
using Core.PlayArea.BlackHoles;
using Core.PlayArea.Blocks;
using Core.PlayArea.Crystals;
using Core.PlayArea.Mines;
using Tutorial.Common;
using Tutorial.Utility;
using UnityEngine;

namespace Tutorial.Tutorials.Mechanics{
    public class TutorialCrystalStrengthen : TutorialBase{
        public new const string PrefabName = "TutorialCrystalStrengthen";
        public TutorialText[] texts;
        public TutorialTapCatcher tapCatcher;
        private StepBase[] _steps;
        protected override StepBase[] Steps => _steps;
        private CrystalView _target;

        public override void OnLoaded(TutorialManager mng){
            _target = GameManager.shared.playAreaManager.GetAllViews().OfType<CrystalView>().First();
            Debug.Assert(_target != null);
            _target.HandOverControlTo(this);
            _target.tutorIsCircleable = false;
            _target.tutorIsSliceable = false;
            var o = _target.gameObject;
            _steps = new StepBase[]{
                // This is a magic crystal! When you slice it, you will have extra inks to draw lines!
                new StepTapToContinue<TutorialCrystalStrengthen>(texts[0], tapCatcher, o)
            };
            base.OnLoaded(mng);
        }

        protected override void Complete() {
            _target.GainBackControlFrom(this);
            base.Complete();
        }
    }
}