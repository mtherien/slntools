using System;

namespace CWDev.SLNTools.Core
{
    public class MissingProjectException : Exception
    {
        public MissingProjectException(string message) : base(message)
        { 
        }
    }
}
