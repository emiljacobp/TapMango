using System;
using System.Collections.Concurrent;
using System.Threading;

namespace TapMango.Services
{
    public class RateLimiter
    {
        private readonly int _maxMessagesPerNumberPerSecond;
        private readonly int _maxMessagesPerAccountPerSecond;
        private readonly ConcurrentDictionary<string, int> _numberMessageCounts;
        private int _accountMessageCount;
        private readonly Timer _resetTimer;

        public RateLimiter(int maxMessagesPerNumberPerSecond, int maxMessagesPerAccountPerSecond)
        {
            _maxMessagesPerNumberPerSecond = maxMessagesPerNumberPerSecond;
            _maxMessagesPerAccountPerSecond = maxMessagesPerAccountPerSecond;
            _numberMessageCounts = new ConcurrentDictionary<string, int>();
            _accountMessageCount = 0;
            _resetTimer = new Timer(ResetCounts, null, 1000, 1000);
        }

        public bool CanSendMessage(string phoneNumber)
        {
            if (_numberMessageCounts.TryGetValue(phoneNumber, out int numberCount) && numberCount >= _maxMessagesPerNumberPerSecond)
            {
                return false;
            }

            if (_accountMessageCount >= _maxMessagesPerAccountPerSecond)
            {
                return false;
            }

            _numberMessageCounts.AddOrUpdate(phoneNumber, 1, (key, count) => count + 1);
            Interlocked.Increment(ref _accountMessageCount);

            return true;
        }

        private void ResetCounts(object state)
        {
            _numberMessageCounts.Clear();
            Interlocked.Exchange(ref _accountMessageCount, 0);
        }
    }
}
