using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using OpticalServer.MQTT;
using Serilog;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Timers;

namespace OpticalServer
{
    public class Program
    {
        #region �رտ��ѡ��
        const int STD_INPUT_HANDLE = -10;
        const uint ENABLE_QUICK_EDIT_MODE = 0x0040;
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern IntPtr GetStdHandle(int hConsoleHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool GetConsoleMode(IntPtr hConsoleHandle, out uint mode);
        [DllImport("kernel32.dll", SetLastError = true)]
        internal static extern bool SetConsoleMode(IntPtr hConsoleHandle, uint mode);
        #endregion

        public static void Main(string[] args)
        {
            #region ��ʼ��LOG��־
            Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .MinimumLevel.Debug()
            .WriteTo.File(Path.Combine(DateTime.Now.ToString("yyyyMM"), $"OpticalServer.txt"),
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: true)
            .CreateLogger();
            #endregion

            #region ͨѶ
            Task.Run(IMTClient.RunAsync);
            #endregion

            #region �����̨
            /// <summary>
            /// ���ó�������E
            /// </summary>
            void InitCW()
            {
                Timer cTimer = new Timer();
                cTimer.Interval = 1000 * 3600;
                cTimer.Elapsed += ConsoleClear;
                //cTimer.AutoReset = false;
                cTimer.Start();
            }
            /// <summary>
            /// �����Լ�
            /// </summary>
            void ConsoleClear(object sender, ElapsedEventArgs e)
            {
                Console.Clear();
                Log.Debug("�������̨");
            }
            InitCW();
            #endregion

            #region ����
            /// <summary>
            /// ���ó�����ƁE
            /// </summary>
            void InitRestart()
            {
                Timer cTimer = new Timer();
                cTimer.Interval = 3000;
                cTimer.Elapsed += Restart;
                //cTimer.AutoReset = false;
                cTimer.Start();
            }
            /// <summary>
            /// �����Լ�
            /// </summary>
            void Restart(object sender, ElapsedEventArgs e)
            {
                //����ʱ���趨
                int h = DateTime.Now.Hour;
                int m = DateTime.Now.Minute;
                int s = DateTime.Now.Second;
                int h1 = 1;//2
                int m1 = 1;//0
                int s1 = 0;
                int s2 = 30;
                if (h == h1 && m == m1 && s >= s1 && s <= s2)
                {
                    Log.Debug("��������" + System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                    // Restart current process Method 1

                    System.Diagnostics.Process.Start(System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();

                }
            }
            InitRestart();
            #endregion

            #region ��ֹ����̨�������
            void DisbleQuickEditMode()
            {
                IntPtr hStdin = GetStdHandle(STD_INPUT_HANDLE);
                uint mode;
                GetConsoleMode(hStdin, out mode);
                mode &= ~ENABLE_QUICK_EDIT_MODE;
                SetConsoleMode(hStdin, mode);

            }
            DisbleQuickEditMode();
            #endregion



            var host = new WebHostBuilder()
                .UseUrls("http://localhost:7776")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();
            host.Run();


            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
