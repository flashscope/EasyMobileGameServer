using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public interface PacketHandler
    {
        void Run(int credential, string payload);
    }

