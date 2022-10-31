namespace Model.Buff{
    public interface IBuffTrigger{ }
    
    public interface IBuffEffect<in T> where T: IBuffModifiable{
        void Execute(T buffable);
    }

    public interface IBuffTriggerOnTurnBegin : IBuffTrigger {
        IBuffEffect<Damageable> OnTurnBegin();
    }

    public interface IBuffTriggerOnTurnEnd : IBuffTrigger {
        IBuffEffect<Damageable> OnTurnEnd();
    }

    public interface IBuffTriggerOnBallSliced<in T, in TBall>: IBuffTrigger 
        where TBall: Ball
        where T: IBuffModifiable{
        IBuffEffect<T> OnBallSliced(TBall ball);
    }

    public interface IBuffTriggerOnBallCircled<in T, in TBall>: IBuffTrigger 
        where TBall: Ball
        where T: IBuffModifiable{
        IBuffEffect<T> OnBallCircled(TBall ball);
    }

    public interface IBuffTriggerOnInputStart<in T> : IBuffTrigger
    where T: IBuffModifiable{
        IBuffEffect<T> OnInputStart();
    }

    public interface IBuffTriggerOnGetPlayerActionInfo : IBuffTrigger{
        IBuffEffect<StageActionPlayerAction> OnGetPlayerActionInfo();
    }

    public interface IBuffTriggerOnGetEnemyActionInfo : IBuffTrigger{
        IBuffEffect<StageActionEnemyAction> OnGetEnemyActionInfo();
    }

    public interface IBuffTriggerOnTakeDamage : IBuffTrigger{
        IBuffEffect<IBuffModifiable> OnTakeDamage();
    }

    public interface IBuffTriggerOnApplyToDamageable : IBuffTrigger{
        IBuffEffect<Damageable> OnApply();
    }
}