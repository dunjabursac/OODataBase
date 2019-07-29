using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class Transaction
    {
        public DBManager db = DBManager.DBM_Instance;

        private Dictionary<string, object> oldItems = new Dictionary<string, object>();
        private Dictionary<string, object> newItems = new Dictionary<string, object>();


        
        public Transaction()
        {

        }

        
        public void Create(object item)
        {
            
        }

        public object Read(string choosenType, int id, int version)
        {
            return new object();
        }

        public void Update(object item)
        {

        }

        public void Delete(string choosenType, int id, int version)
        {

        }


        public bool Commit()
        {
            return true;
        }

        public bool Rollback()
        {
            return true;
        }
    }
}
