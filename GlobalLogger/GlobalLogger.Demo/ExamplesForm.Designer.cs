using GlobalLogger.Demo.Models;

namespace GlobalLogger.Demo
{
    partial class ExamplesForm
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
            this.btnCreateCashier = new System.Windows.Forms.Button();
            this.btnCreateRandomEmployees = new System.Windows.Forms.Button();
            this.btnCreateClerk = new System.Windows.Forms.Button();
            this.btnCreateAccountant = new System.Windows.Forms.Button();
            this.btnCreateStoreManager = new System.Windows.Forms.Button();
            this.grdViewEmployees = new System.Windows.Forms.DataGridView();
            this.numCreateRandomEmployees = new System.Windows.Forms.NumericUpDown();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnTimeLog = new System.Windows.Forms.Button();
            this.btnLogInfo = new System.Windows.Forms.Button();
            this.btnSimpleLogger = new System.Windows.Forms.Button();
            this.btnException = new System.Windows.Forms.Button();
            this.btnNamedLogger = new System.Windows.Forms.Button();
            this.firstNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lastNameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ageDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.badgeNumberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bdsEmployees = new System.Windows.Forms.BindingSource(this.components);
            this.btnComplexLogger = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdViewEmployees)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCreateRandomEmployees)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bdsEmployees)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCreateCashier
            // 
            this.btnCreateCashier.Location = new System.Drawing.Point(14, 6);
            this.btnCreateCashier.Name = "btnCreateCashier";
            this.btnCreateCashier.Size = new System.Drawing.Size(134, 23);
            this.btnCreateCashier.TabIndex = 0;
            this.btnCreateCashier.Text = "Create cashier";
            this.btnCreateCashier.UseVisualStyleBackColor = true;
            // 
            // btnCreateRandomEmployees
            // 
            this.btnCreateRandomEmployees.Location = new System.Drawing.Point(14, 35);
            this.btnCreateRandomEmployees.Name = "btnCreateRandomEmployees";
            this.btnCreateRandomEmployees.Size = new System.Drawing.Size(171, 23);
            this.btnCreateRandomEmployees.TabIndex = 2;
            this.btnCreateRandomEmployees.Text = "Create N random employees";
            this.btnCreateRandomEmployees.UseVisualStyleBackColor = true;
            // 
            // btnCreateClerk
            // 
            this.btnCreateClerk.Location = new System.Drawing.Point(154, 6);
            this.btnCreateClerk.Name = "btnCreateClerk";
            this.btnCreateClerk.Size = new System.Drawing.Size(134, 23);
            this.btnCreateClerk.TabIndex = 4;
            this.btnCreateClerk.Text = "Create clerk";
            this.btnCreateClerk.UseVisualStyleBackColor = true;
            // 
            // btnCreateAccountant
            // 
            this.btnCreateAccountant.Location = new System.Drawing.Point(294, 6);
            this.btnCreateAccountant.Name = "btnCreateAccountant";
            this.btnCreateAccountant.Size = new System.Drawing.Size(134, 23);
            this.btnCreateAccountant.TabIndex = 5;
            this.btnCreateAccountant.Text = "Create accountant";
            this.btnCreateAccountant.UseVisualStyleBackColor = true;
            // 
            // btnCreateStoreManager
            // 
            this.btnCreateStoreManager.Location = new System.Drawing.Point(434, 6);
            this.btnCreateStoreManager.Name = "btnCreateStoreManager";
            this.btnCreateStoreManager.Size = new System.Drawing.Size(134, 23);
            this.btnCreateStoreManager.TabIndex = 6;
            this.btnCreateStoreManager.Text = "Create store manager";
            this.btnCreateStoreManager.UseVisualStyleBackColor = true;
            // 
            // grdViewEmployees
            // 
            this.grdViewEmployees.AutoGenerateColumns = false;
            this.grdViewEmployees.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdViewEmployees.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.firstNameDataGridViewTextBoxColumn,
            this.lastNameDataGridViewTextBoxColumn,
            this.ageDataGridViewTextBoxColumn,
            this.badgeNumberDataGridViewTextBoxColumn,
            this.positionDataGridViewTextBoxColumn});
            this.grdViewEmployees.DataSource = this.bdsEmployees;
            this.grdViewEmployees.Location = new System.Drawing.Point(14, 64);
            this.grdViewEmployees.Name = "grdViewEmployees";
            this.grdViewEmployees.Size = new System.Drawing.Size(694, 646);
            this.grdViewEmployees.TabIndex = 7;
            // 
            // numCreateRandomEmployees
            // 
            this.numCreateRandomEmployees.Location = new System.Drawing.Point(190, 38);
            this.numCreateRandomEmployees.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCreateRandomEmployees.Name = "numCreateRandomEmployees";
            this.numCreateRandomEmployees.Size = new System.Drawing.Size(120, 20);
            this.numCreateRandomEmployees.TabIndex = 9;
            this.numCreateRandomEmployees.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(9, 10);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(741, 739);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnCreateStoreManager);
            this.tabPage1.Controls.Add(this.btnCreateAccountant);
            this.tabPage1.Controls.Add(this.grdViewEmployees);
            this.tabPage1.Controls.Add(this.numCreateRandomEmployees);
            this.tabPage1.Controls.Add(this.btnCreateClerk);
            this.tabPage1.Controls.Add(this.btnCreateCashier);
            this.tabPage1.Controls.Add(this.btnCreateRandomEmployees);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage1.Size = new System.Drawing.Size(733, 713);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Employees Database";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnComplexLogger);
            this.tabPage2.Controls.Add(this.btnNamedLogger);
            this.tabPage2.Controls.Add(this.btnException);
            this.tabPage2.Controls.Add(this.btnTimeLog);
            this.tabPage2.Controls.Add(this.btnLogInfo);
            this.tabPage2.Controls.Add(this.btnSimpleLogger);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(2);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(2);
            this.tabPage2.Size = new System.Drawing.Size(733, 713);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Examples";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnTimeLog
            // 
            this.btnTimeLog.Location = new System.Drawing.Point(166, 159);
            this.btnTimeLog.Margin = new System.Windows.Forms.Padding(2);
            this.btnTimeLog.Name = "btnTimeLog";
            this.btnTimeLog.Size = new System.Drawing.Size(133, 47);
            this.btnTimeLog.TabIndex = 2;
            this.btnTimeLog.Text = "Log time";
            this.btnTimeLog.UseVisualStyleBackColor = true;
            // 
            // btnLogInfo
            // 
            this.btnLogInfo.Location = new System.Drawing.Point(13, 159);
            this.btnLogInfo.Margin = new System.Windows.Forms.Padding(2);
            this.btnLogInfo.Name = "btnLogInfo";
            this.btnLogInfo.Size = new System.Drawing.Size(128, 47);
            this.btnLogInfo.TabIndex = 1;
            this.btnLogInfo.Text = "LogInfo";
            this.btnLogInfo.UseVisualStyleBackColor = true;
            // 
            // btnSimpleLogger
            // 
            this.btnSimpleLogger.Location = new System.Drawing.Point(13, 14);
            this.btnSimpleLogger.Margin = new System.Windows.Forms.Padding(2);
            this.btnSimpleLogger.Name = "btnSimpleLogger";
            this.btnSimpleLogger.Size = new System.Drawing.Size(113, 47);
            this.btnSimpleLogger.TabIndex = 0;
            this.btnSimpleLogger.Text = "Create simple logger";
            this.btnSimpleLogger.UseVisualStyleBackColor = true;
            // 
            // btnException
            // 
            this.btnException.Location = new System.Drawing.Point(323, 159);
            this.btnException.Name = "btnException";
            this.btnException.Size = new System.Drawing.Size(164, 47);
            this.btnException.TabIndex = 3;
            this.btnException.Text = "Log exception";
            this.btnException.UseVisualStyleBackColor = true;
            // 
            // btnNamedLogger
            // 
            this.btnNamedLogger.Location = new System.Drawing.Point(141, 14);
            this.btnNamedLogger.Name = "btnNamedLogger";
            this.btnNamedLogger.Size = new System.Drawing.Size(128, 47);
            this.btnNamedLogger.TabIndex = 4;
            this.btnNamedLogger.Text = "Create named logger";
            this.btnNamedLogger.UseVisualStyleBackColor = true;
            // 
            // firstNameDataGridViewTextBoxColumn
            // 
            this.firstNameDataGridViewTextBoxColumn.DataPropertyName = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.HeaderText = "FirstName";
            this.firstNameDataGridViewTextBoxColumn.Name = "firstNameDataGridViewTextBoxColumn";
            this.firstNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // lastNameDataGridViewTextBoxColumn
            // 
            this.lastNameDataGridViewTextBoxColumn.DataPropertyName = "LastName";
            this.lastNameDataGridViewTextBoxColumn.HeaderText = "LastName";
            this.lastNameDataGridViewTextBoxColumn.Name = "lastNameDataGridViewTextBoxColumn";
            this.lastNameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // ageDataGridViewTextBoxColumn
            // 
            this.ageDataGridViewTextBoxColumn.DataPropertyName = "Age";
            this.ageDataGridViewTextBoxColumn.HeaderText = "Age";
            this.ageDataGridViewTextBoxColumn.Name = "ageDataGridViewTextBoxColumn";
            this.ageDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // badgeNumberDataGridViewTextBoxColumn
            // 
            this.badgeNumberDataGridViewTextBoxColumn.DataPropertyName = "BadgeNumber";
            this.badgeNumberDataGridViewTextBoxColumn.HeaderText = "BadgeNumber";
            this.badgeNumberDataGridViewTextBoxColumn.Name = "badgeNumberDataGridViewTextBoxColumn";
            this.badgeNumberDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // positionDataGridViewTextBoxColumn
            // 
            this.positionDataGridViewTextBoxColumn.DataPropertyName = "Position";
            this.positionDataGridViewTextBoxColumn.HeaderText = "Position";
            this.positionDataGridViewTextBoxColumn.Name = "positionDataGridViewTextBoxColumn";
            this.positionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // bdsEmployees
            // 
            this.bdsEmployees.DataSource = typeof(Employee);
            // 
            // btnComplexLogger
            // 
            this.btnComplexLogger.Location = new System.Drawing.Point(291, 14);
            this.btnComplexLogger.Name = "btnComplexLogger";
            this.btnComplexLogger.Size = new System.Drawing.Size(140, 47);
            this.btnComplexLogger.TabIndex = 5;
            this.btnComplexLogger.Text = "Create complex logger";
            this.btnComplexLogger.UseVisualStyleBackColor = true;
            // 
            // ExamplesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 769);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ExamplesForm";
            this.Text = "Employees Database and Examples";
            ((System.ComponentModel.ISupportInitialize)(this.grdViewEmployees)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numCreateRandomEmployees)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bdsEmployees)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCreateCashier;
        private System.Windows.Forms.Button btnCreateRandomEmployees;
        private System.Windows.Forms.Button btnCreateClerk;
        private System.Windows.Forms.Button btnCreateAccountant;
        private System.Windows.Forms.Button btnCreateStoreManager;
        private System.Windows.Forms.DataGridView grdViewEmployees;
        private System.Windows.Forms.NumericUpDown numCreateRandomEmployees;
        private System.Windows.Forms.BindingSource bdsEmployees;
        private System.Windows.Forms.DataGridViewTextBoxColumn firstNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn lastNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn ageDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn badgeNumberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnSimpleLogger;
        private System.Windows.Forms.Button btnLogInfo;
        private System.Windows.Forms.Button btnTimeLog;
        private System.Windows.Forms.Button btnException;
        private System.Windows.Forms.Button btnNamedLogger;
        private System.Windows.Forms.Button btnComplexLogger;
    }
}

