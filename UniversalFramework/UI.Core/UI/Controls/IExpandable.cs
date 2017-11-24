
namespace Unicorn.UI.Core.UI.Controls
{
    public interface IExpandable
    {
        bool Expanded { get; }

        bool Expand();

        bool Collapse();

    }
}
