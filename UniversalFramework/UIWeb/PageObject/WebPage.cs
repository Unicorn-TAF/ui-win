using System;
using System.Diagnostics;
using System.Reflection;
using Unicorn.UIWeb.Driver;
using Unicorn.UIWeb.UI;

namespace Unicorn.UIWeb.PageObject
{
    public class WebPage
    {
        public virtual void WaitForPageLoading() { throw new NotImplementedException(); }


        protected dynamic FindBy<T>() where T : WebControl
        {
            StackFrame caller = new StackFrame(1);
            MethodBase method = caller.GetMethod();
            string propName = method.Name.Substring(4);
            Type type = method.ReflectedType;
            PropertyInfo pi = type.GetProperty(propName);
            object[] attributes = pi.GetCustomAttributes(typeof(FindByAttribute), true);
            if (attributes != null && attributes.Length == 1)
            {
                FindByAttribute ssAttr = attributes[0] as FindByAttribute;

                return WebDriver.Instance.Find<T>(ssAttr.By, ssAttr.Locator);
            }
            throw new Exception("Unable to find control");
        }
    }
}
