using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Unicorn.UI.Core.Controls;
using Unicorn.UI.Core.Controls.Dynamic;
using Unicorn.UI.Core.Driver;

namespace Unicorn.UI.Core.PageObject
{
    /// <summary>
    /// Provides with ability to initialize classes which represent UI controls containers.
    /// </summary>
    public static class ContainerFactory
    {
        private const string ParentContext = "ParentSearchContext";
        private static readonly Type _iControlType = typeof(IControl);

        /// <summary>
        /// Initialize container with child controls.
        /// </summary>
        /// <typeparam name="T">specific contaiter class type</typeparam>
        /// <param name="container">container instance</param>
        public static void InitContainer<T>(T container)
        {
            InitContainerProperties(container);
            InitContainerListProperties(container);
            InitContainerFields(container);
            InitContainerListFields(container);
        }

        private static void InitContainerProperties<T>(T container)
        {
            IEnumerable<PropertyInfo> properties = container.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.PropertyType.GetInterfaces().Contains(_iControlType) && p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                FindAttribute findAttribute = property.GetCustomAttribute<FindAttribute>(true);

                if (findAttribute == null)
                {
                    findAttribute = property.PropertyType.GetCustomAttribute<FindAttribute>(true);
                }

                if (findAttribute != null)
                {
                    Type controlType = property.PropertyType;
                    var control = Activator.CreateInstance(controlType);

                    IControl iControl = ((IControl)control);
                    iControl.Locator = findAttribute.Locator;
                    iControl.Cached = false;

                    PropertyInfo contextField = control.GetType()
                        .GetProperty(ParentContext, BindingFlags.Public | BindingFlags.Instance);

                    contextField.SetValue(control, container);

                    NameAttribute nameAttribute = property.GetCustomAttribute<NameAttribute>(true);

                    if (nameAttribute != null)
                    {
                        iControl.Name = nameAttribute.Name;
                    }

                    if (control is IDynamicControl)
                    {
                        DefineDynamicControl(ref control, property);
                    }
                    else
                    {
                        InitContainer(control);
                    }

                    property.SetValue(container, control);
                }
            }
        }

        private static void InitContainerListProperties<T>(T container)
        {
            IEnumerable<PropertyInfo> properties = container.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.PropertyType.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>) && 
                    i.GetGenericArguments().Any(ga => ga.GetInterfaces().Contains(typeof(IControl)))) 
                    && p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                Type childrensType = property.PropertyType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>))
                    .GetGenericArguments().First(ga => ga.GetInterfaces().Contains(typeof(IControl)));

                FindAttribute findAttribute = property.GetCustomAttribute<FindAttribute>(true);

                if (findAttribute == null)
                {
                    findAttribute = property.PropertyType.GetCustomAttribute<FindAttribute>(true);
                }

                if (findAttribute != null)
                {
                    Type collectionType = typeof(ControlsList<>);
                    Type constructedClass = collectionType.MakeGenericType(childrensType);
                    var list = Activator.CreateInstance(constructedClass, new object[] { container, findAttribute.Locator });
                    property.SetValue(container, list);
                }
            }
        }

        private static void InitContainerFields<T>(T container)
        {
            IEnumerable<FieldInfo> fields = container.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.FieldType.GetInterfaces().Contains(_iControlType));

            foreach (FieldInfo field in fields)
            {
                FindAttribute findAttribute = field.GetCustomAttribute<FindAttribute>(true);

                if (findAttribute == null)
                {
                    findAttribute = field.FieldType.GetCustomAttribute<FindAttribute>(true);
                }

                if (findAttribute != null)
                {
                    Type controlType = field.FieldType;
                    var control = Activator.CreateInstance(controlType);

                    IControl iControl = ((IControl)control);
                    iControl.Locator = findAttribute.Locator;
                    iControl.Cached = false;

                    PropertyInfo contextField = control.GetType()
                        .GetProperty(ParentContext, BindingFlags.Public | BindingFlags.Instance);

                    contextField.SetValue(control, container);

                    NameAttribute nameAttribute = field.GetCustomAttribute<NameAttribute>(true);

                    if (nameAttribute != null)
                    {
                        iControl.Name = nameAttribute.Name;
                    }

                    if (control is IDynamicControl)
                    {
                        DefineDynamicControl(ref control, field);
                    }
                    else
                    {
                        InitContainer(control);
                    }


                    field.SetValue(container, control);
                }
            }
        }

        private static void InitContainerListFields<T>(T container)
        {
            IEnumerable<FieldInfo> fields = container.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.FieldType.GetInterfaces()
                    .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>) && 
                    i.GetGenericArguments().Any(ga => ga.GetInterfaces().Contains(typeof(IControl)))));

            foreach (FieldInfo field in fields)
            {
                Type childrensType = field.FieldType.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>))
                    .GetGenericArguments().First(ga => ga.GetInterfaces().Contains(typeof(IControl)));

                FindAttribute findAttribute = field.GetCustomAttribute<FindAttribute>(true);

                if (findAttribute == null)
                {
                    findAttribute = field.FieldType.GetCustomAttribute<FindAttribute>(true);
                }

                if (findAttribute != null)
                {
                    Type collectionType = typeof(ControlsList<>);
                    Type constructedClass = collectionType.MakeGenericType(childrensType);
                    var list = Activator.CreateInstance(constructedClass, new object[] { container, findAttribute.Locator });
                    field.SetValue(container, list);
                }
            }
        }

        private static void DefineDynamicControl(ref object control, MemberInfo classMember)
        {
            var definitions = classMember.GetCustomAttributes(typeof(DefineAttribute), true) as DefineAttribute[];
            var dictionary = new Dictionary<int, ByLocator>();

            foreach (var definition in definitions)
            {
                dictionary.Add(definition.ElementDefinition, definition.Locator);
            }

            (control as IDynamicControl).Populate(dictionary);
        }
    }
}
