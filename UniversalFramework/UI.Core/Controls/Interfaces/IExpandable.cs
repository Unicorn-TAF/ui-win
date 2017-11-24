
namespace Unicorn.UI.Core.Controls.Interfaces
{
    public interface IExpandable
    {
        bool Expanded { get; }

        bool Expand();

        bool Collapse();

    }
}
