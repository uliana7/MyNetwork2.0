namespace AForge.WindowsForms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.cmbVideoSource = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StartButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.originalImageBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.processedImgBox = new System.Windows.Forms.PictureBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tresholdTrackBar = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.marginTrackBar = new System.Windows.Forms.TrackBar();
            this.borderTrackBar = new System.Windows.Forms.TrackBar();
            this.statusLabel = new System.Windows.Forms.Label();
            this.ticksLabel = new System.Windows.Forms.Label();
            this.controlPanel = new System.Windows.Forms.Panel();
            this.btnRecognize = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.lblNetwork = new System.Windows.Forms.Label();
            this.cmbNetwork = new System.Windows.Forms.ComboBox();
            this.groupBoxTrain = new System.Windows.Forms.GroupBox();
            this.lblAcceptError = new System.Windows.Forms.Label();
            this.numAcceptError = new System.Windows.Forms.NumericUpDown();
            this.lblMaxPerClass = new System.Windows.Forms.Label();
            this.numMaxPerClass = new System.Windows.Forms.NumericUpDown();
            this.lblSplit = new System.Windows.Forms.Label();
            this.numTrainSplit = new System.Windows.Forms.NumericUpDown();
            this.lblAug = new System.Windows.Forms.Label();
            this.numAug = new System.Windows.Forms.NumericUpDown();
            this.lblEpochs = new System.Windows.Forms.Label();
            this.numEpochs = new System.Windows.Forms.NumericUpDown();
            this.progressTrain = new System.Windows.Forms.ProgressBar();
            this.lblTrainStatus = new System.Windows.Forms.Label();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnTrain = new System.Windows.Forms.Button();
            this.clbClasses = new System.Windows.Forms.CheckedListBox();
            this.lblDatasetInfo = new System.Windows.Forms.Label();
            this.resolutionsBox = new System.Windows.Forms.ComboBox();
            this.btnScanDataset = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.processedImgBox)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tresholdTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.marginTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderTrackBar)).BeginInit();
            this.controlPanel.SuspendLayout();
            this.groupBoxTrain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAcceptError)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxPerClass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrainSplit)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAug)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEpochs)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbVideoSource
            // 
            this.cmbVideoSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmbVideoSource.FormattingEnabled = true;
            this.cmbVideoSource.Location = new System.Drawing.Point(17, 711);
            this.cmbVideoSource.Margin = new System.Windows.Forms.Padding(4);
            this.cmbVideoSource.Name = "cmbVideoSource";
            this.cmbVideoSource.Size = new System.Drawing.Size(291, 24);
            this.cmbVideoSource.TabIndex = 1;
            this.cmbVideoSource.SelectionChangeCommitted += new System.EventHandler(this.cmbVideoSource_SelectionChangeCommitted);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 692);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 16);
            this.label1.TabIndex = 2;
            this.label1.Text = "Выбор камеры";
            // 
            // StartButton
            // 
            this.StartButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.StartButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.StartButton.Location = new System.Drawing.Point(317, 734);
            this.StartButton.Margin = new System.Windows.Forms.Padding(4);
            this.StartButton.Name = "StartButton";
            this.StartButton.Size = new System.Drawing.Size(167, 37);
            this.StartButton.TabIndex = 3;
            this.StartButton.Text = "Старт";
            this.StartButton.UseVisualStyleBackColor = true;
            this.StartButton.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.originalImageBox);
            this.groupBox1.Location = new System.Drawing.Point(1, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(683, 639);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Камера";
            // 
            // originalImageBox
            // 
            this.originalImageBox.Location = new System.Drawing.Point(8, 15);
            this.originalImageBox.Margin = new System.Windows.Forms.Padding(4);
            this.originalImageBox.Name = "originalImageBox";
            this.originalImageBox.Size = new System.Drawing.Size(667, 615);
            this.originalImageBox.TabIndex = 1;
            this.originalImageBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.processedImgBox);
            this.panel1.Location = new System.Drawing.Point(692, 15);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(592, 547);
            this.panel1.TabIndex = 12;
            // 
            // processedImgBox
            // 
            this.processedImgBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.processedImgBox.Location = new System.Drawing.Point(0, 0);
            this.processedImgBox.Margin = new System.Windows.Forms.Padding(4);
            this.processedImgBox.Name = "processedImgBox";
            this.processedImgBox.Size = new System.Drawing.Size(588, 543);
            this.processedImgBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.processedImgBox.TabIndex = 0;
            this.processedImgBox.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.checkBox1);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.tresholdTrackBar);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.marginTrackBar);
            this.panel2.Controls.Add(this.borderTrackBar);
            this.panel2.Location = new System.Drawing.Point(692, 570);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(589, 215);
            this.panel2.TabIndex = 18;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(232, 112);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(108, 20);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Text = "Обработать";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(75, 121);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 16);
            this.label2.TabIndex = 23;
            this.label2.Text = "Порог";
            // 
            // tresholdTrackBar
            // 
            this.tresholdTrackBar.LargeChange = 1;
            this.tresholdTrackBar.Location = new System.Drawing.Point(9, 164);
            this.tresholdTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.tresholdTrackBar.Maximum = 255;
            this.tresholdTrackBar.Name = "tresholdTrackBar";
            this.tresholdTrackBar.Size = new System.Drawing.Size(187, 56);
            this.tresholdTrackBar.TabIndex = 22;
            this.tresholdTrackBar.TickFrequency = 25;
            this.tresholdTrackBar.Value = 120;
            this.tresholdTrackBar.ValueChanged += new System.EventHandler(this.tresholdTrackBar_ValueChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(284, 11);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 16);
            this.label4.TabIndex = 21;
            this.label4.Text = "Зазор";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 11);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(40, 16);
            this.label3.TabIndex = 20;
            this.label3.Text = "Поля";
            // 
            // marginTrackBar
            // 
            this.marginTrackBar.LargeChange = 10;
            this.marginTrackBar.Location = new System.Drawing.Point(216, 38);
            this.marginTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.marginTrackBar.Maximum = 40;
            this.marginTrackBar.Name = "marginTrackBar";
            this.marginTrackBar.Size = new System.Drawing.Size(187, 56);
            this.marginTrackBar.TabIndex = 19;
            this.marginTrackBar.TickFrequency = 4;
            this.marginTrackBar.Value = 10;
            this.marginTrackBar.ValueChanged += new System.EventHandler(this.marginTrackBar_ValueChanged);
            // 
            // borderTrackBar
            // 
            this.borderTrackBar.LargeChange = 60;
            this.borderTrackBar.Location = new System.Drawing.Point(9, 38);
            this.borderTrackBar.Margin = new System.Windows.Forms.Padding(4);
            this.borderTrackBar.Maximum = 160;
            this.borderTrackBar.Name = "borderTrackBar";
            this.borderTrackBar.Size = new System.Drawing.Size(187, 56);
            this.borderTrackBar.TabIndex = 18;
            this.borderTrackBar.TickFrequency = 10;
            this.borderTrackBar.Value = 40;
            this.borderTrackBar.ValueChanged += new System.EventHandler(this.borderTrackBar_ValueChanged);
            // 
            // statusLabel
            // 
            this.statusLabel.AutoSize = true;
            this.statusLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F);
            this.statusLabel.Location = new System.Drawing.Point(13, 650);
            this.statusLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(96, 29);
            this.statusLabel.TabIndex = 24;
            this.statusLabel.Text = "Статус:";
            // 
            // ticksLabel
            // 
            this.ticksLabel.AutoSize = true;
            this.ticksLabel.Location = new System.Drawing.Point(501, 716);
            this.ticksLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ticksLabel.Name = "ticksLabel";
            this.ticksLabel.Size = new System.Drawing.Size(165, 16);
            this.ticksLabel.TabIndex = 30;
            this.ticksLabel.Text = "Ticks for frame processing";
            // 
            // controlPanel
            // 
            this.controlPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controlPanel.Controls.Add(this.btnRecognize);
            this.controlPanel.Controls.Add(this.txtResult);
            this.controlPanel.Controls.Add(this.lblNetwork);
            this.controlPanel.Controls.Add(this.cmbNetwork);
            this.controlPanel.Location = new System.Drawing.Point(693, 799);
            this.controlPanel.Margin = new System.Windows.Forms.Padding(4);
            this.controlPanel.Name = "controlPanel";
            this.controlPanel.Size = new System.Drawing.Size(591, 114);
            this.controlPanel.TabIndex = 33;
            // 
            // btnRecognize
            // 
            this.btnRecognize.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold);
            this.btnRecognize.Location = new System.Drawing.Point(16, 63);
            this.btnRecognize.Margin = new System.Windows.Forms.Padding(4);
            this.btnRecognize.Name = "btnRecognize";
            this.btnRecognize.Size = new System.Drawing.Size(130, 37);
            this.btnRecognize.TabIndex = 0;
            this.btnRecognize.Text = "Распознать";
            this.btnRecognize.UseVisualStyleBackColor = true;
            this.btnRecognize.Click += new System.EventHandler(this.btnRecognize_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(154, 63);
            this.txtResult.Margin = new System.Windows.Forms.Padding(4);
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(415, 22);
            this.txtResult.TabIndex = 1;
            this.txtResult.Text = "Это символ: (нет)";
            // 
            // lblNetwork
            // 
            this.lblNetwork.AutoSize = true;
            this.lblNetwork.Location = new System.Drawing.Point(13, 12);
            this.lblNetwork.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblNetwork.Name = "lblNetwork";
            this.lblNetwork.Size = new System.Drawing.Size(41, 16);
            this.lblNetwork.TabIndex = 100;
            this.lblNetwork.Text = "Сеть:";
            // 
            // cmbNetwork
            // 
            this.cmbNetwork.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNetwork.FormattingEnabled = true;
            this.cmbNetwork.Location = new System.Drawing.Point(80, 9);
            this.cmbNetwork.Margin = new System.Windows.Forms.Padding(4);
            this.cmbNetwork.Name = "cmbNetwork";
            this.cmbNetwork.Size = new System.Drawing.Size(239, 24);
            this.cmbNetwork.TabIndex = 101;
            this.cmbNetwork.SelectedIndexChanged += new System.EventHandler(this.cmbNetwork_SelectedIndexChanged);
            // 
            // groupBoxTrain
            // 
            this.groupBoxTrain.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxTrain.Controls.Add(this.lblAcceptError);
            this.groupBoxTrain.Controls.Add(this.numAcceptError);
            this.groupBoxTrain.Controls.Add(this.lblMaxPerClass);
            this.groupBoxTrain.Controls.Add(this.numMaxPerClass);
            this.groupBoxTrain.Controls.Add(this.lblSplit);
            this.groupBoxTrain.Controls.Add(this.numTrainSplit);
            this.groupBoxTrain.Controls.Add(this.lblAug);
            this.groupBoxTrain.Controls.Add(this.numAug);
            this.groupBoxTrain.Controls.Add(this.lblEpochs);
            this.groupBoxTrain.Controls.Add(this.numEpochs);
            this.groupBoxTrain.Controls.Add(this.progressTrain);
            this.groupBoxTrain.Controls.Add(this.lblTrainStatus);
            this.groupBoxTrain.Controls.Add(this.btnTest);
            this.groupBoxTrain.Controls.Add(this.btnTrain);
            this.groupBoxTrain.Controls.Add(this.btnScanDataset);
            this.groupBoxTrain.Controls.Add(this.clbClasses);
            this.groupBoxTrain.Controls.Add(this.lblDatasetInfo);
            this.groupBoxTrain.Location = new System.Drawing.Point(693, 430);
            this.groupBoxTrain.Margin = new System.Windows.Forms.Padding(4);
            this.groupBoxTrain.Name = "groupBoxTrain";
            this.groupBoxTrain.Padding = new System.Windows.Forms.Padding(4);
            this.groupBoxTrain.Size = new System.Drawing.Size(591, 361);
            this.groupBoxTrain.TabIndex = 50;
            this.groupBoxTrain.TabStop = false;
            this.groupBoxTrain.Text = "Обучение сети";
            // 
            // lblAcceptError
            // 
            this.lblAcceptError.AutoSize = true;
            this.lblAcceptError.Location = new System.Drawing.Point(270, 210);
            this.lblAcceptError.Name = "lblAcceptError";
            this.lblAcceptError.Size = new System.Drawing.Size(80, 16);
            this.lblAcceptError.TabIndex = 13;
            this.lblAcceptError.Text = "Accept error";
            // 
            // numAcceptError
            // 
            this.numAcceptError.DecimalPlaces = 3;
            this.numAcceptError.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numAcceptError.Location = new System.Drawing.Point(390, 208);
            this.numAcceptError.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numAcceptError.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numAcceptError.Name = "numAcceptError";
            this.numAcceptError.Size = new System.Drawing.Size(80, 22);
            this.numAcceptError.TabIndex = 14;
            this.numAcceptError.Value = new decimal(new int[] {
            20,
            0,
            0,
            196608});
            // 
            // lblMaxPerClass
            // 
            this.lblMaxPerClass.AutoSize = true;
            this.lblMaxPerClass.Location = new System.Drawing.Point(10, 294);
            this.lblMaxPerClass.Name = "lblMaxPerClass";
            this.lblMaxPerClass.Size = new System.Drawing.Size(121, 16);
            this.lblMaxPerClass.TabIndex = 11;
            this.lblMaxPerClass.Text = "Лимит/класс (0=∞)";
            this.lblMaxPerClass.Visible = false;
            // 
            // numMaxPerClass
            // 
            this.numMaxPerClass.Location = new System.Drawing.Point(140, 292);
            this.numMaxPerClass.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMaxPerClass.Name = "numMaxPerClass";
            this.numMaxPerClass.Size = new System.Drawing.Size(80, 22);
            this.numMaxPerClass.TabIndex = 12;
            this.numMaxPerClass.Visible = false;
            // 
            // lblSplit
            // 
            this.lblSplit.AutoSize = true;
            this.lblSplit.Location = new System.Drawing.Point(10, 266);
            this.lblSplit.Name = "lblSplit";
            this.lblSplit.Size = new System.Drawing.Size(88, 16);
            this.lblSplit.TabIndex = 9;
            this.lblSplit.Text = "Train split (%)";
            // 
            // numTrainSplit
            // 
            this.numTrainSplit.Location = new System.Drawing.Point(140, 264);
            this.numTrainSplit.Maximum = new decimal(new int[] {
            95,
            0,
            0,
            0});
            this.numTrainSplit.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numTrainSplit.Name = "numTrainSplit";
            this.numTrainSplit.Size = new System.Drawing.Size(80, 22);
            this.numTrainSplit.TabIndex = 10;
            this.numTrainSplit.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // lblAug
            // 
            this.lblAug.AutoSize = true;
            this.lblAug.Location = new System.Drawing.Point(10, 238);
            this.lblAug.Name = "lblAug";
            this.lblAug.Size = new System.Drawing.Size(122, 16);
            this.lblAug.TabIndex = 7;
            this.lblAug.Text = "Аугментаций/изо";
            this.lblAug.Visible = false;
            // 
            // numAug
            // 
            this.numAug.Location = new System.Drawing.Point(140, 236);
            this.numAug.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numAug.Name = "numAug";
            this.numAug.Size = new System.Drawing.Size(80, 22);
            this.numAug.TabIndex = 8;
            this.numAug.Value = new decimal(new int[] {
            14,
            0,
            0,
            0});
            this.numAug.Visible = false;
            // 
            // lblEpochs
            // 
            this.lblEpochs.AutoSize = true;
            this.lblEpochs.Location = new System.Drawing.Point(10, 210);
            this.lblEpochs.Name = "lblEpochs";
            this.lblEpochs.Size = new System.Drawing.Size(47, 16);
            this.lblEpochs.TabIndex = 5;
            this.lblEpochs.Text = "Эпохи";
            // 
            // numEpochs
            // 
            this.numEpochs.Location = new System.Drawing.Point(140, 208);
            this.numEpochs.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.numEpochs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEpochs.Name = "numEpochs";
            this.numEpochs.Size = new System.Drawing.Size(80, 22);
            this.numEpochs.TabIndex = 6;
            this.numEpochs.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // progressTrain
            // 
            this.progressTrain.Location = new System.Drawing.Point(10, 340);
            this.progressTrain.Name = "progressTrain";
            this.progressTrain.Size = new System.Drawing.Size(570, 14);
            this.progressTrain.TabIndex = 16;
            // 
            // lblTrainStatus
            // 
            this.lblTrainStatus.Location = new System.Drawing.Point(10, 320);
            this.lblTrainStatus.Name = "lblTrainStatus";
            this.lblTrainStatus.Size = new System.Drawing.Size(570, 18);
            this.lblTrainStatus.TabIndex = 15;
            this.lblTrainStatus.Text = "Статус обучения: (нет)";
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(270, 173);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(150, 28);
            this.btnTest.TabIndex = 4;
            this.btnTest.Text = "Тест";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnTrain
            // 
            this.btnTrain.Location = new System.Drawing.Point(270, 139);
            this.btnTrain.Name = "btnTrain";
            this.btnTrain.Size = new System.Drawing.Size(150, 28);
            this.btnTrain.TabIndex = 3;
            this.btnTrain.Text = "Обучить";
            this.btnTrain.UseVisualStyleBackColor = true;
            this.btnTrain.Click += new System.EventHandler(this.btnTrain_Click);
            // 
            // clbClasses
            // 
            this.clbClasses.CheckOnClick = true;
            this.clbClasses.FormattingEnabled = true;
            this.clbClasses.Location = new System.Drawing.Point(13, 105);
            this.clbClasses.Name = "clbClasses";
            this.clbClasses.Size = new System.Drawing.Size(240, 89);
            this.clbClasses.TabIndex = 1;
            this.clbClasses.Visible = false;
            // 
            // lblDatasetInfo
            // 
            this.lblDatasetInfo.Location = new System.Drawing.Point(10, 22);
            this.lblDatasetInfo.Name = "lblDatasetInfo";
            this.lblDatasetInfo.Size = new System.Drawing.Size(570, 80);
            this.lblDatasetInfo.TabIndex = 0;
            this.lblDatasetInfo.Text = "Dataset: (не загружен)";
            // 
            // resolutionsBox
            // 
            this.resolutionsBox.AllowDrop = true;
            this.resolutionsBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.resolutionsBox.FormattingEnabled = true;
            this.resolutionsBox.Location = new System.Drawing.Point(19, 745);
            this.resolutionsBox.Margin = new System.Windows.Forms.Padding(4);
            this.resolutionsBox.Name = "resolutionsBox";
            this.resolutionsBox.Size = new System.Drawing.Size(289, 24);
            this.resolutionsBox.TabIndex = 34;
            // 
            // btnScanDataset
            // 
            this.btnScanDataset.Location = new System.Drawing.Point(270, 105);
            this.btnScanDataset.Name = "btnScanDataset";
            this.btnScanDataset.Size = new System.Drawing.Size(150, 28);
            this.btnScanDataset.TabIndex = 2;
            this.btnScanDataset.Text = "Сканировать";
            this.btnScanDataset.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1297, 913);
            this.Controls.Add(this.resolutionsBox);
            this.Controls.Add(this.groupBoxTrain);
            this.Controls.Add(this.controlPanel);
            this.Controls.Add(this.ticksLabel);
            this.Controls.Add(this.statusLabel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.StartButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbVideoSource);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MainForm";
            this.Text = "Распознавалка";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.originalImageBox)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.processedImgBox)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tresholdTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.marginTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.borderTrackBar)).EndInit();
            this.controlPanel.ResumeLayout(false);
            this.controlPanel.PerformLayout();
            this.groupBoxTrain.ResumeLayout(false);
            this.groupBoxTrain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAcceptError)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxPerClass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTrainSplit)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numAug)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEpochs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbVideoSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button StartButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.PictureBox originalImageBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox processedImgBox;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TrackBar tresholdTrackBar;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TrackBar marginTrackBar;
        private System.Windows.Forms.TrackBar borderTrackBar;
        private System.Windows.Forms.Label statusLabel;
        private System.Windows.Forms.Label ticksLabel;
        private System.Windows.Forms.Panel controlPanel;
        private System.Windows.Forms.Button btnRecognize;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.ComboBox resolutionsBox;
        private System.Windows.Forms.ComboBox cmbNetwork;
        private System.Windows.Forms.Label lblNetwork;

        private System.Windows.Forms.GroupBox groupBoxTrain;
        private System.Windows.Forms.CheckedListBox clbClasses;
        private System.Windows.Forms.Button btnTrain;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Label lblDatasetInfo;
        private System.Windows.Forms.Label lblTrainStatus;
        private System.Windows.Forms.ProgressBar progressTrain;
        private System.Windows.Forms.NumericUpDown numEpochs;
        private System.Windows.Forms.NumericUpDown numAug;
        private System.Windows.Forms.NumericUpDown numTrainSplit;
        private System.Windows.Forms.NumericUpDown numMaxPerClass;
        private System.Windows.Forms.NumericUpDown numAcceptError;
        private System.Windows.Forms.Label lblEpochs;
        private System.Windows.Forms.Label lblAug;
        private System.Windows.Forms.Label lblSplit;
        private System.Windows.Forms.Label lblMaxPerClass;
        private System.Windows.Forms.Label lblAcceptError;
        private System.Windows.Forms.Button btnScanDataset;
    }
}
