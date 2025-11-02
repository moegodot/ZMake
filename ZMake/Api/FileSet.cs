using System.Collections;
using System.Collections.Frozen;
using M31.FluentApi.Attributes;

namespace ZMake.Api;

[FluentApi]
public sealed class FileSet : ISet<string>
{
    [FluentMember(0)]
    public FileType Type { get; init; }

    public FileSet(FileType type,FrozenSet<string> fileSet)
    {
        Files = fileSet;
        Type = type;
    }

    [FluentMember(1)]
    private FrozenSet<string> Files { get; init; }

    public IEnumerator<string> GetEnumerator()
    {
        return Files.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)Files).GetEnumerator();
    }

    void ICollection<string>.Add(string item)
    {
        throw new NotSupportedException();
    }

    public void ExceptWith(IEnumerable<string> other)
    {
        throw new NotSupportedException();
    }

    public void IntersectWith(IEnumerable<string> other)
    {
        throw new NotSupportedException();
    }

    public bool IsProperSubsetOf(IEnumerable<string> other)
    {
        return Files.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<string> other)
    {
        return Files.IsProperSupersetOf(other);
    }

    public bool IsSubsetOf(IEnumerable<string> other)
    {
        return Files.IsSubsetOf(other);
    }

    public bool IsSupersetOf(IEnumerable<string> other)
    {
        return Files.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<string> other)
    {
        return Files.Overlaps(other);
    }

    public bool SetEquals(IEnumerable<string> other)
    {
        return Files.SetEquals(other);
    }

    public void SymmetricExceptWith(IEnumerable<string> other)
    {
        throw new NotSupportedException();
    }

    public void UnionWith(IEnumerable<string> other)
    {
        throw new NotSupportedException();
    }

    bool ISet<string>.Add(string item)
    {
        throw new NotSupportedException();
    }

    public void Clear()
    {
        throw new NotSupportedException();
    }

    public bool Contains(string item)
    {
        return Files.Contains(item);
    }

    public void CopyTo(string[] array, int arrayIndex)
    {
        Files.CopyTo(array, arrayIndex);
    }

    public bool Remove(string item)
    {
        throw new NotSupportedException();
    }

    public int Count => Files.Count;

    public bool IsReadOnly => true;
}
