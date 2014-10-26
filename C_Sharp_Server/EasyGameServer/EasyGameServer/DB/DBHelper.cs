using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data;


namespace EasyGameServer.DB
{
    class DBHelper
    {
        private MySqlConnection m_Connection = new MySqlConnection( SQLStatement.DB_SERVER_PATH );

        public bool Initialize()
        {
            try
            {
                m_Connection.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }

            return true;
        }

        public void Test()
        {
            try
            {
                string sql = "SELECT * FROM  `next_cpp_pye_cha_jang_RANK` LIMIT 0 , 30";

                MySqlCommand cmd = new MySqlCommand(sql, m_Connection);
                MySqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Console.WriteLine("{0}: {1} | {2}", rdr["idx"], rdr["player_name"], rdr["player_time"]);
                }
                rdr.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        public void TestSP()
        {
                /*
                OUT oGameId int,
               IN iId VARCHAR(20),
               IN iName VARCHAR(20),
               OUT oPosX DOUBLE,
               OUT oPosY DOUBLE,
               OUT oPosZ DOUBLE
                */

                MySqlCommand cmd = new MySqlCommand();

                cmd.Connection = m_Connection;
                cmd.CommandText = "SP_AddUser";
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(new MySqlParameter("@oGameId", MySqlDbType.Int32));
                cmd.Parameters["@oGameId"].Direction = ParameterDirection.Output;


                cmd.Parameters.AddWithValue("@iId", "testID66");
                cmd.Parameters["@iId"].Direction = ParameterDirection.Input;

                cmd.Parameters.AddWithValue("@iName", "testNAME66");
                cmd.Parameters["@iName"].Direction = ParameterDirection.Input;


                cmd.Parameters.Add(new MySqlParameter("@oPosX", MySqlDbType.Double));
                cmd.Parameters["@oPosX"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new MySqlParameter("@oPosY", MySqlDbType.Double));
                cmd.Parameters["@oPosY"].Direction = ParameterDirection.Output;
                cmd.Parameters.Add(new MySqlParameter("@oPosZ", MySqlDbType.Double));
                cmd.Parameters["@oPosZ"].Direction = ParameterDirection.Output;

                cmd.ExecuteScalar();
                Console.WriteLine("{0}", cmd.Parameters["@oPosX"].Value);


        }

    }
}
