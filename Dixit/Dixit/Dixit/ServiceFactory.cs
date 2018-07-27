using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dixit
{
    public class ServiceFactory
    {
        public static ServiceFactory Instance = new ServiceFactory();

        public Random Random = new Random();
    }
}
