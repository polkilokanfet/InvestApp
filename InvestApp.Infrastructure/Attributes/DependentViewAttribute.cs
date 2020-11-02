using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class DependentViewAttribute : Attribute
{
    public string Region { get; }
    public Type Type { get; }
    /// <summary>
    /// У зависимого вида тот же контекст, что и у основного
    /// </summary>
    public bool HasSameDataContext { get; }

    /// <summary>
    /// Атрибут зависимого вида.
    /// </summary>
    /// <param name="region">В какой регион внедрять</param>
    /// <param name="type">Тип зависимого вида</param>
    /// <param name="hasSameDataContext">У зависимого вида тот же контекст, что и у основного.</param>
    public DependentViewAttribute(string region, Type type, bool hasSameDataContext = true)
    {
        if (region == null)
            throw new ArgumentNullException(nameof(region));

        if (type == null)
            throw new ArgumentNullException(nameof(type));

        Region = region;
        Type = type;
        HasSameDataContext = hasSameDataContext;
    }
}