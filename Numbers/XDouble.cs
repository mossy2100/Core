namespace Galaxon.Core.Numbers;

/// <summary>
/// Extension methods for Double.
/// </summary>
public static class XDouble
{
    /// <summary>
    /// The default maximum difference between 2 double values being compared for equality.
    /// </summary>
    public const double Delta = 1e-9;

    #region Miscellaneous methods

    /// <summary>
    /// Round off a value to a given number of significant figures.
    /// </summary>
    /// <param name="d">The number to round.</param>
    /// <param name="nSigFigs">The number of significant figures.</param>
    /// <returns>The rounded number.</returns>
    public static double RoundSigFigs(double d, int nSigFigs)
    {
        if (d <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(nSigFigs), "Must be positive.");
        }

        // The maximum number of significant digits supported by double is 17, so if it's this or
        // greater there's nothing to do.
        if (d >= 17)
        {
            return d;
        }

        double scale = Pow(10, Floor(Log10(d)) + 1);
        return scale * Round(d / scale, nSigFigs);
    }

    #endregion Miscellaneous methods

    #region Methods for checking doubles as integers

    /// <summary>
    /// Check if a double is a positive integer.
    /// </summary>
    public static bool IsPositiveInteger(double d) =>
        (d > 0) && double.IsInteger(d);

    /// <summary>
    /// Check if a double is a negative integer.
    /// </summary>
    public static bool IsNegativeInteger(double d) =>
        (d < 0) && double.IsInteger(d);

    /// <summary>
    /// Check if a value is a perfect square.
    /// </summary>
    public static bool IsPerfectSquare(double d) =>
        double.IsPositive(d) && double.IsInteger(Sqrt(d));

    #endregion Methods for checking doubles as integers

    #region Methods for fuzzy equals

    /// <summary>
    /// Check if 2 double values are equal for practical purposes.
    ///
    /// If two double values differ only by the least significant bit, this is more likely
    /// due to inaccuracies in floating point representations than actual inequality.
    ///
    /// This code is copied/adapted from Google Guava DoubleMath.fuzzyEquals().
    /// <see href="https://github.com/google/guava/blob/master/guava/src/com/google/common/math/DoubleMath.java#L360" />
    ///
    /// I initially tried the algorithm from the Microsoft documentation, it didn't work in all cases.
    /// <see href="https://learn.microsoft.com/en-us/dotnet/api/system.double.equals?view=net-7.0#system-double-equals(system-double)" />
    /// </summary>
    /// <param name="a">First number.</param>
    /// <param name="b">Second number.</param>
    /// <param name="tolerance">The maximum allowable difference between them.</param>
    /// <returns>If close enough to equal.</returns>
    public static bool FuzzyEquals(this double a, double b, double tolerance = Delta)
    {
        // Ensure tolerance is non-negative.
        if (tolerance < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(tolerance), "Cannot be negative.");
        }

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        // Handle NaN separately so the method behaves the same as double.Equals().
        // The equality operator will return false when comparing 2 NaN values.
        // The equality operator will return true when comparing infinities.
        return (double.IsNaN(a) && double.IsNaN(b)) || (a == b) || Abs(a - b) <= tolerance;
    }

    /// <summary>
    /// Compare two nullable doubles for fuzzy equality.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="tolerance"></param>
    /// <returns></returns>
    public static bool FuzzyEquals(this double? a, double? b, double tolerance = Delta)
    {
        // If they're both null then they're equal.
        if (!a.HasValue && !b.HasValue)
        {
            return true;
        }

        // If only one of them is null they're unequal.
        if (!a.HasValue || !b.HasValue)
        {
            return false;
        }

        // Check for fuzzy equality.
        return a.Value.FuzzyEquals(b.Value, tolerance);
    }

    /// <summary>
    /// IsInteger() can be a bit strict. This method allows for some fuzziness.
    /// </summary>
    public static bool FuzzyIsInteger(double d, double tolerance = Delta) =>
        d.FuzzyEquals(Round(d), tolerance);

    /// <summary>
    /// Check if a double is a positive integer, with some fuzziness.
    /// </summary>
    public static bool FuzzyIsPositiveInteger(double d, double tolerance = Delta) =>
        (d > 0) && FuzzyIsInteger(d, tolerance);

    /// <summary>
    /// Check if a double is a negative integer, with some fuzziness.
    /// </summary>
    public static bool FuzzyIsNegativeInteger(double d, double tolerance = Delta) =>
        (d < 0) && FuzzyIsInteger(d, tolerance);

    #endregion Methods for fuzzy equals
}
