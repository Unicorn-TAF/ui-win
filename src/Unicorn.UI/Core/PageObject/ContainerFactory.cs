using System;
using System.Reflection;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.PageObject
{
    public static class ContainerFactory
    {
        public static void InitContainer<T>(T container)
        {
            PropertyInfo[] fields = container.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            foreach (var property in fields)
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

                    if (control is IContainer)
                    {
                        InitContainer(control);
                    }

                    property.SetValue(container, control);
                }
            }
        }
    }
}
