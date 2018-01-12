using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Unicorn.Core.Testing.Tests.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    class ParameterizedAttribute : Attribute
    {
    }
}
