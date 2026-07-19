namespace GasTemplate.Core.Models;

/// <summary>
/// Газификационный проект.
/// Содержит все трассы, входящие в проект.
/// </summary>
public sealed class GasProject
{
    /// <summary>
    /// Уникальный идентификатор проекта.
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Наименование проекта.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Все трассы проекта.
    /// </summary>
    public List<GasAlignment> Alignments { get; }

    public GasProject(string name)
    {
        Id = Guid.NewGuid();

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя проекта не может быть пустым.");

        Name = name.Trim();

        Alignments = new List<GasAlignment>();
    }

    public void Rename(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Имя проекта не может быть пустым.");

        Name = name.Trim();
    }

    public void AddAlignment(GasAlignment alignment)
    {
        ArgumentNullException.ThrowIfNull(alignment);

        if (Alignments.Any(a => a.Name == alignment.Name))
            throw new InvalidOperationException(
                $"Трасса '{alignment.Name}' уже существует.");

        Alignments.Add(alignment);
    }
}