using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGameServer
{
    class credentialManager
    {
        private List<int> m_CredentailPool = new List<int>();

        private System.Object lockThis = new System.Object();

        public credentialManager()
        {
            Random r = new Random();


            for (int i = 0; i < Defines.MAX_CONNECTION; ++i)
            {
                m_CredentailPool.Add( (i*10) + r.Next(0, 9) );
            }

            Shuffle(m_CredentailPool);
        }

        private void Shuffle(List<int> list)
        {
            Random rng = new Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public int GetCredential()
        {
            lock (lockThis)
            {
                if(m_CredentailPool.Count > 0)
                {
                    int cre = m_CredentailPool[0];
                    m_CredentailPool.RemoveAt(0);
                    return cre;
                }
            }
            return -1;
        }

        public void ReturnCredential(int credential)
        {
            lock (lockThis)
            {
                m_CredentailPool.Add(credential);
            }
        }


        public int GetPoolLeft()
        {
            return m_CredentailPool.Count;
        }


    }
}
