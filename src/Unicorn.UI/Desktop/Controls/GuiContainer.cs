using System;
using System.Reflection;
using System.Windows.Automation;
using Unicorn.Core.Logging;
using Unicorn.UI.Core.Driver;
using Unicorn.UI.Core.PageObject;
using Unicorn.UI.Desktop.Controls.Typified;

namespace Unicorn.UI.Desktop.Controls
{
    public abstract class GuiContainer : GuiControl, IContainer
    {
        public GuiContainer() : base()
        {
        }

        public GuiContainer(AutomationElement instance) : base(instance)
        {
        }

        public override AutomationElement Instance
        {
            get
            {
                if (!this.Cached)
                {
                    this.SearchContext = GetNativeControlFromParentContext(this.Locator, this.GetType());
                }

                return this.SearchContext;
            }

            set
            {
                this.SearchContext = value;
                this.Init();
            }
        }

        public void Init()
        {
            FieldInfo[] fields = GetType().GetFields();
            foreach (FieldInfo field in fields)
            {
                var attributes = field.GetCustomAttributes(typeof(FindAttribute), true) as FindAttribute[];
                if (attributes.Length != 0)
                {
                    Type controlType = field.FieldType;
                    var control = Activator.CreateInstance(controlType);
                    ((GuiControl)control).Locator = attributes[0].Locator;
                    ((GuiControl)control).Cached = false;
                    ((GuiControl)control).ParentSearchContext = this;

                    var nameAttribute = field.GetCustomAttribute(typeof(NameAttribute), true) as NameAttribute;

                    if (nameAttribute != null)
                    {
                        ((GuiControl)control).Name = nameAttribute.Name;
                    }

                    if (controlType.IsSubclassOf(typeof(GuiContainer)))
                    {
                        ((GuiContainer)control).Init();
                    }

                    field.SetValue(this, control);
                }
            }
        }

        public virtual void ClickButton(string locator)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Click '{locator}' button");

            Button button = Find<Button>(ByLocator.Name(locator));
            button.Click();
        }

        public virtual void InputText(string locator, string text)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Input Text '{text}' to '{locator}' field");

            TextInput edit = Find<TextInput>(ByLocator.Name(locator));
            edit.SendKeys(text);
        }

        public virtual bool SelectRadio(string locator)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Select '{locator}' radio button");

            Radio radio = Find<Radio>(ByLocator.Name(locator));
            return radio.Select();
        }

        public virtual bool SetCheckbox(string locator, bool @checked)
        {
            Logger.Instance.Log(LogLevel.Debug, $"Set checkbox '{locator}' to '{@checked}'");

            Checkbox checkbox = Find<Checkbox>(ByLocator.Name(locator));
            return @checked ? checkbox.Check() : checkbox.Uncheck();
        }
    }
}
