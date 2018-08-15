using System;
using System.Reflection;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.PageObject
{
    public static class ContainerFactory
    {
        public static void InitContainer<T>(T container)
        {
            FieldInfo[] fields = container.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (FieldInfo field in fields)
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
