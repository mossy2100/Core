using System.Globalization;

namespace Galaxon.Core.Time;

/// <summary>
/// Extension methods for the DateTime class.
/// </summary>
public static class XDateTime
{
    #region Constants

    /// <summary>
    /// The number of days from the start of the Julian period to the start of the epoch used by
    /// .NET (0001-01-01 00:00:00 UTC).
    /// <see cref="GetTotalDays" />
    /// </summary>
    public const double JulianPeriodOffset = 1721425.5;

    #endregion Constants

    #region Formatting

    /// <summary>
    /// Format the date using ISO format YYYY-MM-DDThh:mm:ss.
    /// This format is useful for databases.
    /// The time zone is not shown. For that, use ToString("u") (UniversalSortableDateTimePattern).
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.datetime.tostring?view=net-7.0" />
    /// </summary>
    /// <param name="date">The DateTime instance.</param>
    /// <returns>A string representing the datetime in ISO format.</returns>
    public static string ToIsoString(this DateTime date)
    {
        DateTimeFormatInfo dtfi = new ();
        return date.ToString(dtfi.SortableDateTimePattern);
    }

    #endregion Formatting

    #region Extract date and time parts

    /// <summary>
    /// Get the date part of a DateTime as a DateOnly object.
    /// An alternative to the Date property, which returns a DateTime.
    /// </summary>
    /// <param name="dt">The DateTime.</param>
    /// <returns>The date part of the DateTime.</returns>
    public static DateOnly GetDateOnly(this DateTime dt) =>
        DateOnly.FromDateTime(dt);

    /// <summary>
    /// Get the time part of a DateTime as a TimeOnly object.
    /// An alternative to the TimeOfDay property, which returns a TimeSpan.
    /// </summary>
    /// <param name="dt">The DateTime.</param>
    /// <returns>The time part of the DateTime.</returns>
    public static TimeOnly GetTimeOnly(this DateTime dt) =>
        TimeOnly.FromTimeSpan(dt.TimeOfDay);

    #endregion Extract date and time parts

    #region Methods for getting the instant as a count of time units

    // The naming convention for these methods is intended to match the TimeSpan
    // properties (e.g. TotalDays, TotalHours, TotalSeconds, etc.)

    /// <summary>
    /// Get the total number of nanoseconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of nanoseconds since the epoch start.</returns>
    public static double GetTotalNanoseconds(this DateTime dt) =>
        dt.Ticks * TimeSpan.NanosecondsPerTick;

    /// <summary>
    /// Get the total number of microseconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of microseconds since the epoch start.</returns>
    public static double GetTotalMicroseconds(this DateTime dt) =>
        (double)dt.Ticks / TimeSpan.TicksPerMicrosecond;

    /// <summary>
    /// Get the total number of milliseconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of milliseconds since the epoch start.</returns>
    public static double GetTotalMilliseconds(this DateTime dt) =>
        (double)dt.Ticks / TimeSpan.TicksPerMillisecond;

    /// <summary>
    /// Get the total number of seconds from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of seconds since the epoch start.</returns>
    public static double GetTotalSeconds(this DateTime dt) =>
        (double)dt.Ticks / TimeSpan.TicksPerSecond;

    /// <summary>
    /// Get the total number of minutes from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of minutes since the epoch start.</returns>
    public static double GetTotalMinutes(this DateTime dt) =>
        (double)dt.Ticks / TimeSpan.TicksPerMinute;

    /// <summary>
    /// Get the total number of hours from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of hours since the epoch start.</returns>
    public static double GetTotalHours(this DateTime dt) =>
        (double)dt.Ticks / TimeSpan.TicksPerHour;

    /// <summary>
    /// Get the total number of days from the start of the epoch to the datetime.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The number of days since the epoch start.</returns>
    public static double GetTotalDays(this DateTime dt) =>
        (double)dt.Ticks / TimeSpan.TicksPerDay;

    #endregion Methods for getting the instant as a count of time units

    #region Create new object from time unit count

    /// <summary>
    /// Create a new DateTime given the number of seconds since the start of the epoch.
    /// </summary>
    /// <param name="seconds">The number of seconds.</param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromTotalSeconds(double seconds)
    {
        long ticks = (long)Math.Round(seconds * TimeSpan.TicksPerSecond);
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    /// <summary>
    /// Create a new DateTime given the number of days since the start of the epoch.
    /// </summary>
    /// <param name="days">
    /// The day count. May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromTotalDays(double days)
    {
        long ticks = (long)Math.Round(days * TimeSpan.TicksPerDay);
        return new DateTime(ticks, DateTimeKind.Utc);
    }

    #endregion Create new object from time unit count

    #region Conversion to/from Julian Day

    /// <summary>
    /// Express the DateTime as a Julian Day.
    /// The time of day information in the DateTime will be expressed as the fractional part of
    /// the return value. Note, however, a Julian Day begins at 12:00 noon.
    /// </summary>
    /// <param name="dt">The DateTime instance.</param>
    /// <returns>The Julian Day value</returns>
    public static double ToJulianDay(this DateTime dt) =>
        JulianPeriodOffset + GetTotalDays(dt);

    /// <summary>
    /// Convert a Julian Day value to a DateTime object.
    /// </summary>
    /// <param name="jd">
    /// The Julian Day value. May include a fractional part indicating the time of day.
    /// </param>
    /// <returns>A new DateTime object.</returns>
    public static DateTime FromJulianDay(double jd) =>
        FromTotalDays(jd - JulianPeriodOffset);

    #endregion Conversion to/from Julian Day
}
