using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace EasyGameServer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        private static EasyGameServer server = null;
        private static Thread serverThread = null;


        private void bu_server_start_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                System.Diagnostics.Process.GetCurrentProcess().PriorityClass = System.Diagnostics.ProcessPriorityClass.RealTime;
                server = new EasyGameServer();
                serverThread = new Thread(server.Run);
                serverThread.Start();
            }
            
        }

        private void bu_test_Click(object sender, EventArgs e)
        {
            DB.DBHelper dbHelper = new DB.DBHelper();
            if( dbHelper.Initialize() )
            {
                Console.WriteLine("DB INIT OK");
                dbHelper.TestSP();
            }
            else
            {
                Console.WriteLine("DB INIT FAIL");
            }
            
        }

        private void bu_console_clear_Click(object sender, EventArgs e)
        {
            Console.Clear();
        }

    }
}
