using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();

    public static T GetService<T>() where T : class
    {
        return Services[typeof(T)] as T;
    }

    public static void AddService<T>(object service)
    {
        Services[typeof(T)] = service;
    }
}
