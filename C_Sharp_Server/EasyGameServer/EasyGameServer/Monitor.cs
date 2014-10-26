using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

using System.Diagnostics;

namespace EasyGameServer
{
    class Monitor
    {
        private bool m_RunMonitor = true;
        private int m_RefreshTime = 5000;

        private PerformanceCounter m_CpuCounter;
        private PerformanceCounter m_RamCounter;

        public Monitor(int refreshTime)
        {
            m_CpuCounter = new PerformanceCounter();

            m_CpuCounter.CategoryName = "Processor";
            m_CpuCounter.CounterName = "% Processor Time";
            m_CpuCounter.InstanceName = "_Total";

            m_RamCounter = new PerformanceCounter("Memory", "Available MBytes");

            m_RefreshTime = refreshTime;
        }

        public void Run()
        {
            while (m_RunMonitor)
            {
                Console.WriteLine("======================================");
                Console.WriteLine("CPU Usage:" + m_CpuCounter.NextValue() + "%");
                Console.WriteLine("AvailableRAM:" + m_RamCounter.NextValue() + "MB");
                Console.WriteLine("CredentialPoolLeft:" + EasyGameServer.g_credentialManager.GetPoolLeft());
                Thread.Sleep(m_RefreshTime);
            }
        }


        public void StopMonitoring()
        {
            m_RunMonitor = false;
        }
    }
}
