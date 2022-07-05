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
                .Where(p => p.PropertyType.GetInterfaces().Contains(_iControlType) && p.CanWrite);

            foreach (PropertyInfo property in properties)
            {
                ByLocator locator = GetControlLocator(property, property.PropertyType);

                if (locator != null)
                {
                    var control = InitControl(property.PropertyType, locator, container, property);
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
                Type childrensType = GetChildrenType(property.PropertyType);
                ByLocator locator = GetControlLocator(property, property.PropertyType);

                if (locator != null)
                {
                    Type collectionType = typeof(ControlsList<>);
                    Type constructedClass = collectionType.MakeGenericType(childrensType);
                    var list = Activator.CreateInstance(constructedClass, new object[] { container, locator });
                    property.SetValue(container, list);
                }
            }
        }

        private static void InitContainerFields<T>(T container)
        {
            IEnumerable<FieldInfo> fields = container.GetType()
                .GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(f => f.FieldType.GetInterfaces().Contains(_iControlType));

            foreach (FieldInfo field in fields)
            {
                ByLocator locator = GetControlLocator(field, field.FieldType);

                if (locator != null)
                {
                    var control = InitControl(field.FieldType, locator, container, field);
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
                Type childrensType = GetChildrenType(field.FieldType);
                ByLocator locator = GetControlLocator(field, field.FieldType);

                if (field != null)
                {
                    Type collectionType = typeof(ControlsList<>);
                    Type constructedClass = collectionType.MakeGenericType(childrensType);
                    var list = Activator.CreateInstance(constructedClass, new object[] { container, locator });
                    field.SetValue(container, list);
                }
            }
        }

        private static object InitControl(Type controlType, ByLocator locator, object parent, MemberInfo memberInfo)
        {
            var control = Activator.CreateInstance(controlType);

            IControl iControl = ((IControl)control);
            iControl.Locator = locator;
            iControl.Cached = false;

            PropertyInfo contextField = control.GetType()
                .GetProperty(ParentContext, BindingFlags.Public | BindingFlags.Instance);

            contextField.SetValue(control, parent);

            NameAttribute nameAttribute = memberInfo.GetCustomAttribute<NameAttribute>(true);

            if (nameAttribute != null)
            {
                iControl.Name = nameAttribute.Name;
            }

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

            foreach (var definition in definitions)
            {
                dictionary.Add(definition.ElementDefinition, definition.Locator);
            }

            (control as IDynamicControl).Populate(dictionary);
        }

        private static ByLocator GetControlLocator(MemberInfo memberInfo, Type type)
        {
            FindAttribute findAttribute = memberInfo.GetCustomAttribute<FindAttribute>(true);

            if (findAttribute == null)
            {
                findAttribute = type.GetCustomAttribute<FindAttribute>(true);
            }

            return findAttribute?.Locator;
        }

        private static Type GetChildrenType(Type memberType) =>
            memberType.GetInterfaces()
            .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICollection<>))
            .GetGenericArguments().First(ga => ga.GetInterfaces().Contains(typeof(IControl)));
    }
}
