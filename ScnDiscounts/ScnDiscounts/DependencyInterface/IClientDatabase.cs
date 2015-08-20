using System;

namespace ScnDiscounts.DependencyInterface
{
    public interface IClientDatabase
    {
        string GetPath(string fileName);
    }
}