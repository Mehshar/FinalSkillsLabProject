using System;

namespace FinalSkillsLabProject.Common.Exceptions
{
    [Serializable]
    public class DuplicationException : Exception
    {
        public DuplicationException(string message) : base(message) { }
    }
}
