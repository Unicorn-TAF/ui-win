using System;
using System.Collections.Generic;
using System.Reflection;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject
{
    public static class ContainerFactory
    {
        public static void InitContainer<T>(T container)
        {
            InitContainerProperties(container);
            InitContainerFields(container);
        }

        private static void InitContainerProperties<T>(T container)
        {
            var properties = container.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var property in properties)
            {
                var findAttribute = property.GetCustomAttribute(typeof(FindAttribute), true) as FindAttribute;

                if (findAttribute != null)
                {
                    Type controlType = property.PropertyType;
                    var control = Activator.CreateInstance(controlType);
                    ((IControl)control).Locator = findAttribute.Locator;
                    ((IControl)control).Cached = false;

                    var contextField = control.GetType().GetProperty("ParentSearchContext", BindingFlags.Public | BindingFlags.Instance);
                    contextField.SetValue(control, container);

                    var nameAttribute = property.GetCustomAttribute(typeof(NameAttribute), true) as NameAttribute;

                    if (nameAttribute != null)
                    {
                        ((IControl)control).Name = nameAttribute.Name;
                    }

                    if (control is IDynamicControl)
                    {
                        DefineDynamicControl(ref control, property);
                    }

                    if (control is IContainer)
                    {
                        InitContainer(control);
                    }

                    property.SetValue(container, control);
                }
            }
        }

        private static void InitContainerFields<T>(T container)
        {
            var fields = container.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var field in fields)
            {
                var findAttribute = field.GetCustomAttribute(typeof(FindAttribute), true) as FindAttribute;

                if (findAttribute != null)
                {
                    Type controlType = field.FieldType;
                    var control = Activator.CreateInstance(controlType);
                    ((IControl)control).Locator = findAttribute.Locator;
                    ((IControl)control).Cached = false;

                    var contextField = control.GetType().GetProperty("ParentSearchContext", BindingFlags.Public | BindingFlags.Instance);
                    contextField.SetValue(control, container);

                    var nameAttribute = field.GetCustomAttribute(typeof(NameAttribute), true) as NameAttribute;

                    if (nameAttribute != null)
                    {
                        ((IControl)control).Name = nameAttribute.Name;
                    }

                    if (control is IDynamicControl)
                    {
                        DefineDynamicControl(ref control, field);
                    }

                    if (control is IContainer)
                    {
                        InitContainer(control);
                    }

                    field.SetValue(container, control);
                }
            }
        }

        private static void DefineDynamicControl(ref object control, MemberInfo classMember)
        {
            var definitions = classMember.GetCustomAttributes(typeof(DefineAttribute), true) as DefineAttribute[];
            var dictionary = new Dictionary<int, ByLocator>();

            foreach (var definition in definitions)
            {
                dictionary.Add(definition.ElementDef, definition.Locator);
            }

            (control as IDynamicControl).Populate(dictionary);
        }
    }
}
