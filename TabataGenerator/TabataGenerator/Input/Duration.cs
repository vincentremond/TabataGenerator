using System;
using System.Diagnostics;
using System.Globalization;

namespace TabataGenerator.Input
{
    [DebuggerDisplay("Duration {_timeSpan}")]
    public struct Duration
    {
        private TimeSpan _timeSpan;

        private Duration(TimeSpan timeSpan)
        {
            if (timeSpan.Ticks < 0)
            {
                throw new ArgumentException("Duration should be positive", nameof(timeSpan));
            }

            _timeSpan = timeSpan;
        }

        public Duration(int minutes, int seconds)
            : this(new TimeSpan(hours: 0, minutes, seconds))
        {
        }

        public static Duration Empty => new(TimeSpan.Zero);
        public int TotalSeconds => (int)_timeSpan.TotalSeconds;

        public static implicit operator Duration(TimeSpan timeSpan) => new Duration(timeSpan);

        public static implicit operator Duration(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return Duration.Empty;
            }

            var success =
                    TimeSpan.TryParseExact(value, "s\\s", CultureInfo.InvariantCulture, out var result)
                    || TimeSpan.TryParseExact(value, "m\\m", CultureInfo.InvariantCulture, out result)
                    || TimeSpan.TryParseExact(value, "m\\ms\\s", CultureInfo.InvariantCulture, out result)
                ;

            if (!success)
            {
                throw new Exception($"Failed to parse timespan {value}");
            }

            return new Duration(result);
        }

        public static Duration FromSeconds(int seconds)
            => new Duration(0, seconds);
    }
}
