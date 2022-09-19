﻿using Autofac;
using BitMono;
using BitMono.API.Protections;
using dnlib.DotNet;
using dnlib.DotNet.MD;
using dnlib.DotNet.Writer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

public class Program
{
    private static async Task Main(string[] args)
    {
        var file = args[0];
        var encryptionFile = "BitMono.Encryption.dll";

        var moduleDefMD = ModuleDefMD.Load(file, new ModuleCreationOptions(CLRRuntimeReaderKind.Mono));
        var encryptionModuleDefMD = ModuleDefMD.Load(encryptionFile);

        var moduleWriterOptions = new ModuleWriterOptions(moduleDefMD);
        var container = new Application().BuildContainer();
        var protectionContext = new ProtectionContext(moduleDefMD: moduleDefMD,
                                                      moduleWriterOptions: moduleWriterOptions,
                                                      encryptionModuleDefMD: encryptionModuleDefMD,
                                                      targetAssembly: Assembly.ReflectionOnlyLoadFrom(file));

        using (container)
        {
            var protections = container.Resolve<ICollection<IProtection>>(); 
            foreach (var protection in protections)
            {
                Console.WriteLine("Executing protection! " + protection.GetType().FullName);
                await protection.ExecuteAsync(protectionContext);
            }
        }

        Console.WriteLine("[+] Writing..");
        moduleWriterOptions.MetadataLogger = DummyLogger.NoThrowInstance;
        moduleWriterOptions.MetadataOptions.Flags |= MetadataFlags.KeepOldMaxStack;
        moduleWriterOptions.Cor20HeaderOptions.Flags = ComImageFlags.ILOnly;
        file = new StringBuilder()
            .Append(Path.GetFileNameWithoutExtension(file))
            .Append("_bitmono")
            .Append(Path.GetExtension(file))
            .ToString();
        moduleDefMD.Write(file, moduleWriterOptions);
        Console.WriteLine("[+] Writing done!");

        Console.ReadLine();
    }
}