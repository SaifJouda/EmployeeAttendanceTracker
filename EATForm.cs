using System;
using System.Runtime.InteropServices;
namespace EmployeeAttendanceTracker;
using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
public partial class EATForm : Form
{
    [DllImport("user32.dll")]
        static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);
        private Label statusLabel= new Label();
        private Label timeLabel= new Label();
        private Button saveButton = new Button();
        private int totalActiveTime = 0;
        private int totalInactiveTime = 0;


        public EATForm()
        {
            InitializeComponent();

            statusLabel.Text = "Status: ";
            statusLabel.Location = new System.Drawing.Point(10, 10);
            statusLabel.AutoSize = true;
            Controls.Add(statusLabel);

            timeLabel.Text = "Time: ";
            timeLabel.Location = new System.Drawing.Point(10, 40);
            timeLabel.AutoSize = true;
            Controls.Add(timeLabel);

            saveButton.Text = "Save";
            saveButton.Location = new System.Drawing.Point(10, 70);
            saveButton.Click += SaveButton_Click;
            Controls.Add(saveButton);


            Load += MyForm_Load;
        }

        private void MyForm_Load(object? sender, EventArgs e)
        {
            Timer timer = new Timer();
            timer.Tick += Timer_Tick;
            timer.Interval = 1000; // Delay between activity checks (in milliseconds)
            timer.Start();
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            LASTINPUTINFO lastInputInfo = new LASTINPUTINFO();
            lastInputInfo.cbSize = Marshal.SizeOf(lastInputInfo);

            if (GetLastInputInfo(ref lastInputInfo))
            {
                int idleTime = Environment.TickCount - lastInputInfo.dwTime;

                if (idleTime >= 10000) // Adjust the idle time threshold as needed (in milliseconds)
                {
                    statusLabel.Text = "User is inactive.";
                    totalInactiveTime += 1;
                }
                else
                {
                    statusLabel.Text = "User is active.";
                    totalActiveTime += 1;
                }
                timeLabel.Text = (Environment.TickCount - lastInputInfo.dwTime).ToString();
            }
            else
            {
                statusLabel.Text = "Failed to get last input information.";
            }
        }
        private void SaveButton_Click(object? sender, EventArgs e)
        {
            string filePath = "timelog.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, true))
                {
                    writer.WriteLine("Active Time: " + totalActiveTime);
                    writer.WriteLine("Inactive Time: " + totalInactiveTime);
                    writer.WriteLine();
                }

                MessageBox.Show("Time log saved to file successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the time log: " + ex.Message);
            }
            
        }
}
struct LASTINPUTINFO
{
    public int cbSize;
    public int dwTime;
}
