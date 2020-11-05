using System;
using System.Threading.Tasks;
using Rocket.API;

namespace PluginPerformanceProfiler
{
    public class CommandAwaiter
    {
        public IRocketPlayer Player;
        public string Command;
        public OnCompleteHandler OnComplete;
        public bool Result;
        public TaskCompletionSource<bool> Awaiter = new TaskCompletionSource<bool>();
        public delegate void OnCompleteHandler(CommandAwaiter Awaiter);
        public long Started;
        public long Finished;

        public long TotalTicks => Finished - Started;
        public double TotalMS => (((double)Finished - Started) / 10000);

        public void SendStarted()
        {
            Started = DateTime.Now.Ticks;
        }


        public CommandAwaiter(IRocketPlayer player, string command)
        {
            Player = player;
            Command = command;
        }

        public void SendComplete(bool state)
        {
            Finished = DateTime.Now.Ticks;
            Result = state;
            Awaiter.SetResult(state);
            OnComplete?.Invoke(this);
        }

        public async Task<bool> Await()
        {
            return await Awaiter.Task;
        }
    }
}