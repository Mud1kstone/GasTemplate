namespace GasTemplate.Core.Models;

/// <summary>
/// Инженерная трасса газопровода.
///
/// Каждая трасса соответствует отдельной системе пикетажа:
/// 1ПК, 2ПК, 3ПК и так далее.
///
/// В дальнейшем трасса будет связана с Alignment
/// и продольным профилем Autodesk Civil 3D.
/// </summary>
public sealed class GasAlignment
{
    /// <summary>
    /// Уникальный идентификатор трассы внутри проекта.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Инженерное имя трассы: 1ПК, 2ПК, 3ПК и так далее.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Начальный пикет трассы в метрах.
    /// Для новой трассы обычно равен 0.
    /// </summary>
    public double StartStation { get; }

    /// <summary>
    /// Конечный пикет трассы в метрах.
    /// Например, 435.6 соответствует пикету 4+35,6.
    /// </summary>
    public double EndStation { get; private set; }

    /// <summary>
    /// Наименование основного продольного профиля.
    /// Одна трасса имеет один основной профиль.
    /// </summary>
    public string? ProfileName { get; private set; }

    public GasAlignment(
        string name,
        double endStation,
        string? profileName = null)
    {
        Id = Guid.NewGuid();
        StartStation = 0;

        Rename(name);
        SetEndStation(endStation);
        SetProfileName(profileName);
    }

    /// <summary>
    /// Изменяет инженерное имя трассы.
    /// </summary>
    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(
                "Имя трассы не может быть пустым.",
                nameof(name));
        }

        Name = name.Trim();
    }

    /// <summary>
    /// Устанавливает конечный пикет трассы.
    /// </summary>
    public void SetEndStation(double endStation)
    {
        if (endStation < StartStation)
        {
            throw new ArgumentOutOfRangeException(
                nameof(endStation),
                "Конечный пикет не может быть меньше начального.");
        }

        EndStation = endStation;
    }

    /// <summary>
    /// Назначает основной продольный профиль.
    /// </summary>
    public void SetProfileName(string? profileName)
    {
        ProfileName = string.IsNullOrWhiteSpace(profileName)
            ? null
            : profileName.Trim();
    }

    /// <summary>
    /// Возвращает общую длину трассы по пикетажу.
    /// </summary>
    public double Length => EndStation - StartStation;
}