using System;
using System.Linq;

namespace Model.EnemySpecialAttacks{
    public abstract class SpecialAttackBase{

        public virtual string Desc => "This is a desc!";
        
        protected readonly string[] args;

        public SpecialAttackBase(string[] args){
            this.args = args;
        }

        public abstract void Execute(StageActionBase info);

        public static SpecialAttackBase MakeSpecialAttack(string line){
            if (string.IsNullOrEmpty(line)) return null;
            var spStr = line.Split(";");
            var className = spStr.First();
            if(className != "")
                return Activator.CreateInstance(Type.GetType($"Model.EnemySpecialAttacks.{className}", true),
                        new object[]{ spStr[1..] }) as
                    SpecialAttackBase;
            return null;
        }
    }
}