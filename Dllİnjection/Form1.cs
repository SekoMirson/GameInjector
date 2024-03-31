using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using System.Security.Policy;

namespace Dllİnjection
{
    public partial class Form1 : Form
    {
        private Random random = new Random();

        public Form1()
        {
            InitializeComponent();
            LoadRunningProcesses();
            StartColorChange();
        }
        private void LoadRunningProcesses()
        {
            // Task Manager'deki tüm işlemleri al
            Process[] processes = Process.GetProcesses();

            // ComboBox'ı temizle
            comboBox1.Items.Clear();

            // Her bir işlem için
            foreach (Process process in processes)
            {
                // İşlem adını ComboBox'a ekle
                comboBox1.Items.Add(process.ProcessName);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox'ta seçilen öğeyi al
            string selectedProcess = comboBox1.SelectedItem.ToString();

            // Seçilen işlemi bul
            Process[] processes = Process.GetProcessesByName(selectedProcess); 
        }

        private const int SW_SHOWNORMAL = 1;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "DLL (.dll) |*.dll";
            openFileDialog1.ShowDialog();
            string DosyaAdi;
            DosyaAdi = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("\\"));
            string DLl_file = DosyaAdi.Replace("\\", "\\");
            listBox1.Items.Add(openFileDialog1.FileName);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Seçilen işlemi al
            string selectedProcess = comboBox1.SelectedItem.ToString();

            // Seçilen işlem adına sahip bir Process var mı kontrol et
            Process[] processes = Process.GetProcessesByName(selectedProcess);
            if (processes.Length == 0)
            {
                MessageBox.Show("Seçilen işlem bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Process nesnesini seçilen işleme ata
            Process proc = processes[0];

            // Seçilen DLL dosyasını yükle
            foreach (string dllFile in listBox1.Items)
            {
                try
                {
                    // DLL dosyasını seçilen işleme yükle
                    IntPtr hnd = Win32.LoadLibrary(dllFile);
                    if (hnd == IntPtr.Zero)
                    {
                        MessageBox.Show("DLL yüklenirken bir hata oluştu: " + dllFile, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        MessageBox.Show("DLL başarıyla yüklendi: " + dllFile, "Başarı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("DLL yüklenirken bir hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Win32 API'yi çağırmak için gerekli tanımlamalar
        private static class Win32
        {
            [System.Runtime.InteropServices.DllImport("kernel32", SetLastError = true)]
            public static extern IntPtr LoadLibrary(string lpFileName);
        }
        private void StartColorChange()
        {
            // Timer oluştur
            Timer timer = new Timer();
            timer.Interval = 1000; // 1 saniye
            timer.Tick += Timer_Tick;
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Label'ın rengini rastgele değiştir
            label2.ForeColor = GetRandomColor();
        }

        private Color GetRandomColor()
        {
            // Rastgele renk oluştur
            return Color.FromArgb(random.Next(256), random.Next(256), random.Next(256));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Developer: SekoMirson");
            Process.Start("https://instagram.com/officialseko");
        }

        private void label2_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/SekoMirson");
        }
    }
}
