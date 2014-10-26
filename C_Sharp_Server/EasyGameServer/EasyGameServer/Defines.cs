using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer
{
    class Defines
    {

        public const int LISTEN_PORT = 9001;
        public const int MAX_CONNECTION = 10000;


        public const int GC_INTERVAL = 5000;
        public const int HEARTBEAT_TIMEOUT = 60000;

        public const int CONNECTION_ERROR_MAX = 2;
    }
}
