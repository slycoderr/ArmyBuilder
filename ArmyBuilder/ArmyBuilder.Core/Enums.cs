using System.Xml.Serialization;

namespace ArmyBuilder.Core
{
    public enum ForceOrgSlot
    {
        [XmlEnum("0")]
        HQ = 0,
        [XmlEnum("1")]
        Troop = 1,
        [XmlEnum("2")]
        Elite = 2,
        [XmlEnum("3")]
        FastAttack = 3,
        [XmlEnum("4")]
        HeavySupport = 4,
        [XmlEnum("5")]
        LordOfWar = 5,
        [XmlEnum("6")]
        Fortification = 6
    }

    public enum EquipmentType
    {
        [XmlEnum("0")]
        Normal,
        [XmlEnum("1")]
        Upgrade = 1
    }

    public enum DetachmentType
    {
        [XmlEnum("0")]
        UnitDetachment,
        [XmlEnum("1")]
        BattleForged,
        [XmlEnum("2")]
        ForceOrgDetachment
    }

    public enum DetachmentRequirementType
    {
        [XmlEnum("0")]
        ForceOrg,
        [XmlEnum("1")]
        Unit,
        [XmlEnum("2")]
        Detachment
    }

    public enum GameSystem
    {
        [XmlEnum("0")]
        W40K,
        [XmlEnum("1")]
        AoS
    }

    public enum Allegiance
    {
        [XmlEnum("0")]
        Order,
        [XmlEnum("1")]
        Death,
        Chaos,
        Destruction,
        Sylvaneth
    }
}