using System;
using System.Collections.Generic;
using System.Reflection;
using HarmonyLib;
using Rocket.API;
using Rocket.Core.Commands;
using Rocket.Core.Plugins;

namespace PluginPerformanceProfiler
{
    public class PluginPerformanceProfiler : RocketPlugin
    {
        public Harmony Harmony;
        public static Dictionary<string, CommandAwaiter> Awaiters = new Dictionary<string, CommandAwaiter>();

        public override void LoadPlugin()
        {
            base.LoadPlugin();
            Harmony = new Harmony("PluginPerformanceProfiler");
            MethodInfo EStart = typeof(PluginPerformanceProfiler).GetMethod("OnExcecuteStart", BindingFlags.Public | BindingFlags.Static);
            MethodInfo EEnd = typeof(PluginPerformanceProfiler).GetMethod("OnExcecuteEnd", BindingFlags.Public | BindingFlags.Static);
            MethodInfo E = typeof(RocketCommandManager).GetMethod("Execute", BindingFlags.Instance | BindingFlags.Public);
            var patcher = Harmony.CreateProcessor(E);
            patcher.AddPrefix(EStart);
            patcher.AddPostfix(EEnd);
            patcher.Patch();
            Console.WriteLine("Patched!");
        }

        public static void OnExcecuteStart(IRocketPlayer player, string command)
        {
            CommandAwaiter awaiter = new CommandAwaiter(player, command);
            awaiter.SendStarted();
            awaiter.OnComplete = OnAwaiterFinished;
            lock (Awaiters)
            {
                if (!Awaiters.ContainsKey(player.Id))
                {
                    Awaiters.Add(player.Id, awaiter);
                }
            }
        }

        public static void OnAwaiterFinished(CommandAwaiter awaiter)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[UnturnedPluginProfiler] Command [{awaiter.Command}] took {Math.Round(awaiter.TotalMS, 2)}ms ({awaiter.TotalTicks} ticks), executor: {awaiter.Player.DisplayName}");
            Console.ResetColor();
        }

        public static void OnExcecuteEnd( IRocketPlayer player, string command)
        {
            lock (Awaiters)
            {
                if (Awaiters.ContainsKey(player.Id))
                {
                    CommandAwaiter awaiter = Awaiters[player.Id];
                    Awaiters.Remove(player.Id);
                    awaiter.SendComplete(true);
                }
            }
        }
    }
}