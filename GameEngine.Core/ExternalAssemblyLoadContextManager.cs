using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Loader;

namespace GameEngine.Core; 

public class ExternalAssemblyLoadContextManager : IDisposable {
    
    private ExternalAssemblyLoadContext1? _externalAssemblyLoadContext;
    private List<(WeakReference lifetimeDependency, MulticastDelegate @delegate)> _unloadLifetimeDelegates = new();
    private List<Func<bool>> _unloadDelegates = new();
    
    public IEnumerable<Assembly> ExternalAssemblies {
        get {
            if(_externalAssemblyLoadContext is null)
                yield break;
            foreach(Assembly assembly in _externalAssemblyLoadContext.Assemblies) {
                yield return assembly;
            }
        }
    }
    
    public void LoadExternalAssembly(string assemblyPath, bool isDependency) {
        _externalAssemblyLoadContext ??= new ExternalAssemblyLoadContext1();
        _externalAssemblyLoadContext.LoadFromAssemblyPath(assemblyPath);
        if(isDependency)
            _externalAssemblyLoadContext.AddDependency(assemblyPath);
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    public void Unload() {
        if(_externalAssemblyLoadContext is null)
            return;
        
        InvokeUnloadDelegate();
        
        UnloadInternal(out WeakReference externalAssemblyLoadContextRef);
        
        const int MAX_GC_ATTEMPTS = 10;
        for(int i = 0; externalAssemblyLoadContextRef.IsAlive; i++) {
            if(i >= MAX_GC_ATTEMPTS) {
                Console.LogError($"Failed to unload external assemblies!");
                _externalAssemblyLoadContext = externalAssemblyLoadContextRef.Target as ExternalAssemblyLoadContext1;
                return;
            }
            Console.Log($"GC Attempt ({i + 1}/{MAX_GC_ATTEMPTS})...");
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
        
        Console.LogSuccess($"Successfully unloaded external assemblies!");
    }
    
    [MethodImpl(MethodImplOptions.NoInlining)]
    private void UnloadInternal(out WeakReference externalAssemblyLoadContextRef) {
        foreach(Assembly assembly in ExternalAssemblies) {
            Console.Log($"Unloading external assembly from: '{assembly.Location}'...");
        }
        
        // crashes after recovery and attempted unloading for the second time
        _externalAssemblyLoadContext.Unload();
        
        externalAssemblyLoadContextRef = new WeakReference(_externalAssemblyLoadContext);
        _externalAssemblyLoadContext = null;
    }
    
    public void AddUnloadTask(Func<bool> @delegate) {
        _unloadDelegates.Add(@delegate);
    }
    
    public void AddUnloadTaskWithLifetime<T>(T lifetimeDependency, Func<T, bool> @delegate) {
        _unloadLifetimeDelegates.Add((new WeakReference(lifetimeDependency), @delegate));
    }
    
    private void InvokeUnloadDelegate() {
        foreach((WeakReference lifetimeDependency, MulticastDelegate @delegate) in _unloadLifetimeDelegates) {
            if(!lifetimeDependency.IsAlive)
                continue;
            
            bool result = (bool) @delegate.DynamicInvoke(new object?[] { lifetimeDependency.Target })!;
            if(!result)
                Console.LogError("some unload delegate returned with failure");
        }
        _unloadLifetimeDelegates = new();
        
        foreach(Func<bool> @delegate in _unloadDelegates) {
            bool result = @delegate.Invoke();
            if(!result)
                Console.LogError("some unload delegate returned with failure");
        }
        _unloadDelegates = new();
    }
    
    void IDisposable.Dispose() {
        UnloadInternal(out WeakReference _);
    }
    
    private class ExternalAssemblyLoadContext1 : AssemblyLoadContext {
        
        private readonly List<AssemblyDependencyResolver> _assemblyDependencyResolvers;
        
        public ExternalAssemblyLoadContext1() : base(true) {
            _assemblyDependencyResolvers = new List<AssemblyDependencyResolver>();
        }
        
        public void AddDependency(string assemblyPath) {
            _assemblyDependencyResolvers.Add(new AssemblyDependencyResolver(assemblyPath));
        }
        
        protected override Assembly? Load(AssemblyName assemblyName) {
            foreach(AssemblyDependencyResolver assemblyDependencyResolver in _assemblyDependencyResolvers) {
                if(assemblyDependencyResolver.ResolveAssemblyToPath(assemblyName) is {} resolvedAssemblyPath)
                    return LoadFromAssemblyPath(resolvedAssemblyPath);
            }
            return null;
        }
        
    }
    
}
