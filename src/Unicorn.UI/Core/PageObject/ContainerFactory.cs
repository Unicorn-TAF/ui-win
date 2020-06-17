using System;
using System.Linq;
using System.Reflection;
using Unicorn.UI.Core.Controls;

namespace Unicorn.UI.Core.PageObject
{
    /// <summary>
    /// Provides with ability to initialize classes which represent UI controls containers.
    /// </summary>
    public static class ContainerFactory
    {
        private const string ParentContext = "ParentSearchContext";
        private static Type iControlType = typeof(IControl);

        /// <summary>
        /// Initialize container with child controls.
        /// </summary>
        /// <typeparam name="T">specific contaiter class type</typeparam>
        /// <param name="container">container instance</param>
        public static void InitContainer<T>(T container)
        {
            InitContainerProperties(container);
            InitContainerFields(container);
        }

        private static void InitContainerProperties<T>(T container)
        {
            var properties = container.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.PropertyType.GetInterfaces().Contains(iControlType));

            foreach (var property in properties)
            {
                var findAttribute = property.GetCustomAttribute(typeof(FindAttribute), true) as FindAttribute;

                if (findAttribute != null)
                {
                    Type controlType = property.PropertyType;
                    var control = Activator.CreateInstance(controlType);
                    
                    var iControl = ((IControl)control);
                    iControl.Locator = findAttribute.Locator;
                    iControl.Cached = false;

                    var contextField = control.GetType().GetProperty(ParentContext, BindingFlags.Public | BindingFlags.Instance);
                    contextField.SetValue(control, container);

                    var nameAttribute = property.GetCustomAttribute(typeof(NameAttribute), true) as NameAttribute;

                    if (nameAttribute != null)
                    {
                        iControl.Name = nameAttribute.Name;
                    }

                    InitContainer(control);

                    property.SetValue(container, control);
                }
            }
        }

        private static void InitContainerFields<T>(T container)
        {
            var fields = container.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.FieldType.GetInterfaces().Contains(iControlType));

            foreach (var field in fields)
            {
                var findAttribute = field.GetCustomAttribute(typeof(FindAttribute), true) as FindAttribute;

                if (findAttribute != null)
                {
                    Type controlType = field.FieldType;
                    var control = Activator.CreateInstance(controlType);

                    var iControl = ((IControl)control);
                    iControl.Locator = findAttribute.Locator;
                    iControl.Cached = false;

                    var contextField = control.GetType().GetProperty(ParentContext, BindingFlags.Public | BindingFlags.Instance);
                    contextField.SetValue(control, container);

                    var nameAttribute = field.GetCustomAttribute(typeof(NameAttribute), true) as NameAttribute;

                    if (nameAttribute != null)
                    {
                        iControl.Name = nameAttribute.Name;
                    }

                    InitContainer(control);

                    field.SetValue(container, control);
                }
            }
        }
    }
}
