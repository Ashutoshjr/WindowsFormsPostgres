using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsPostgresSilent
{
    public partial class Form1 : Form
    {

        public int currentCounter = 1;
        Process procPotgresInstall = new Process();

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.WorkerSupportsCancellation = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy != true)
            {
                // Start the asynchronous operation.

                Task.Run(() => {

                    InstallPostgres();

                });

                backgroundWorker1.RunWorkerAsync();
            }
        }

        private void InstallPostgres()
        {
            string filePath = @"E:\Postgres\SilentPostgresInstall.bat";
            ProcessStartInfo processPostgres = new ProcessStartInfo();

            processPostgres.CreateNoWindow = true; //This hides the dos-style black window that the command prompt usually shows
            processPostgres.FileName = @"cmd.exe";
            processPostgres.WindowStyle = ProcessWindowStyle.Hidden;
            //psi.UseShellExecute = false;

            processPostgres.Verb = "runas"; //This is what actually runs the command as administrator
            processPostgres.Arguments = "/C " + filePath;
            try
            {
                var process = new Process();
                process.StartInfo = processPostgres;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception ex)
            {
                //If you are here the user clicked decline to grant admin privileges (or he's not administrator)
            }
        }


        //private void UninstallPostgres()
        //{
        //    ProcessStartInfo processPostgres = new ProcessStartInfo();

        //    processPostgres.CreateNoWindow = true; //This hides the dos-style black window that the command prompt usually shows
           
        //    processPostgres.WindowStyle = ProcessWindowStyle.Hidden;
        //    //psi.UseShellExecute = false;
        //    processPostgres.WorkingDirectory = @"C:\Program Files\PostgreSQL\14";
        //    processPostgres.FileName = @"C:\Program Files\PostgreSQL\14\cmd.exe";
        //    processPostgres.Verb = "runas"; //This is what actually runs the command as administrator
        //    processPostgres.Arguments = "/C " + filePath + "uninstall - postgresql.exe--mode unattended";
        //    try
        //    {
        //        var process = new Process();
        //        process.StartInfo = processPostgres;
        //        process.Start();
        //        process.WaitForExit();
        //    }
        //    catch (Exception ex)
        //    {
        //        //If you are here the user clicked decline to grant admin privileges (or he's not administrator)
        //    }
        //}

        private void button2_Click(object sender, EventArgs e)
        {
            if (backgroundWorker2.IsBusy != true)
            {
                // Start the asynchronous operation.

                Task.Run(() => {

                   // UninstallPostgres();

                });

                backgroundWorker2.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            bool IsPostgresInstall = false;

            do
            {
                System.ServiceProcess.ServiceController ctl = System.ServiceProcess.ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "postgreSQL");

                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (ctl != null)
                    {
                        if (ctl.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {

                            IsPostgresInstall = true;
                            currentCounter = progressBar1.Maximum;
                        }
                    }

                    worker.ReportProgress(currentCounter, IsPostgresInstall);
                }


                System.Threading.Thread.Sleep(1000);
                currentCounter++;
            } while (!IsPostgresInstall);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < progressBar1.Maximum && Convert.ToBoolean(e.UserState) == false) // INSTALATION ELASPED TIME GOING AND INSTALION NOT FINISHED
            {
                progressBar1.Value = e.ProgressPercentage;
            }
            else if (e.ProgressPercentage == progressBar1.Maximum && Convert.ToBoolean(e.UserState) == true) // INSTALATION ELASPED TIME FINISHED AND INSTALION FINISHED
            {
                progressBar1.Value = progressBar1.Maximum;
            }
            else if (e.ProgressPercentage == progressBar1.Maximum && Convert.ToBoolean(e.UserState) == false)
            {

                //procPotgresInstall.Kill();
                // procPotgresInstall.Close();
                // procPotgresInstall.Dispose();
                //label1.Text = (e.ProgressPercentage.ToString() + "%");

            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled == true)
            {
                label1.Text = "Canceled!";
            }
            else if (e.Error != null)
            {
                label1.Text = "Error: " + e.Error.Message;
            }
            else
            {
                label1.Text = "Done!";
            }
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            bool IsPostgresUninstall = false;

            do
            {
                System.ServiceProcess.ServiceController ctl = System.ServiceProcess.ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == "postgreSQL");

                if (worker.CancellationPending == true)
                {
                    e.Cancel = true;
                    break;
                }
                else
                {
                    if (ctl != null)
                    {
                        if (ctl.Status == System.ServiceProcess.ServiceControllerStatus.Running)
                        {
                            worker.ReportProgress(currentCounter, IsPostgresUninstall);
                            IsPostgresUninstall = false;
                            
                        }
                    }
                    IsPostgresUninstall = true;
                    currentCounter = progressBar1.Maximum;
                }

                System.Threading.Thread.Sleep(1000);
                currentCounter++;
            } while (!IsPostgresUninstall);
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < progressBar1.Maximum && Convert.ToBoolean(e.UserState) == false) // INSTALATION ELASPED TIME GOING AND INSTALION NOT FINISHED
            {
                progressBar1.Value = e.ProgressPercentage;
            }
            else if (e.ProgressPercentage == progressBar1.Maximum && Convert.ToBoolean(e.UserState) == true) // INSTALATION ELASPED TIME FINISHED AND INSTALION FINISHED
            {
                progressBar1.Value = progressBar1.Maximum;
            }
            else if (e.ProgressPercentage == progressBar1.Maximum && Convert.ToBoolean(e.UserState) == false)
            {

                //procPotgresInstall.Kill();
                // procPotgresInstall.Close();
                // procPotgresInstall.Dispose();
                //label1.Text = (e.ProgressPercentage.ToString() + "%");

            }
        }

        private void backgroundWorker2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
    }
}
