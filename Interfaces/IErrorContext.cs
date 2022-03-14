using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EntityAbstractions.Persistence.Interfaces
{
    public interface IErrorContext
    {
        public bool HasErrors();
    }
}