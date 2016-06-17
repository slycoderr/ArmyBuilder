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
        Normal,
        [XmlEnum("1")]
        BattleForged
    }

    public enum DetachmentRequirementType
    {
        [XmlEnum("0")]
        ForceOrg,
        [XmlEnum("1")]
        Unit
    }

}