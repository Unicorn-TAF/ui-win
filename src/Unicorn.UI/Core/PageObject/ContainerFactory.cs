using System;
using System.Reflection;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.PageObject
{
    public static class ContainerFactory
    {
        public static void InitContainer<T>(T container)
        {
            FieldInfo[] fields = container.GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(FindAttribute), true) as FindAttribute[];
                if (attributes.Length != 0)
                {
                    Type controlType = field.FieldType;
                    var control = Activator.CreateInstance(controlType);
                    ((IControl)control).Locator = attributes[0].Locator;
                    ((IControl)control).Cached = false;

                    var contextField = control.GetType().GetProperty("ParentSearchContext", BindingFlags.Public | BindingFlags.Instance);
                    contextField.SetValue(control, container);

                    var nameAttribute = field.GetCustomAttribute(typeof(NameAttribute), true) as NameAttribute;

                    if (nameAttribute != null)
                    {
                        ((IControl)control).Name = nameAttribute.Name;
                    }

                    if (control is IContainer)
                    {
                        InitContainer(control);
                    }

                    field.SetValue(container, control);
                }
            }
        }
    }
}
