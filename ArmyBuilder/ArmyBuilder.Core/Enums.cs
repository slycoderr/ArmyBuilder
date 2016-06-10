namespace ArmyBuilder.Core
{
    public enum ForceOrgSlot
    {
        HQ = 0,
        Troop = 1,
        Elite = 2,
        FastAttack = 3,
        HeavySupport = 4,
        LordOfWar = 5
    }

    public enum AffectRule
    {
        None,
        /// <summary>
        /// When this equipment is taken, a slot change occurs
        /// </summary>
        SlotChange
    }

    public enum EquipmentType
    {
        /// <summary>
        /// Any number of models in the unit can take this. Example <PMR=A>
        /// </summary>
        AnyCanTake,
        /// <summary>
        /// All models in the unit must take this if chosen. Example <PMR=A>
        /// </summary>
        AllMustTake,

        /// <summary>
        /// For Each X number of models in the unit, you can take Y amount. Example <PMR=P10>
        /// </summary>
        PerNumberCanTake,

        /// <summary>
        /// A unit must have X minimum size to take this upgrade
        /// </summary>
        UnitMustHaveMinSize,

        /// <summary>
        /// A unit must have no more than X size to take this piece of wargear
        /// </summary>
        UnitMustNotExceedMaxSize,

        /// <summary>
        /// This piece of equipment is just an upgrade 
        /// </summary>
        Upgrade,

        /// <summary>
        /// This upgrade depends on another one to be taken
        /// </summary>
        DependantOn,

        /// <summary>
        /// This followed by a list of comma seperated id numbers states these set of upgrades are Mutually Exclusive
        /// </summary>
        MutuallyExclusive
    }

    public enum UnitRuleType
    {
        None,
        DependsOn
    }

    public enum ReplacesRule
    {
        None = 0,
        /// <summary>
        /// Simply, this gear replaces another piece
        /// </summary>
        Default,

        /// <summary>
        /// Taking this replaces all the pieces of gear in the id list
        /// </summary>
        ReplacesAll,

        /// <summary>
        /// This gear can replace any of the gear in the id list. add up the counts on those ids and us that as a current max minus the num taken
        /// </summary>
        ReplacesAny
    }

    public enum AppType
    {
        Builder,
        Designer
    }
}