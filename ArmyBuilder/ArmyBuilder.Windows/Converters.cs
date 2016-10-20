﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ArmyBuilder.Core;
using ArmyBuilder.Core.Models;

namespace ArmyBuilder.Windows
{
    public class DetachmentDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate
            SelectTemplate(object item, DependencyObject container)
        {
            FrameworkElement element = container as FrameworkElement;

            if (element != null && item != null && item is DetachmentData)
            {
                DetachmentData detachment = item as DetachmentData;

                switch (detachment.Detachment.Type)
                {
                    case DetachmentType.UnitDetachment:
                        return element.FindResource("UnitDetachmentTemplate") as DataTemplate;
                    case DetachmentType.BattleForged:
                        return element.FindResource("BattleForgedDetachmentTemplate") as DataTemplate;
                    case DetachmentType.ForceOrgDetachment:
                        return element.FindResource("ForceOrgDetachmentTemplate") as DataTemplate;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return null;
        }
    }
}
