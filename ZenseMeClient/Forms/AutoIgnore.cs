using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using ZenseMe.Lib.DataAccessObjects;

namespace ZenseMe.Client.Forms
{
    public partial class AutoIgnore : Form
    {
        private AutoIgnoreDAO AutoIgnoreDAO;

        public AutoIgnore()
        {
            InitializeComponent();
            AutoIgnoreDAO = new AutoIgnoreDAO();
        }

        private void button_addRule_Click(object sender, EventArgs e)
        {
            string field = combo_field.Text;
            string value = textBox_value.Text;
            if (value == "")
            {
                return;
            }

            textBox_value.Text = "";

            DataRowView dataRowView = (DataRowView)autoIgnoreRuleBindingSource.AddNew();
            dataRowView.BeginEdit();

            DataRow dataRow = dataRowView.Row;
            dataRow["field"] = field;
            dataRow["value"] = value;

            dataRowView.EndEdit();
        }

        private void AutoIgnore_Load(object sender, EventArgs e)
        {
            AutoIgnoreDAO.SetBindingSource(ref autoIgnoreRuleBindingSource);
            AutoIgnoreDAO.FillData();
        }

        private void AutoIgnore_FormClosing(object sender, FormClosingEventArgs e)
        {
            AutoIgnoreDAO.SubmitData();
        }
    }
}
