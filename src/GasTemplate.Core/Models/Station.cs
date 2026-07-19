using System.Globalization;

namespace GasTemplate.Core.Models;

/// <summary>
/// Положение вдоль трассы в метрах от начала пикетажа.
///
/// Например:
/// 435,6 м соответствует ПК4+35,6.
/// Номер трассы сюда не входит и хранится в GasAlignment.
/// </summary>
public readonly record struct Station
{
    /// <summary>
    /// Расстояние от начала трассы в метрах.
    /// </summary>
    public double Value { get; }

    public Station(double value)
    {
        if (double.IsNaN(value) || double.IsInfinity(value))
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "Пикетаж должен быть конечным числом.");
        }

        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(value),
                "Пикетаж не может быть отрицательным.");
        }

        Value = value;
    }

    /// <summary>
    /// Форматирует пикет без имени трассы:
    /// ПК4+35,6
    /// ПК4+0,0
    /// </summary>
    public override string ToString()
    {
        int fullPickets = (int)Math.Floor(Value / 100.0);
        double remainder = Value - fullPickets * 100.0;

        // Защита от ошибок double около границы пикета:
        // 399,999999999 превращаем в ПК4+0,0, а не ПК3+100,0.
        remainder = Math.Round(remainder, 1, MidpointRounding.AwayFromZero);

        if (remainder >= 100.0)
        {
            fullPickets++;
            remainder = 0.0;
        }

        return string.Create(
            CultureInfo.GetCultureInfo("ru-RU"),
            $"ПК{fullPickets}+{remainder:0.0}");
    }

    /// <summary>
    /// Форматирует пикет совместно с именем трассы:
    /// 1ПК4+35,6.
    /// </summary>
    public string ToString(string alignmentName)
    {
        if (string.IsNullOrWhiteSpace(alignmentName))
        {
            throw new ArgumentException(
                "Имя трассы не может быть пустым.",
                nameof(alignmentName));
        }

        string stationText = ToString();

        // GasAlignment называется "1ПК", поэтому повторное "ПК"
        // из начала пикетажа удаляем.
        if (alignmentName.Trim().EndsWith(
                "ПК",
                StringComparison.OrdinalIgnoreCase))
        {
            return $"{alignmentName.Trim()}{stationText[2..]}";
        }

        return $"{alignmentName.Trim()}{stationText}";
    }

    public static double operator -(Station left, Station right)
    {
        return left.Value - right.Value;
    }
}