using System;
using System.Drawing;
using System.Windows.Forms;

namespace MissionPlanner.Controls
{
    public partial class ServoStatus : Form
    {
        // 舵机数量，根据实际情况调整
        private const int MaxServos = 8;

        // 行索引常量
        private const int RowTitle = 0;
        private const int RowVoltage = 1;
        private const int RowCurrent = 2;
        private const int RowPower = 3;

        // 存储每个舵机的数据标签，方便定时器更新
        private Label[] lblVoltage;
        private Label[] lblCurrent;
        private Label[] lblPower;

        public ServoStatus()
        {
            InitializeComponent();

            Utilities.ThemeManager.ApplyThemeTo(this);

            BuildTableLayout();

            timer1.Start();
        }

        /// <summary>
        /// 动态构建表格布局
        /// 行0: 舵机编号 (ESC1, ESC2, ...)
        /// 行1: 电压
        /// 行2: 电流
        /// 行3: 功率
        /// </summary>
        private void BuildTableLayout()
        {
            var table = new TableLayoutPanel();
            table.Dock = DockStyle.Fill;
            table.ColumnCount = MaxServos;
            table.RowCount = 4;

            // 设置列宽均分
            for (int i = 0; i < MaxServos; i++)
                table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / MaxServos));

            // 设置行高
            table.RowStyles.Add(new RowStyle(SizeType.Absolute, 30)); // 标题行
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 33));
            table.RowStyles.Add(new RowStyle(SizeType.Percent, 34));

            // 行标签（最左列描述）
            // 这里不需要，因为我们把描述放在行首

            // 初始化数据标签数组
            lblVoltage = new Label[MaxServos];
            lblCurrent = new Label[MaxServos];
            lblPower = new Label[MaxServos];

            for (int i = 0; i < MaxServos; i++)
            {
                // 行0: 舵机编号标题
                var lblTitle = new Label
                {
                    Text = $"SERVO{i + 1}",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    Font = new Font(this.Font, FontStyle.Bold),
                    ForeColor = Color.Cyan
                };
                table.Controls.Add(lblTitle, i, RowTitle);

                // 行1: 电压
                lblVoltage[i] = new Label
                {
                    Text = "--",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.Yellow
                };
                table.Controls.Add(lblVoltage[i], i, RowVoltage);

                // 行2: 电流
                lblCurrent[i] = new Label
                {
                    Text = "--",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.LimeGreen
                };
                table.Controls.Add(lblCurrent[i], i, RowCurrent);

                // 行3: 功率
                lblPower[i] = new Label
                {
                    Text = "--",
                    TextAlign = ContentAlignment.MiddleCenter,
                    Dock = DockStyle.Fill,
                    ForeColor = Color.Orange
                };
                table.Controls.Add(lblPower[i], i, RowPower);
            }

            this.Controls.Add(table);
        }

        /// <summary>
        /// 定时器触发，更新舵机状态显示
        /// </summary>
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!MainV2.comPort.BaseStream.IsOpen)
                return;

            UpdateData();
        }

        /// <summary>
        /// 更新舵机数据到界面
        /// </summary>
        private void UpdateData()
        {
            var cs = MainV2.comPort.MAV.cs;

            UpdateServo(0, cs.servo1_volt, cs.servo1_curr, cs.servo1_power);
            UpdateServo(1, cs.servo2_volt, cs.servo2_curr, cs.servo2_power);
            UpdateServo(2, cs.servo3_volt, cs.servo3_curr, cs.servo3_power);
            UpdateServo(3, cs.servo4_volt, cs.servo4_curr, cs.servo4_power);
            UpdateServo(4, cs.servo5_volt, cs.servo5_curr, cs.servo5_power);
            UpdateServo(5, cs.servo6_volt, cs.servo6_curr, cs.servo6_power);
            UpdateServo(6, cs.servo7_volt, cs.servo7_curr, cs.servo7_power);
            UpdateServo(7, cs.servo8_volt, cs.servo8_curr, cs.servo8_power);
        }

        /// <summary>
        /// 更新单个舵机的显示数据
        /// </summary>
        /// <param name="index">舵机索引 (0-based)</param>
        /// <param name="voltage">电压 (V)</param>
        /// <param name="current">电流 (A)</param>
        /// <param name="power">功率 (W)</param>
        private void UpdateServo(int index, float voltage, float current, float power)
        {
            if (index < 0 || index >= MaxServos)
                return;

            lblVoltage[index].Text = $"{voltage:F1}V";
            lblCurrent[index].Text = $"{current:F2}A";
            lblPower[index].Text = $"{power:F0}W";

            // 电压低于阈值时标红警告
            if (voltage > 0 && voltage < 10.0f)
                lblVoltage[index].ForeColor = Color.Red;
            else if (voltage > 0)
                lblVoltage[index].ForeColor = Color.Yellow;

            // 功率过高警告
            if (power > 200)
                lblPower[index].ForeColor = Color.Red;
            else
                lblPower[index].ForeColor = Color.Orange;
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            timer1.Stop();
            base.OnFormClosing(e);
        }
    }
}
