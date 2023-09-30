using System;
using System.Diagnostics;
using UnityEngine;

namespace DDCore
{
    public class Watch : IDisposable
    {
        private readonly string _text;
        private readonly Stopwatch _watch;
        public Watch(string text)
        {
            _text = text;
            _watch = Stopwatch.StartNew();
        }

        public void Dispose()
        {
            _watch.Stop();
            DebugWrapper.Log($"{_text} {_watch.ElapsedMilliseconds}ms");
        }
    }
}