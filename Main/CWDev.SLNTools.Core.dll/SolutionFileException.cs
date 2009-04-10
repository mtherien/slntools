using System;
using System.Collections.Generic;
using System.Text;

namespace CWDev.SLNTools.Core
{
    class SolutionFileException
        : Exception
    {
        public SolutionFileException(string message)
            : base(message)
        {
        }

        public SolutionFileException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
