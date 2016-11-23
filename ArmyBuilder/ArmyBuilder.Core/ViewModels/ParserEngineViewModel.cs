using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArmyBuilder.Core.Models;

namespace ArmyBuilder.Core.ViewModels
{
    public class ParserEngineViewModel
    {
        private readonly MainViewModel mainViewModel;

        public ParserEngineViewModel(MainViewModel mv)
        {
            mainViewModel = mv;
        }

        public IArmyList GenerateListFromText(List<string> input)
        {
            IArmyList list = null;
            DetachmentData currentDetachment = null;

            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                }

                else if (line.Contains("="))
                {
                    ParseListMetadata(ref list, line);
                }

                else if (line.Contains("*"))
                {
                    currentDetachment = ParseListFormation(list, line);
                }

                else if (line.Contains("^"))
                {
                    if (list != null)
                    {
                        if (currentDetachment == null)
                        {
                            currentDetachment = list.Detachments.FirstOrDefault(d => d.Detachment.Name == "Uncategorized");

                            if (currentDetachment == null)
                            {
                                currentDetachment =new DetachmentData(new Detachment {Name = "Uncategorized", Army = list.Army});
                                list.Detachments.Add(currentDetachment);
                            }
                    }

                        ParseListUnit(list, currentDetachment, line);
                    }
                    else
                    {
                        throw new ArgumentException($"The army list parameter {line} is invalid. The game system must be declared first.");
                    }

                }

                else
                {
                    throw new ArgumentException($"The army list parameter {line} is invalid. This command is unknown.");
                }
            }

            return list;
        }

        public void ParseListMetadata(ref IArmyList list, string line)
        {
            var split = line.Split('=');
            var field = split.ElementAtOrDefault(0);
            var value = split.ElementAtOrDefault(1);

            if (string.IsNullOrEmpty(field))
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The field is empty.");
            }

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The value is empty.");
            }

            switch (field.ToLower())
            {
                case "system":
                {
                    switch (value.ToLower())
                    {
                        case "40k":
                        {
                            list = new ArmyList {System = GameSystem.W40K};
                            break;
                        }
                        case "aos":
                        {
                            list = new AgeOfSigmarArmyList {System = GameSystem.AoS};
                            break;
                        }
                        default:
                        {
                            throw new ArgumentException($"The army list parameter {line} is invalid. The value of {value} is not a known system.");
                        }
                    }

                    mainViewModel.SelectedArmyList = list;

                    break;
                }

                case "points":
                {
                    if (list == null)
                    {
                        throw new ArgumentException($"The army list parameter {line} is invalid. The game system must be declared first");
                    }

                    if (!uint.TryParse(value, out uint points))
                    {
                        throw new ArgumentException($"The army list parameter {line} is invalid. The value of {value} must be a non negative whole number.");
                    }

                    list.PointsLimit = points;

                    break;
                }

                case "name":
                {
                    if (list == null)
                    {
                        throw new ArgumentException($"The army list parameter {line} is invalid. The game system must be declared first");
                    }

                    list.Name = value;
                    break;
                }

                case "allegiance":
                {
                    if (list == null)
                    {
                        throw new ArgumentException($"The army list parameter {line} is invalid. The game system must be declared first");
                    }

                    if (list.System != GameSystem.AoS)
                    {
                        throw new ArgumentException($"The army list parameter {line} is invalid. The allegiance field only exists in Age of Sigmar.");
                    }

                    if(!Enum.TryParse(value, out Allegiance a ))
                    {
                        throw new ArgumentException($"The army list parameter {line} is invalid. The allegiance {value} is not known.");
                    }

                    ((AgeOfSigmarArmyList) list).Allegiance = a;

                    switch (a)
                    {
                        case Allegiance.Order:
                            break;
                        case Allegiance.Death:
                            break;
                        case Allegiance.Chaos:
                            break;
                        case Allegiance.Destruction:
                            break;
                        case Allegiance.Sylvaneth:
                            list.Army = mainViewModel.Armies.First(ar => ar.Id == 10);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    break;
                }

                default:
                {
                    throw new ArgumentException($"The army list parameter {line} is invalid. The value of {field} is not a known metadata field.");
                }
            }
        }

        public DetachmentData ParseListFormation(IArmyList list, string line)
        {
            var split = line.Split('*');
            var value = split.ElementAtOrDefault(0);

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The formation is empty.");
            }

            var formation = mainViewModel.Armies.FirstOrDefault(a => a.Id == list.Army.Id).Detachments.FirstOrDefault(d => d.Name == value);

            if (formation == null)
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The formation {value} is not known.");
            }

            return new DetachmentData(formation);
        }

        public void ParseListUnit(IArmyList list, DetachmentData detachment, string line)
        {
            var split = line.Split('^');
            var inputUnitSize = split.ElementAtOrDefault(0);
            var inputUnitName = split.ElementAtOrDefault(1);
            Army army = detachment.Detachment.Army;
            uint unitSize;

            if (string.IsNullOrEmpty(inputUnitSize))
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The field is empty.");
            }

            if (string.IsNullOrEmpty(inputUnitName))
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The value is empty.");
            }

            var unit = army.Units.FirstOrDefault(d => d.Name.ToLower() == inputUnitName.ToLower());

            if (unit == null)
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The unit {inputUnitName} is not known.");
            }


            if (!uint.TryParse(inputUnitSize, out unitSize))
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The unit size must be a whole number greater than 1.");
            }

            if (unitSize < unit.Minimum)
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The number {inputUnitSize} is lower than the minimum unit size of {unit.Minimum}.");
            }

            if (unitSize > unit.Maximum)
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The number {inputUnitSize} is greater than the maximum unit size of {unit.Maximum}.");
            }

            if (unitSize % unit.IncrementSize != 0)
            {
                throw new ArgumentException($"The army list parameter {line} is invalid. The number {inputUnitSize} is not in increments of {unit.IncrementSize}.");
            }

            detachment.Units.Add(new ArmyListData(unit, list.Army){Count = unitSize});

            mainViewModel.UpdateListCost();
        }
    }
}
