namespace ZenseMe.Client.Forms
{
    partial class AutoIgnore
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoIgnore));
            this.combo_field = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox_value = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button_addRule = new System.Windows.Forms.Button();
            this.dataGridView_rules = new System.Windows.Forms.DataGridView();
            this.autoIgnoreRuleBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.field = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rules)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoIgnoreRuleBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // combo_field
            // 
            this.combo_field.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.combo_field.FormattingEnabled = true;
            this.combo_field.Items.AddRange(new object[] {
            "Artist",
            "Album",
            "Genre"});
            this.combo_field.Location = new System.Drawing.Point(15, 203);
            this.combo_field.Name = "combo_field";
            this.combo_field.Size = new System.Drawing.Size(83, 21);
            this.combo_field.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(285, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "Automatically ignore tracks that meet the following\r\nconditions while fetching (u" +
    "se \'Delete\' key to remove):";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 187);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(88, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Add a new rule:";
            // 
            // textBox_value
            // 
            this.textBox_value.Location = new System.Drawing.Point(125, 203);
            this.textBox_value.Name = "textBox_value";
            this.textBox_value.Size = new System.Drawing.Size(100, 22);
            this.textBox_value.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(104, 206);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(15, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "=";
            // 
            // button_addRule
            // 
            this.button_addRule.Location = new System.Drawing.Point(231, 202);
            this.button_addRule.Name = "button_addRule";
            this.button_addRule.Size = new System.Drawing.Size(52, 23);
            this.button_addRule.TabIndex = 7;
            this.button_addRule.Text = "Add";
            this.button_addRule.UseVisualStyleBackColor = true;
            this.button_addRule.Click += new System.EventHandler(this.button_addRule_Click);
            // 
            // dataGridView_rules
            // 
            this.dataGridView_rules.AllowUserToAddRows = false;
            this.dataGridView_rules.AutoGenerateColumns = false;
            this.dataGridView_rules.BackgroundColor = System.Drawing.SystemColors.Window;
            this.dataGridView_rules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_rules.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.field,
            this.value});
            this.dataGridView_rules.DataSource = this.autoIgnoreRuleBindingSource;
            this.dataGridView_rules.Location = new System.Drawing.Point(15, 38);
            this.dataGridView_rules.Name = "dataGridView_rules";
            this.dataGridView_rules.RowHeadersWidth = 25;
            this.dataGridView_rules.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_rules.Size = new System.Drawing.Size(268, 146);
            this.dataGridView_rules.TabIndex = 8;
            // 
            // autoIgnoreRuleBindingSource
            // 
            this.autoIgnoreRuleBindingSource.AllowNew = true;
            this.autoIgnoreRuleBindingSource.Sort = "";
            // 
            // field
            // 
            this.field.DataPropertyName = "field";
            this.field.HeaderText = "Field";
            this.field.Name = "field";
            this.field.Width = 80;
            // 
            // value
            // 
            this.value.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.value.DataPropertyName = "value";
            this.value.HeaderText = "Value";
            this.value.Name = "value";
            // 
            // AutoIgnore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(299, 236);
            this.Controls.Add(this.dataGridView_rules);
            this.Controls.Add(this.button_addRule);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBox_value);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.combo_field);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "AutoIgnore";
            this.Text = "ZenseMe - AutoIgnore";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AutoIgnore_FormClosing);
            this.Load += new System.EventHandler(this.AutoIgnore_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_rules)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.autoIgnoreRuleBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox combo_field;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_value;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button_addRule;
        private System.Windows.Forms.DataGridView dataGridView_rules;
        private System.Windows.Forms.BindingSource autoIgnoreRuleBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn field;
        private System.Windows.Forms.DataGridViewTextBoxColumn value;
    }
}