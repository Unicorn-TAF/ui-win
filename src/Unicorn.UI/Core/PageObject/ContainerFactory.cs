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
        private static readonly Type ControlInterface = typeof(IControl);
        private static readonly Type ControlsListType = typeof(ControlsList<>);
        private static readonly Type CollectionType = typeof(ICollection<>);

        /// <summary>
        /// Initializes container with child controls. Both single controls and controls lists are initialized. <br/>
        /// To be initialized controls should implement <see cref="IControl"/> and 
        /// have <see cref="FindAttribute"/> or it's derivatives specified for field/property or for type itself.
        /// </summary>
        /// <typeparam name="T">specific contaiter class type</typeparam>
        /// <param name="container">container instance</param>
        public static void InitContainer<T>(T container)
        {
            /* 
             * The order makes sense as initialization of fields also initializes properties backing fields
             * so need to init properties in the second order to rewrite backing fields correctly.
            */
            InitContainerFields(container);
            InitContainerListFields(container);
            InitContainerProperties(container);
            InitContainerListProperties(container);
        }

        private static void InitContainerProperties<T>(T container)
        {
            IEnumerable<PropertyInfo> properties = container.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => p.PropertyType.GetInterfaces().Contains(ControlInterface) && p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                Type controlType = property.PropertyType;
                ByLocator locator = GetControlLocator(property, controlType);

                if (locator != null)
                {
                    object control = InitControl(controlType, locator, container, property);
                    property.SetValue(container, control);
                }
            }
        }

        private static void InitContainerListProperties<T>(T container)
        {
            IEnumerable<PropertyInfo> properties = container.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => IsControlsList(p.PropertyType) && p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                Type controlType = GetChildrenType(property.PropertyType);
                ByLocator locator = GetControlLocator(property, controlType);

                if (locator != null)
                {
                    Type constructedClass = ControlsListType.MakeGenericType(controlType);
                    object list = Activator.CreateInstance(constructedClass, new object[] { container, locator });
                    property.SetValue(container, list);
                }
            }
        }

        private static void InitContainerFields<T>(T container)
        {
            IEnumerable<FieldInfo> fields = container.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(f => f.FieldType.GetInterfaces().Contains(ControlInterface));

            foreach (FieldInfo field in fields)
            {
                Type controlType = field.FieldType;
                ByLocator locator = GetControlLocator(field, controlType);

                if (locator != null)
                {
                    object control = InitControl(controlType, locator, container, field);
                    field.SetValue(container, control);
                }
            }
        }

        private static void InitContainerListFields<T>(T container)
        {
            IEnumerable<FieldInfo> fields = container.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(p => IsControlsList(p.FieldType));

            foreach (FieldInfo field in fields)
            {
                Type controlType = GetChildrenType(field.FieldType);
                ByLocator locator = GetControlLocator(field, controlType);

                if (locator != null)
                {
                    Type constructedClass = ControlsListType.MakeGenericType(controlType);
                    object list = Activator.CreateInstance(constructedClass, new object[] { container, locator });
                    field.SetValue(container, list);
                }
            }
        }

        private static object InitControl(Type controlType, ByLocator locator, object parent, MemberInfo memberInfo)
        {
            object control = Activator.CreateInstance(controlType);

            IControl iControl = ((IControl)control);
            iControl.Locator = locator;
            iControl.Cached = false;

            PropertyInfo contextField = control.GetType()
                .GetProperty(ParentContext, BindingFlags.Public | BindingFlags.Instance);

            contextField.SetValue(control, parent);

            NameAttribute nameAttribute = memberInfo.GetCustomAttribute<NameAttribute>(true);

            if (nameAttribute == null)
            {
                nameAttribute = controlType.GetCustomAttribute<NameAttribute>(true);
            }

            iControl.Name = nameAttribute?.Name;

            if (control is IDynamicControl)
            {
                DefineDynamicControl(ref control, memberInfo);
            }
            else
            {
                InitContainer(control);
            }

            return control;
        }

        private static void DefineDynamicControl(ref object control, MemberInfo classMember)
        {
            var definitions = classMember.GetCustomAttributes(typeof(DefineAttribute), true) as DefineAttribute[];
            var dictionary = new Dictionary<int, ByLocator>();

            Array.ForEach(definitions, 
                d => dictionary.Add(d.ElementDefinition, d.Locator));

            (control as IDynamicControl).Populate(dictionary);
        }

        private static ByLocator GetControlLocator(MemberInfo memberInfo, Type controlType)
        {
            FindAttribute findAttribute = memberInfo.GetCustomAttribute<FindAttribute>(true);

            if (findAttribute == null)
            {
                findAttribute = controlType.GetCustomAttribute<FindAttribute>(true);
            }

            return findAttribute?.Locator;
        }

        private static Type GetChildrenType(Type memberType) =>
            memberType.GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == CollectionType)
            .GetGenericArguments().First(ga => ga.GetInterfaces().Contains(ControlInterface));

        private static bool IsControlsList(Type type) =>
            type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == CollectionType &&
                i.GetGenericArguments().Any(ga => ga.GetInterfaces().Contains(ControlInterface)));
    }
}
