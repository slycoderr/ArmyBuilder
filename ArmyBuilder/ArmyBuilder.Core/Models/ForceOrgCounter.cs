using Slycoder.MVVM;

namespace ArmyBuilder.Core.Models
{
    public class ForceOrgCounter : BindableBase
    {
        public int HQ { get { return hq; } set { SetValue(ref hq, value); } }

        public int Troop { get { return troop; } set { SetValue(ref troop, value); } }

        public int Elite { get { return elite; } set { SetValue(ref elite, value); } }

        public int FastAttack { get { return fastAttack; } set { SetValue(ref fastAttack, value); } }

        public int HeavySupport { get { return heavySupport; } set { SetValue(ref heavySupport, value); } }

        public int LordOfWar { get { return lordOfWar; } set { SetValue(ref lordOfWar, value); } }

        public int Fortification { get { return fortification; } set { SetValue(ref fortification, value); } }
        private int elite;
        private int fastAttack;
        private int fortification;
        private int heavySupport;
        private int hq;
        private int lordOfWar;
        private int troop;
    }
}