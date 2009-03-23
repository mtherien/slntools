using System;

namespace CWDev.VSSolutionTools.Core
{
    public class MissingProjectException : Exception
    {
        public MissingProjectException(string message) : base(message)
        { 
        }
    }
}
