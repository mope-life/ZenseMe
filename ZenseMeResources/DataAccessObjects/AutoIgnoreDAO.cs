using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZenseMe.Lib.Objects;
using ZenseMe.Lib.Storage;

namespace ZenseMe.Lib.DataAccessObjects
{
    public class AutoIgnoreDAO
    {
        private Database database;
        private SQLiteDataAdapter dataAdapter;
        private BindingSource bindingSource;

        public AutoIgnoreDAO()
        {
            database = new Database();
            dataAdapter = database.GetDataAdapter("SELECT * FROM auto_ignore_rules");
        }

        public void SetBindingSource(ref BindingSource bindingSource)
        {
            this.bindingSource = bindingSource;
        }

        public void FillData()
        {
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);

            bindingSource.DataSource = dataTable;
        }

        public void SubmitData()
        {
            Console.WriteLine("Updating autoignore rules in database.");

            dataAdapter.Update((DataTable)bindingSource.DataSource);
        }
    }
}
