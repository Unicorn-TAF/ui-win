![Nuget](https://img.shields.io/nuget/v/Unicorn.UI.Win?style=plastic) ![Nuget](https://img.shields.io/nuget/dt/Unicorn.UI.Win?style=plastic)

# Unicorn.UI.Win

Implementation of desktop GUI interaction based on UIA3 library.

* GUI Driver implementation
* Typified controls implementations
* PageObject implementation
* UI actions
* User input clients (mouse, keyboard)

PageObject example
```csharp
public class TestAppWindow : Window
{
    public TestAppWindow()
    {
    }

    public TestAppWindow(IUIAutomationElement instance)
        : base(instance)
    {
    }

    // Control of any type derived from WebControl could be a part of page object.
    // Controls implementing predefined controls interfaces allow to apply type specific matchers 
    // to make tests and assertions easier and more readable.
    [Name("'Switch app' button"), ById("switchAppButton")]
    public Button SwitchAppToggle { get; set; }

    // Controls could be located using generic FindAttribute.
    [Name("'Hello World' view"), [Find(Using.Id, "HelloWorldView")]
    public HelloWorldView HelloWorldView { get; set; }

    // Name attribute allows control to be self-reportable in combination with matchers 
    // and to be logged with scpecified name
    [Name("'Hello World' view"), ById("SampleView")]
    public SamplesView SamplesView { get; set; }

    [Name("Modal window"), ByClass("#32770")]
    public ModalWindow Modal {  get; set; }
}
```


Creating custom application
```csharp
// Describes desktop application (TestWindowsApp application).
// should inherit Application.
public class TestWindowsApp : Application
{
    // Application constructor. Calls base constructor with path to application and application executable name.
    public TestWindowsApp() : base("", "path_to_application")
    { 
    }

    // main application window
    public TestAppWindow Window { get; private set; }

    public override void Start()
    {
        // Application.Start starts app by path provided in constructor and stores Process info
        base.Start();

        IUIAutomationElement element = null;

        // Initializing built-in waiter
        DefaultWait wait = new DefaultWait(TimeSpan.FromSeconds(5), TimeSpan.FromMilliseconds(250));
        wait.IgnoreExceptionTypes(typeof(COMException));

        // And wait until window with process window handle is found
        wait.Until(() =>
        {
            element = WinDriver.Instance.Driver.ElementFromHandle(Process.MainWindowHandle);
            return true;
        });

        Window = new TestAppWindow(element);
    }
}
```

Built-in matchers
```csharp
Assert.That(application.Window, UI.Window.HasTitle("'Hello World' app"));
Assert.That(application.TitleDropdown, UI.Dropdown.HasSelectedValue(""));
Assert.That(application.NameInput, UI.TextInput.HasValue(string.Empty));
```