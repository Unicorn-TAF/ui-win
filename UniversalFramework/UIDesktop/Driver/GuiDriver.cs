﻿using System;
using System.Diagnostics;
using System.Windows.Automation;
using Unicorn.UICore.Driver;
using Unicorn.UIDesktop.UI.Controls;

namespace Unicorn.UIDesktop.Driver
{
    public class GuiDriver : GuiSearchContext, IDriver
    {
        private static GuiDriver _instance = null;
        private Process CurrentProcess;

        public static GuiDriver Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GuiDriver(); ;
                    _instance.SearchContext = AutomationElement.RootElement;
                }
                return _instance;
            }
        }

        public void Close()
        {
            try
            {
                new Window(AutomationElement.FromHandle(CurrentProcess.MainWindowHandle)).Close();
            }
            catch
            {

            }
        }

        public void Get(string path)
        {
            CurrentProcess = Process.Start(path);
        }

        public void SetImplicitlyWait(TimeSpan time)
        {
            ImplicitlyWait = time;
        }

    }
}
