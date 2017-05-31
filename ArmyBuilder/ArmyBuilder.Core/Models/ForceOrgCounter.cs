using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    public class ForceOrgCounter : BindableBase
    {
        public int HQ { get => hq; set => SetValue(ref hq, value); }

        public int Troop { get => troop; set => SetValue(ref troop, value); }

        public int Elite { get => elite; set => SetValue(ref elite, value); }

        public int FastAttack { get => fastAttack; set => SetValue(ref fastAttack, value); }

        public int HeavySupport { get => heavySupport; set => SetValue(ref heavySupport, value); }

        public int LordOfWar { get => lordOfWar; set => SetValue(ref lordOfWar, value); }

        public int Fortification { get => fortification; set => SetValue(ref fortification, value); }
        private int elite;
        private int fastAttack;
        private int fortification;
        private int heavySupport;
        private int hq;
        private int lordOfWar;
        private int troop;
    }
}