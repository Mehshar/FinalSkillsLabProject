using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalSkillsLabProject.Common.Exceptions
{
    [Serializable]
    public class DuplicationException : Exception
    {
        public DuplicationException(string message) : base(message) { }
    }
}
