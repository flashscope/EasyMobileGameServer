using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public interface PacketHandler
    {
        void Run(int cridential, string payload);
    }

