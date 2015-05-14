using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using System.Threading;
using System.Timers;
using System.Runtime.InteropServices;


using Color = System.Drawing.Color;

using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace TimerTester
{
    class TimerTester
    {
        /*[DllImport("ntdll.dll", EntryPoint = "NtSetTimerResolution")]
        public static extern void NtSetTimerResolution(uint DesiredResolution, bool SetResolution, ref uint CurrentResolution);*/

        private static DateTime assemblyLoadTime = DateTime.Now;
        private static Stopwatch stopWatch = Stopwatch.StartNew();

        private static float lastGameTimerStart = 0;
        private static float lastTickCountTimerStart = 0;
        private static float lastWatchTimerStart = 0;

        private static float lastGameTimerTick = 0;
        private static float lastTickCountTimerTick = 0;
        private static float lastWatchTimerTick = 0;

        private static float lastGameTimerFreq = 0;
        private static float lastTickCountTimerFreq = 0;
        private static float lastWatchTimerFreq = 0;

        private static float getWatchTimer { get { return stopWatch.ElapsedMilliseconds; } }
        private static float getGameTimer { get { return Game.Time * 1000; } }
        private static float getTickCountTimer { get { return (int)DateTime.Now.Subtract(assemblyLoadTime).TotalMilliseconds; } }

        private static System.Timers.Timer timer;

        public TimerTester()
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
            //OnStart();

            //Console.ReadLine();
        }

        private void OnStart()
        {
            lastGameTimerStart = getGameTimer;
            lastTickCountTimerStart = getTickCountTimer;
            lastWatchTimerStart = getWatchTimer;

            //SetHighResolution();

            timer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;
        }

        private void Game_OnGameLoad(EventArgs args)
        {
            Drawing.OnDraw += Drawing_OnDraw;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Hero.OnProcessSpellCast += Game_ProcessSpell;

            lastGameTimerStart = getGameTimer;
            lastTickCountTimerStart = getTickCountTimer;
            lastWatchTimerStart = getWatchTimer;

            //SetHighResolution();

            /*timer = new System.Timers.Timer(50);
            // Hook up the Elapsed event for the timer. 
            timer.Elapsed += OnTimedEvent;
            timer.Enabled = true;*/

            //UpdateTicks();
        }

        private void Game_ProcessSpell(Obj_AI_Base hero, GameObjectProcessSpellCastEventArgs args)
        {
            UpdateTicks();
            Thread.Sleep(16);

            /*for (int i = 0; i < 9999999; i++)
            {
                Math.Sqrt(Math.PI);
            }*/

            OnEvent();
        }

        private void SetHighResolution()
        {
            uint DesiredResolution = 9000;
            bool SetResolution = false;
            uint CurrentResolution = 0;

            //NtSetTimerResolution(DesiredResolution, SetResolution, ref CurrentResolution);

            Console.WriteLine("Current Timer Resolution: " + CurrentResolution);
        }

        private static void OnEvent()
        {
            Console.WriteLine("Timer1 Freq: " + (getGameTimer - lastGameTimerTick));
            Console.WriteLine("Timer2 Freq: " + (getTickCountTimer - lastTickCountTimerTick));
            Console.WriteLine("Timer3 Freq: " + (getWatchTimer - lastWatchTimerTick));
            Console.WriteLine("");
            UpdateTicks();
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            OnEvent();
        }

        private void Game_OnUpdate(EventArgs args)
        {
            //if (getWatchTimer - lastWatchTimerTick > 50)
            OnEvent();
            
        }

        private void Drawing_OnDraw(EventArgs args)
        {
            PrintTimers();
            //UpdateTicks();
            //Console.WriteLine((int)DateTime.Now.Subtract(assemblyLoadTime).TotalMilliseconds);
        }

        private static void UpdateTicks()
        {
            lastGameTimerFreq = getGameTimer - lastGameTimerTick;
            lastTickCountTimerFreq = getTickCountTimer - lastTickCountTimerTick;
            lastWatchTimerFreq = getWatchTimer - lastWatchTimerTick;

            lastGameTimerTick = getGameTimer;
            lastTickCountTimerTick = getTickCountTimer;
            lastWatchTimerTick = getWatchTimer;

            //Utility.DelayAction.Add(1000, () => UpdateTicks());
        }

        private void PrintTimers()
        {
            /*Drawing.DrawText(10, 10, Color.White, "Timer1 Freq: " + (getGameTimer - lastGameTimerTick));
            Drawing.DrawText(10, 30, Color.White, "Timer2 Freq: " + (getTickCountTimer - lastTickCountTimerTick));
            Drawing.DrawText(10, 50, Color.White, "Timer3 Freq: " + (getWatchTimer - lastWatchTimerTick));//(getWatchTimer - lastWatchTimerTick));*/

            Drawing.DrawText(10, 10, Color.White, "Timer1 Freq: " + lastGameTimerFreq);
            Drawing.DrawText(10, 30, Color.White, "Timer2 Freq: " + lastTickCountTimerFreq);
            Drawing.DrawText(10, 50, Color.White, "Timer3 Freq: " + lastWatchTimerFreq);//(getWatchTimer - lastWatchTimerTick));

            Drawing.DrawText(10, 70, Color.White, "Timer1 Total: " + (getGameTimer - lastGameTimerStart));
            Drawing.DrawText(10, 90, Color.White, "Timer2 Total: " + (getTickCountTimer - lastTickCountTimerStart));
            Drawing.DrawText(10, 110, Color.White, "Timer3 Total: " + (getWatchTimer - lastWatchTimerStart));

        }
    }
}
